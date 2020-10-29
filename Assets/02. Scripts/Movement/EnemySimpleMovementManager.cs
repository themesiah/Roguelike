using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private bool right = true;
        private bool stopped = false;

        public EnemySimpleMovementManager(Rigidbody2D body, ScriptableFloatReference movementSpeed, int raycastLayerMask, Transform raycastPivot)
        {
            this.body = body;
            this.movementSpeed = movementSpeed;
            this.raycastLayerMask = raycastLayerMask;
            this.raycastPivot = raycastPivot;
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
            body.velocity = Vector2.zero;
            stopped = true;
        }

        public void Resume()
        {
            stopped = false;
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

        private void Turn()
        {
            right = !right;
            var scale = body.transform.localScale;
            scale.x *= -1f;
            body.transform.localScale = scale;
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