using Laresistance.Behaviours;
using Laresistance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.StateMachines
{
    public class EnemyMapDummyChaseState : EnemyMapState
    {
        public EnemyMapDummyChaseState(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, Transform visibilityPivot, GameObject playerObject, IPlayerCollidable playerCollidable)
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
            resolve("Move");
            yield return null;
        }
    }
}