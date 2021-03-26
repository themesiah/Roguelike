using GamedevsToolbox.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class Character2DController : MonoBehaviour, IPausable
    {
        private static float GROUNDED_CERCLE_RADIUS = 0.2f;

        [SerializeField]
        private Rigidbody2D body = default;

        [SerializeField]
        private Transform groundCheck = default;

        [SerializeField]
        private LayerMask groundLayers = default;

        [Header("Events")]
        public UnityEvent<Vector3> OnLand = default;
        public UnityEvent<Vector3> OnJump = default;
        public UnityEvent<bool> OnFlip = default;

        // State
        private bool isGrounded = false;
        private bool facingRight = true;
        private int performedJumps = 0;
        private bool isPaused = false;

        // Accessors
        public bool IsJumpingOrFalling => !isGrounded;
        public bool IsGrounded => isGrounded;
        public Vector2 CurrentVelocity => body.velocity;
        public bool FacingRight => facingRight;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(body);
            UnityEngine.Assertions.Assert.IsNotNull(groundCheck);
            facingRight = transform.localScale.x > 0f ? true : false;
            Flip(!FacingRight);
        }

        public void Jump(Vector2 force, int jumpLimit)
        {
            if (isPaused)
                return;
            if (isGrounded || performedJumps < jumpLimit)
            {
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
            }
        }

        public void StopJump()
        {
            if (isPaused)
                return;
            if (!isGrounded && body.velocity.y > 0f)
            {
                var vel = body.velocity;
                vel.y = 0f;
                body.velocity = vel;
            }
        }

        public void Move(float movement)
        {
            if (isPaused)
                return;
            Vector3 targetVelocity = new Vector2(movement, body.velocity.y);
            body.velocity = targetVelocity;
            if (movement > 0 && !facingRight)
            {
                Flip();
            }
            else if (movement < 0 && facingRight)
            {
                Flip();
            }
        }

        public void Flip()
        {
            facingRight = !facingRight;
            var scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }

        public void Flip(bool right)
        {
            if (facingRight != right)
            {
                Flip();
            }
        }

        private void FixedUpdate()
        {
            if (isPaused)
                return;
            CheckGrounded();
        }

        private void CheckGrounded()
        {
            bool wasGrounded = isGrounded;
            isGrounded = false;

            if (body.velocity.y <= 0f)
            {
                Collider2D[] colliders = GetGroundColliders();
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        isGrounded = true;
                        if (!wasGrounded)
                        {
                            performedJumps = 0;
                            OnLand?.Invoke(groundCheck.position);
                        }
                        break;
                    }
                }
            }
        }

        public Collider2D[] GetGroundColliders()
        {
            return Physics2D.OverlapCircleAll(groundCheck.position, GROUNDED_CERCLE_RADIUS, groundLayers);
        }

        public void Pause()
        {
            isPaused = true;
            body.simulated = false;
            body.velocity = Vector3.zero;
        }

        public void Resume()
        {
            isPaused = false;
            body.simulated = true;
        }
    }
}