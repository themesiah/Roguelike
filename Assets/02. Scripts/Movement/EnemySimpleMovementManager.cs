using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Movement
{
    public class EnemySimpleMovementManager : IMovementManager
    {
        private static float HORIZONTAL_RAYCAST_LENGTH = 1.5f;
        private static float HORIZONTAL_RAYCAST_OFFSET_UP = 1f;
        private static float VERTICAL_RAYCAST_LENGTH = 2f;
        private static float VERTICAL_RAYCAST_OFFSET = 0.1f;
        private Rigidbody2D body;
        private ScriptableFloatReference movementSpeed;
        private int raycastLayerMask;
        private Transform raycastPivot;
        private UnityAction<bool> onTurn;

        private bool right = true;
        private bool stopped = false;
        private int pauseStack = 0;

        public EnemySimpleMovementManager(Rigidbody2D body, ScriptableFloatReference movementSpeed, int raycastLayerMask, Transform raycastPivot, UnityAction<bool> onTurn)
        {
            this.body = body;
            this.movementSpeed = movementSpeed;
            this.raycastLayerMask = raycastLayerMask;
            this.raycastPivot = raycastPivot;
            this.onTurn = onTurn;
            if (body.transform.localScale.x > 0f)
            {
                right = false;
            }
        }

        public void Tick(float delta)
        {
            if (stopped)
                return;
            bool needToTurn = CheckRaycastNeedToTurn();
            if (needToTurn)
            {
                Turn();
            }
            Move();
        }

        public void Pause()
        {
            pauseStack++;
            body.velocity = Vector2.zero;
            body.simulated = false;
            stopped = true;
        }

        public void Resume()
        {
            pauseStack--;
            if (pauseStack == 0)
            {
                stopped = false;
                body.simulated = true;
            }
        }

        private bool CheckRaycastNeedToTurn()
        {
            if (!right)
            {
                // Left
                if (Physics2D.Raycast(raycastPivot.position, Vector3.left, HORIZONTAL_RAYCAST_LENGTH, raycastLayerMask) || Physics2D.Raycast(body.transform.position + Vector3.up * HORIZONTAL_RAYCAST_OFFSET_UP, Vector3.left, HORIZONTAL_RAYCAST_LENGTH, raycastLayerMask))
                {
                    return true;
                }
                // Left down
                if (!Physics2D.Raycast(raycastPivot.position + Vector3.left * VERTICAL_RAYCAST_OFFSET, Vector3.down, VERTICAL_RAYCAST_LENGTH, raycastLayerMask))
                {
                    return true;
                }
            }
            else
            {
                // Right
                if (Physics2D.Raycast(raycastPivot.position, Vector3.right, HORIZONTAL_RAYCAST_LENGTH, raycastLayerMask) || Physics2D.Raycast(body.transform.position + Vector3.up * HORIZONTAL_RAYCAST_OFFSET_UP, Vector3.right, HORIZONTAL_RAYCAST_LENGTH, raycastLayerMask))
                {
                    return true;
                }
                // Right down
                if (!Physics2D.Raycast(raycastPivot.position + Vector3.right * VERTICAL_RAYCAST_OFFSET, Vector3.down, VERTICAL_RAYCAST_LENGTH, raycastLayerMask))
                {
                    return true;
                }
            }
            return false;
        }

        public void Turn()
        {
            right = !right;
            var scale = body.transform.localScale;
            if (right == true)
            {
                scale.x = Mathf.Abs(scale.x);
            } else
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            body.transform.localScale = scale;
            onTurn?.Invoke(right);
        }

        public void Turn(bool r)
        {
            right = !r;
            Turn();
        }

        private void Move()
        {
            float modifier = 1f;
            if (right == false)
                modifier = -1f;
            var velocity = body.velocity;
            velocity.x = modifier * movementSpeed.GetValue();
            body.velocity = velocity;
        }

        public bool Right => right;
    }
}