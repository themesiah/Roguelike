using UnityEngine;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Movement
{
    public class PlayerHorizontalMovement
    {
        private Transform transform;
        private Rigidbody2D body;
        private PlayerMovementStatus status;
        private Animator animator;
        private ScriptableFloatReference speed;
        private float currentSpeed = 0f;

        public PlayerHorizontalMovement(Transform transform, Rigidbody2D body, PlayerMovementStatus status, Animator animator, ScriptableFloatReference speed)
        {
            this.transform = transform;
            this.body = body;
            this.status = status;
            this.speed = speed;
            this.animator = animator;
        }

        public void Tick(float delta)
        {
            body.velocity = Vector2.right * currentSpeed + Vector2.up * body.velocity.y;
        }

        public void Move(InputAction.CallbackContext context)
        {
            float axisValue = context.ReadValue<Vector2>().x * speed.GetValue();
            if (context.canceled && axisValue == 0f)
            {
                currentSpeed = 0f;
            } else if (context.started && axisValue != 0f)
            {
                currentSpeed = context.ReadValue<Vector2>().x * speed.GetValue();
            }
            if ((currentSpeed < 0 && transform.localScale.x > 0) || (currentSpeed > 0 && transform.localScale.x < 0))
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }
}