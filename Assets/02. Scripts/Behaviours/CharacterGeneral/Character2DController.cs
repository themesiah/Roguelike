using GamedevsToolbox.ScriptableArchitecture.Events;
using GamedevsToolbox.Utils;
using Laresistance.Behaviours.Platforms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class Character2DController : MonoBehaviour, IPausable
    {
        private static float JUMP_ATTEMPT_TIMER = 0.15f;
        private static float JUMP_GRACE_TIME = 0.2f;

        [Header("References")]
        [SerializeField]
        private Rigidbody2D body = default;
        [SerializeField]
        private Transform groundCheck = default;
        [SerializeField]
        private PhysicsMaterial2D nonFrictionMaterial = default;
        [SerializeField]
        private PhysicsMaterial2D frictionMaterial = default;

        [Header("Configuration")]
        [SerializeField]
        private LayerMask groundLayers = default;
        [SerializeField]
        private float groundedCircleRadius = 0.2f;
        [SerializeField] [Tooltip("If the object tries to jump when it is not grounded or do not have more available jumps, it waits JUMP_ATTEMPT_TIMER time for retry the jump, augmenting responsivity.")]
        private bool allowJumpAttempt = false;
        [SerializeField]
        private float notGroundedTimeToBeNotGrounded = 3f / 60f;
        [SerializeField]
        private float delayAfterLanding = 0.3f;
        [SerializeField]
        private float slopeCheckDistance = 0.5f;
        [SerializeField]
        private float maxSlopeAngle = 45f;
        [SerializeField]
        private float timeToFall = 0.1f;

        [Header("Events")]
        public UnityEvent<Vector3> OnLand = default;
        public UnityEvent<Vector3> OnStartFall = default;
        public UnityEvent<Vector3> OnJump = default;
        public UnityEvent<bool> OnFlip = default;
        public UnityEvent<float> OnHorizontalVelocityChanged = default;
        public UnityEvent<float> OnVerticalVelocityChanged = default;

        // State
        // Movement state
        private float currentMovement = 0f;
        // Jump and fall state
        private bool isGrounded = false;
        private bool falling = false;
        private int performedJumps = 0;
        private float notGroundedTime = 0f;
        private float delayAfterLandingTimer = 0f;
        private bool jumping = false;
        private float fallingTime = 0f;
        private bool canJump = true;
        private bool canJumpSignal = true;
        // Orientation state
        private bool facingRight = true;
        // Meta state
        private bool isPaused = false;
        private Vector2 beforePauseVelocity;
        // Slope state
        private bool isOnSlope = false;
        private Vector2 slopeNormalPerpendicular;
        private float slopeDownAngle;
        private float slopeSideAngle;
        private bool canWalkOnSlope;
        // Jump Attempt state
        private float jumpAttemptTimer = 0f;
        private Vector2 jumpAttemptForce = Vector2.zero;
        private int jumpAttemptLimit = 0;

        // Public accessors
        public bool IsJumpingOrFalling => !isGrounded;
        public bool IsGrounded => isGrounded;
        public Vector2 CurrentVelocity => body.velocity;
        public bool FacingRight => facingRight;
        public bool IsJumping => jumping;
        public bool IsFalling => falling;
        // Slope accessors
        public bool IsOnSlope => isOnSlope;
        public float SlopeDownAngle => slopeDownAngle;
        public float SlopeSideAngle => slopeSideAngle;
        public bool CanWalkOnSlope => canWalkOnSlope;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(body);
            UnityEngine.Assertions.Assert.IsNotNull(groundCheck);
            facingRight = true;
            Flip(!FacingRight);
        }

        private bool HaveAfterLandDelay()
        {
            return delayAfterLandingTimer > 0f;
        }

        public void Jump(Vector2 force, int jumpLimit)
        {
            if (isPaused)
                return;
            if (HaveAfterLandDelay())
                return;
            if (!canJump)
                return;
            if (isGrounded || performedJumps < jumpLimit)
            {
                jumpAttemptTimer = 0f;
                // No longer in the ground
                isGrounded = false;
                // Remove Y velocity, for in mid air jumps
                var vel = body.velocity;
                vel.y = 0f;
                body.velocity = vel;
                // Add the impulse
                body.AddForce(force, ForceMode2D.Impulse);
                // Manage events
                OnJump?.Invoke(groundCheck.position);
                // Increase number of jumps done in mid air
                performedJumps++;
                jumping = true;
            } else if (!isGrounded && allowJumpAttempt)
            {
                jumpAttemptTimer = JUMP_ATTEMPT_TIMER;
                jumpAttemptForce = force;
                jumpAttemptLimit = jumpLimit;
            }
        }

        public void StopJump()
        {
            if (isPaused)
                return;
            jumpAttemptTimer = 0f;
            if (!isGrounded && body.velocity.y > 0f)
            {
                var vel = body.velocity;
                vel.y *= 0.5f;
                body.velocity = vel;
            }
        }

        private void ApplyMovement()
        {
            Vector2 targetVelocity = Vector2.zero;
            if (isGrounded && !isOnSlope)
            {
                targetVelocity = new Vector2(currentMovement, 0f);
            }
            else if (isGrounded && isOnSlope)
            {
                targetVelocity = -slopeNormalPerpendicular.normalized * currentMovement;
                //targetVelocity = new Vector2(currentMovement * -slopeNormalPerpendicular.x, currentMovement * -slopeNormalPerpendicular.y);
            }
            else if (!isGrounded)
            {
                targetVelocity = new Vector2(currentMovement, body.velocity.y);
            }


            body.velocity = targetVelocity;

            currentMovement = 0f;
        }

        public void Move(float movement)
        {
            if (isPaused)
                return;
            if (HaveAfterLandDelay())
                return;

            currentMovement = movement;
        }

        public void Flip()
        {
            facingRight = !facingRight;
            OnFlip?.Invoke(facingRight);
        }

        public void Flip(bool right)
        {
            if (facingRight != right)
            {
                Flip();
            }
        }

        private void CheckFlip()
        {
            if (currentMovement > 0f && !facingRight)
            {
                Flip();
            }
            else if (currentMovement < 0f && facingRight)
            {
                Flip();
            }
        }

        private void FixedUpdate()
        {
            if (isPaused)
                return;
            CheckFlip();
            CheckSlope();
            CheckGrounded();
            CheckJumpAttempt();
            CheckNotGroundedTime();
            CheckJumpGraceTime();
            ApplyMovement();
            if (delayAfterLandingTimer > 0f)
            {
                delayAfterLandingTimer -= Time.fixedDeltaTime;
            }
        }

        private void CheckGrounded()
        {
            bool wasGrounded = isGrounded;
            isGrounded = false;

            OnVerticalVelocityChanged?.Invoke(body.velocity.y);
            OnHorizontalVelocityChanged?.Invoke(body.velocity.x);
            bool collided = false;
            if (canJumpSignal == true)
            {
                canJump = canJumpSignal;
            }
            if (!canJump)
            {
                isGrounded = true;
                falling = false;
                collided = true;
            } else
            if ((body.velocity.y <= 0f || isOnSlope && !jumping))
            {
                Collider2D[] colliders = GetGroundColliders();
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        collided = true;
                        isGrounded = true;
                        falling = false;
                        fallingTime = 0f;
                        if (canJumpSignal == false)
                        {
                            canJump = canJumpSignal;
                        }
                        if (!wasGrounded && notGroundedTime >= notGroundedTimeToBeNotGrounded)
                        {
                            delayAfterLandingTimer = delayAfterLanding;
                            performedJumps = 0;
                            OnLand?.Invoke(groundCheck.position);
                            var raycastHit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.3f, groundLayers);
                            if (raycastHit.transform == null)
                            {
                                // Bad landing
                                //Tenerlo activado solo trae problemas. Comentado por ahora.
                                //foreach (var collider in colliders)
                                //{
                                //    OneWayPlatform platform = collider.GetComponentInChildren<OneWayPlatform>();
                                //    if (platform != null)
                                //    {
                                //        platform.ActivateFallingPlatform(); 
                                //        break;
                                //    }
                                //}
                                isGrounded = false;
                                falling = true;
                                //platformFallEvent?.Raise();
                            } else
                            {
                                if (delayAfterLanding > 0f && raycastHit.normal.y == 1f)
                                {
                                    body.velocity = Vector3.zero;
                                    OnHorizontalVelocityChanged?.Invoke(0f);
                                }
                            }
                        }
                        break;
                    }
                }
            }
            if (!collided && body.velocity.y <= 0f)
            {
                if (falling == false && !jumping && fallingTime < timeToFall)
                {
                    fallingTime += Time.fixedDeltaTime;
                    isGrounded = true;
                } else if (falling == false)
                {
                    fallingTime = 0f;
                    falling = true;
                    jumping = false;
                    OnStartFall?.Invoke(groundCheck.position);
                }
            }
        }

        private void CheckSlope()
        {
            // Checks if there is a slope on the right or the left. Just to check if you can walk there or not, but does not affect movement on current position.
            CheckSlopeHorizontal(groundCheck.position);
            // Checks if you are on top of a slope.
            CheckSlopeVertical(groundCheck.position);
        }

        private void CheckSlopeHorizontal(Vector2 checkPos)
        {
            RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, groundCheck.right, slopeCheckDistance, groundLayers);
            RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -groundCheck.right, slopeCheckDistance, groundLayers);

            if (slopeHitFront)
            {
                isOnSlope = true;

                slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

            }
            else if (slopeHitBack)
            {
                isOnSlope = true;

                slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
            }
            else
            {
                slopeSideAngle = 0.0f;
                isOnSlope = false;
            }
        }

        private void CheckSlopeVertical(Vector2 checkPos)
        {
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayers);

            if (hit)
            {

                slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;

                slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeDownAngle != 0f)
                {
                    isOnSlope = true;
                }
