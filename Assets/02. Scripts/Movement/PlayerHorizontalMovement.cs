using UnityEngine;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Movement
{
    public class PlayerHorizontalMovement
    {
        private static float MOVEMENT_THRESHOLD = 0.3f;

        private Transform transform;
        private Rigidbody2D body;
        private PlayerMovementStatus status;
        private Animator animator;
        private ScriptableFloatReference speed;
        private float currentSpeed = 0f;
        private bool turn = false;

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
            if (turn)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                turn = false;
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            float axisValue = context.ReadValue<Vector2>().x;
            currentSpeed = 0f;
            if (context.performed && (axisValue > MOVEMENT_THRESHOLD || axisValue < -MOVEMENT_THRESHOLD))
            {
                //currentSpeed = context.ReadValue<Vector2>().x * speed.GetValue();
                currentSpeed = speed.GetValue() * Mathf.Sign(axisValue);
            }
            if ((currentSpeed < 0 && transform.localScale.x > 0) || (currentSpeed > 0 && transform.localScale.x < 0))
            {
                turn = true;
            }
        }

        public void Stop()
        {
            currentSpeed = 0f;
        }
    }
}