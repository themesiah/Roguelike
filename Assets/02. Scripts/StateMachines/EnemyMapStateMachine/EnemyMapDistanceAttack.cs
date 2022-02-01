using Laresistance.Behaviours;
using Laresistance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.StateMachines
{
    public class EnemyMapDistanceAttack : EnemyMapState
    {
        private static float MAX_TIME_NO_SEE_PLAYER = 1.5f;
        private float timeNoSeePlayer = 0f;

        private float delayTimer = 0f;
        private IRangedAttackSpawner rangedAttackSpawner;

        public EnemyMapDistanceAttack(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, Transform visibilityPivot, GameObject playerObject, IPlayerCollidable playerCollidable)
            : base(characterController, enemyMapData, raycastLayerMask, raycastPivot, visibilityPivot, playerObject, playerCollidable)
        {
            rangedAttackSpawner = characterController.gameObject.GetComponent<IRangedAttackSpawner>();
        }

        public override IEnumerator EnterState()
        {
            delayTimer = enemyMapData.AttackDelay;
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
                delayTimer += Time.deltaTime;
                if (delayTimer >= enemyMapData.AttackDelay)
                {
                    Turn(characterController.transform.position.x - playerObject.transform.position.x < 0f);
                    yield return rangedAttackSpawner.SpawnRangedAttack(playerObject.transform);
                    delayTimer = 0f;
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
            }
            yield return null;
        }
    }
}