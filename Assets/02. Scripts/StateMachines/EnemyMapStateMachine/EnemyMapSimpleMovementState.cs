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

        public EnemyMapSimpleMovementState(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, Transform visibilityPivot,
            GameObject playerObject, IPlayerCollidable playerCollidable, UnityEvent onPlayerDiscovered, UnityEvent onPlayerLost)
            : base (characterController, enemyMapData, raycastLayerMask, raycastPivot, visibilityPivot, playerObject, playerCollidable)
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
            if (!Paused)
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
                    playerCollidable.SetStatus("PlayerDiscover");
                    resolve("PlayerDiscover");
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void Move()
        {
            float modifier = characterController.FacingRight ? 1f : -1f;
            float movement = modifier * enemyMapData.Speed;
            characterController.Move(movement);
        }
    }
}