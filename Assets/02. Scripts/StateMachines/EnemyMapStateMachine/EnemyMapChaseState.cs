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

        public EnemyMapChaseState(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, GameObject playerObject)
            : base(characterController, enemyMapData, raycastLayerMask, raycastPivot, playerObject)
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
            Move();
            if (CheckPlayerDiscovered())
            {
                timeNoSeePlayer = 0f;
            } else
            {
                timeNoSeePlayer += Time.deltaTime;
                if (timeNoSeePlayer > MAX_TIME_NO_SEE_PLAYER)
                {
                    // Return to move state
                    resolve("Move");
                }
            }
            yield return null;
        }

        private void Move()
        {
            float modifier = characterController.FacingRight ? 1f : -1f;
            float movement = modifier * enemyMapData.ChaseSpeed;
            characterController.Move(movement);
        }
    }
}