#if UNITY_EDITOR
                Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.blue);
                Debug.DrawRay(hit.point, hit.normal, Color.green);
#endif

            } else
            {
                slopeDownAngle = 0f;
            }

            if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
            {
                canWalkOnSlope = false;
            }
            else
            {
                canWalkOnSlope = true;
            }

            if (isOnSlope && canWalkOnSlope && currentMovement == 0f)
            {
                body.sharedMaterial = frictionMaterial;
            }
            else
            {
                body.sharedMaterial = nonFrictionMaterial;
            }
        }

        private void CheckJumpAttempt()
        {
            if (jumpAttemptTimer > 0f)
            {
                jumpAttemptTimer -= Time.deltaTime;
                if (jumpAttemptTimer > 0f && isGrounded)
                {
                    Jump(jumpAttemptForce, jumpAttemptLimit);
                }
            }
        }

        private void CheckNotGroundedTime()
        {
            if (isGrounded)
            {
                notGroundedTime = 0f;
            }
            else
            {
                notGroundedTime += Time.deltaTime;
            }
        }

        private void CheckJumpGraceTime()
        {
            if (!isGrounded)
            {
                if (notGroundedTime >= JUMP_GRACE_TIME && performedJumps == 0)
                {
                    performedJumps = 1;
                }
            }
        }

        public Collider2D[] GetGroundColliders()
        {
            return Physics2D.OverlapCircleAll(groundCheck.position, groundedCircleRadius, groundLayers);
        }

        public void SetCanJump(bool canJump)
        {
            this.canJumpSignal = canJump;
        }

        public void StartChangingRoom()
        {
            OnHorizontalVelocityChanged?.Invoke(100f);
        }

        public void Pause()
        {
            isPaused = true;
            body.simulated = false;
            beforePauseVelocity = body.velocity;
            body.velocity = Vector3.zero;
            OnHorizontalVelocityChanged?.Invoke(0f);
            OnVerticalVelocityChanged?.Invoke(0f);
        }

        public void Resume()
        {
            isPaused = false;
            body.simulated = true;
            body.velocity = beforePauseVelocity;
        }
    }
}