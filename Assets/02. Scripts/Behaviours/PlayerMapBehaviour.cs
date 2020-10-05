using UnityEngine;
using Laresistance.Movement;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Interaction;
using Laresistance.Behaviours.Platforms;
using Laresistance.Data;

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

        protected override void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            base.Awake();
        }

        protected override IMovementManager CreateMovementManager()
        {
            return new PlayerMovementManager(movementData, transform, body, animator, platformFallEvent);
        }

        public void Move(InputAction.CallbackContext context) => ((PlayerMovementManager)movementManager).HorizontalMovement.Move(context);
        public void Jump(InputAction.CallbackContext context) => ((PlayerMovementManager)movementManager).Jump.Jump(context);
        public void PlatformFall(InputAction.CallbackContext context) => ((PlayerMovementManager)movementManager).Jump.Fall(context);
        public void Interact(InputAction.CallbackContext context) => ((PlayerMovementManager)movementManager).ScenarioInteraction.Interact(context, currentInteraction);

        public bool IsJumping => ((PlayerMovementManager)movementManager).MovementStatus.jumping;
        public bool IsFalling => ((PlayerMovementManager)movementManager).MovementStatus.falling;
        public bool IsPlatformFalling => ((PlayerMovementManager)movementManager).MovementStatus.platformFalling;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.GetContact(0).normal.y > 0.9f)
            {
                ((PlayerMovementManager)movementManager).Jump.GroundContact();
            }
        }

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