using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Movement
{
    public class EnemySimpleMovementManager : IMovementManager
    {
        private EnemyMapData enemyMapData;
        private int raycastLayerMask;
        private Transform raycastPivot;
        private Character2DController characterController;

        private int pauseStack = 0;

        public EnemySimpleMovementManager(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot)
        {
            this.characterController = characterController;
            this.enemyMapData = enemyMapData;
            this.raycastLayerMask = raycastLayerMask;
            this.raycastPivot = raycastPivot;
        }

        public void Tick(float delta)
        {
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
            characterController.Pause();
        }

        public void Resume()
        {
            pauseStack--;
            pauseStack = System.Math.Max(0, pauseStack);
            if (pauseStack == 0)
            {
                characterController.Resume();
            }
        }

        private bool CheckRaycastNeedToTurn()
        {
            if (!characterController.FacingRight)
            {
                // Left
                if (Physics2D.Raycast(raycastPivot.position, Vector3.left, GameConstantsBehaviour.Instance.enemyMapHorizontalRaycastLength.GetValue(), raycastLayerMask) || Physics2D.Raycast(characterController.transform.position + Vector3.up * GameConstantsBehaviour.Instance.enemyMapHorizontalRaycastOffsetUp.GetValue(), Vector3.left, GameConstantsBehaviour.Instance.enemyMapHorizontalRaycastLength.GetValue(), raycastLayerMask))
                {
                    return true;
                }
                // Left down
                if (!Physics2D.Raycast(raycastPivot.position + Vector3.left * GameConstantsBehaviour.Instance.enemyMapVerticalRaycastOffset.GetValue(), Vector3.down, GameConstantsBehaviour.Instance.enemyMapVerticalRaycastLength.GetValue(), raycastLayerMask))
                {
                    return true;
                }
            }
            else
            {
                // Right
                if (Physics2D.Raycast(raycastPivot.position, Vector3.right, GameConstantsBehaviour.Instance.enemyMapHorizontalRaycastLength.GetValue(), raycastLayerMask) || Physics2D.Raycast(characterController.transform.position + Vector3.up * GameConstantsBehaviour.Instance.enemyMapHorizontalRaycastOffsetUp.GetValue(), Vector3.right, GameConstantsBehaviour.Instance.enemyMapHorizontalRaycastLength.GetValue(), raycastLayerMask))
                {
                    return true;
                }
                // Right down
                if (!Physics2D.Raycast(raycastPivot.position + Vector3.right * GameConstantsBehaviour.Instance.enemyMapVerticalRaycastOffset.GetValue(), Vector3.down, GameConstantsBehaviour.Instance.enemyMapVerticalRaycastLength.GetValue(), raycastLayerMask))
                {
                    return true;
                }
            }
            return false;
        }

        private void Turn()
        {
            characterController.Flip();
        }

        private void Turn(bool r)
        {
            characterController.Flip(r);
        }

        private void Move()
        {
            float modifier = characterController.FacingRight ? 1f : -1f;
            float movement = modifier * enemyMapData.Speed;
            characterController.Move(movement);
        }
    }
}