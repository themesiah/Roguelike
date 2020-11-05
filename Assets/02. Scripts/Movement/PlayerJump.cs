using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laresistance.Movement
{
    public class PlayerJump
    {
        private Transform transform;
        private Rigidbody2D body;
        private PlayerMovementStatus movementStatus;
        private Animator animator;
        private ScriptableFloatReference jumpForce;
        private GameEvent platformFallEvent;
        private int jumpsDone = 0;
        private ScriptableIntReference jumpsLimit;

        public PlayerJump(Transform transform, Rigidbody2D body, PlayerMovementStatus movementStatus, Animator animator, ScriptableFloatReference jumpForce, GameEvent platformFallEvent, ScriptableIntReference jumpsLimit)
        {
            this.transform = transform;
            this.body = body;
            this.movementStatus = movementStatus;
            this.animator = animator;
            this.jumpForce = jumpForce;
            this.platformFallEvent = platformFallEvent;
            this.jumpsLimit = jumpsLimit;
        }

        public void Tick(float delta)
        {
            if (body.velocity.y < -0.1f && movementStatus.falling == false)
            {
                movementStatus.falling = true;
                movementStatus.jumping = false;
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if ((context.canceled || context.performed) && !movementStatus.falling)
            {
                body.velocity = Vector2.right * body.velocity.x;
                movementStatus.jumping = false;
                movementStatus.falling = true;
                movementStatus.platformFalling = false;
            }
            else if (context.started && jumpsDone < jumpsLimit.GetValue())
            {
                if (movementStatus.platformFalling)
                {
                    platformFallEvent.Raise();
                    movementStatus.platformFalling = false;
                }
                else
                {
                    body.velocity = Vector3.zero;
                    body.AddForce(Vector2.up * jumpForce.GetValue(), ForceMode2D.Impulse);
                    movementStatus.jumping = true;
                    movementStatus.falling = false;
                    jumpsDone++;
                }
            }
        }

        public void Fall(InputAction.CallbackContext context)
        {
            if (context.started && !movementStatus.jumping && !movementStatus.falling)
                movementStatus.platformFalling = true;
            if (context.canceled)
                movementStatus.platformFalling = false;
        }

        public void GroundContact()
        {
            movementStatus.falling = false;
            movementStatus.jumping = false;
            movementStatus.platformFalling = false;
            jumpsDone = 0;
        }
    }
}