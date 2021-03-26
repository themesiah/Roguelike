using UnityEngine;
using Laresistance.Movement;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Interaction;
using Laresistance.Behaviours.Platforms;
using Laresistance.Data;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
    public class PlayerMapBehaviour : MapBehaviour
    {
        [SerializeField]
        private PlayerMovementData movementData = default;
        [SerializeField]
        private GameEvent platformFallEvent = default;

        private Rigidbody2D body;
        private Animator animator;

        private ScenarioInteraction currentInteraction = null;
        private PlayerScenarioInteraction playerInteraction = null;

        private bool fallSignal = false;
        private bool jumpSignal = false;
        private bool stopJumpSignal = false;
        private float currentMovementValue = 0f;

        protected override void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerInteraction = new PlayerScenarioInteraction();
            base.Awake();
        }

        private void FixedUpdate()
        {
            characterController.Move(currentMovementValue);
            if (jumpSignal)
            {
                Vector2 jumpForce = Vector2.up * movementData.JumpForce.GetValue();
                characterController.Jump(jumpForce, movementData.JumpsLimit.GetValue());
                jumpSignal = false;
            }
            if (stopJumpSignal)
            {
                characterController.StopJump();
                stopJumpSignal = false;
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                float axisValue = context.ReadValue<Vector2>().x;
                if (Mathf.Abs(axisValue) > 0.3f)
                {
                    float movement = movementData.HorizontalSpeed.GetValue() * axisValue;
                    currentMovementValue = movement;
                } else
                {
                    currentMovementValue = 0f;
                }
            } else if (context.canceled)
            {
                currentMovementValue = 0f;
            }
        }
        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (fallSignal)
                {
                    var groundColliders = characterController.GetGroundColliders();
                    bool haveEffector = false;
                    // If not above a platform effector, jump normally, even if pressing down
                    foreach(var collider in groundColliders)
                    {
                        PlatformEffector2D effector = collider.GetComponent<PlatformEffector2D>();
                        if (effector != null)
                        {
                            haveEffector = true;
                            break;
                        }
                    }
                    if (!haveEffector)
                    {
                        jumpSignal = true;
                    } else
                    {
                        platformFallEvent?.Raise();
                    }
                } else
                {
                    jumpSignal = true;
                }
            } else if (context.canceled)
            {
                stopJumpSignal = true;
            }
        }
        public void PlatformFall(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                fallSignal = true;
            } else if (context.canceled)
            {
                fallSignal = false;
            }
        }

        public bool IsJumpingOrFalling => characterController.IsJumpingOrFalling;
        public Vector2 CurrentVelocity => characterController.CurrentVelocity;
        public bool FallingSignal => fallSignal;
        public void Interact(InputAction.CallbackContext context) => playerInteraction.Interact(context, currentInteraction, false);
        public void InteractEquip(InputAction.CallbackContext context) => playerInteraction.Interact(context, currentInteraction, true);

        private void OnTriggerEnter2D(Collider2D collider)
        {
            currentInteraction = collider.gameObject.GetComponent<ScenarioInteraction>();
            if (currentInteraction != null)
            {
                currentInteraction.EnterInteractionZone();
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (currentInteraction != null)
            {
                currentInteraction.ExitInteractionZone();
            }
            currentInteraction = null;
        }
    }
}