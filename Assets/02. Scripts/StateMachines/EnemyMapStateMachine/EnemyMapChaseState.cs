using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;
using Laresistance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.StateMachines
{
    public class EnemyMapChaseState : EnemyMapState
    {
        private static float MAX_TIME_NO_SEE_PLAYER = 1.5f;
        private float timeNoSeePlayer = 0f;

        public EnemyMapChaseState(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, Transform visibilityPivot, GameObject playerObject, IPlayerCollidable playerCollidable)
            : base(characterController, enemyMapData, raycastLayerMask, raycastPivot, visibilityPivot, playerObject, playerCollidable)
        {
        }

        public override IEnumerator EnterState()
        {
            yield return null;
        }

        public override IEnumerator ExitState()
        {
            yield return null;
        }

        public override IEnumerator Update(Action<string> resolve)
        {
            if (!Paused)
            {
                Move();
                if (CheckRaycastNeedToTurn())
                {
                    resolve("Move");
                }
                else
                if (CheckPlayerDiscovered())
                {
                    timeNoSeePlayer = 0f;
                }
                else
                {
                    timeNoSeePlayer += Time.deltaTime;
                    if (timeNoSeePlayer > MAX_TIME_NO_SEE_PLAYER)
                    {
                        // Return to move state
                        playerCollidable.SetStatus("Move");
                        resolve("Move");
                    }
                }
                yield return null;
            }
        }

        private void Move()
        {
            float modifier = characterController.FacingRight ? 1f : -1f;
            float movement = modifier * enemyMapData.ChaseSpeed;
            characterController.Move(movement);
        }
    }
}