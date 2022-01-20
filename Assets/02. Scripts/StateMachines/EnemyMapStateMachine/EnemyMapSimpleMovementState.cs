using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;
using Laresistance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.StateMachines
{
    public class EnemyMapSimpleMovementState : EnemyMapState
    {
        private UnityEvent OnPlayerDiscovered;
        private UnityEvent OnPlayerLost;

        public EnemyMapSimpleMovementState(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, GameObject playerObject,
                                            UnityEvent onPlayerDiscovered, UnityEvent onPlayerLost)
            : base (characterController, enemyMapData, raycastLayerMask, raycastPivot, playerObject)
        {
            OnPlayerDiscovered = onPlayerDiscovered;
            OnPlayerLost = onPlayerLost;
        }

        public override IEnumerator EnterState()
        {
            characterController.Move(0f);
            OnPlayerLost?.Invoke();
            yield return new WaitForSeconds(1f);
        }

        public override IEnumerator ExitState()
        {
            characterController.Move(0f);
            OnPlayerDiscovered?.Invoke();
            yield return new WaitForSeconds(1f);
        }

        public override IEnumerator Update(Action<string> resolve)
        {
            bool needToTurn = CheckRaycastNeedToTurn();
            if (needToTurn)
            {
                Turn();
            }
            Move();
            bool nextState = CheckPlayerDiscovered();
            if (nextState)
            {
                // Go to PlayerDiscover state
                resolve("PlayerDiscover");
            }
            else
            {
                yield return null;
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