using UnityEngine;
using Laresistance.Movement;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Interaction;
using Laresistance.Behaviours.Platforms;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
    public class PlayerMapBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ScriptableFloatReference horizontalSpeed = default;
        [SerializeField]
        private ScriptableFloatReference jumpForce = default;
        [SerializeField]
        private ScriptableIntReference jumpsLimit = default;
        [SerializeField]
        private GameEvent platformFallEvent = default;

        private Rigidbody2D body;
        private Animator animator;

        private PlayerMovementStatus movementStatus;
        private PlayerHorizontalMovement horizontalMovement;
        private PlayerJump jump;
        private PlayerScenarioInteraction playerScenarioInteraction;

        private ScenarioInteraction currentInteraction = null;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            movementStatus = new PlayerMovementStatus();
            horizontalMovement = new PlayerHorizontalMovement(transform, body, movementStatus, animator, horizontalSpeed);
            jump = new PlayerJump(transform, body, movementStatus, animator, jumpForce, platformFallEvent, jumpsLimit);
            playerScenarioInteraction = new PlayerScenarioInteraction();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            horizontalMovement.Tick(delta);
            jump.Tick(delta);
        }

        public void Move(InputAction.CallbackContext context) => horizontalMovement.Move(context);
        public void Jump(InputAction.CallbackContext context) => jump.Jump(context);
        public void PlatformFall(InputAction.CallbackContext context) => jump.Fall(context);
        public void Interact(InputAction.CallbackContext context) => playerScenarioInteraction.Interact(context, currentInteraction);

        public bool IsJumping => movementStatus.jumping;
        public bool IsFalling => movementStatus.falling;
        public bool IsPlatformFalling => movementStatus.platformFalling;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.GetContact(0).normal.y > 0.9f)
            {
                jump.GroundContact();
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