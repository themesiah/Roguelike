using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;
using Laresistance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.StateMachines
{
    public abstract class EnemyMapState : ICoroutineState
    {
        protected EnemyMapData enemyMapData;
        protected int raycastLayerMask;
        protected Transform raycastPivot;
        protected Character2DController characterController;
        protected GameObject playerObject;

        private int pauseStack = 0;
        protected bool Paused => pauseStack > 0;

        public EnemyMapState(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, GameObject playerObject)
        {
            this.characterController = characterController;
            this.enemyMapData = enemyMapData;
            this.raycastLayerMask = raycastLayerMask;
            this.raycastPivot = raycastPivot;
            this.playerObject = playerObject;
        }

        public abstract IEnumerator EnterState();

        public abstract IEnumerator ExitState();

        public void Pause()
        {
            pauseStack++;
            characterController.Pause();
        }

        public virtual void ReceiveSignal(string signal)
        {
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

        public abstract IEnumerator Update(Action<string> resolve);

        protected bool CheckPlayerDiscovered()
        {
            Vector3 distance = (playerObject.transform.position + Vector3.up * 0.5f) - raycastPivot.position;
            float angle = Vector3.Angle(distance, characterController.FacingRight ? raycastPivot.right : -raycastPivot.right);
            if (angle <= enemyMapData.ViewAngle)
            {
                RaycastHit2D hit = Physics2D.Raycast(raycastPivot.position, distance.normalized, enemyMapData.ViewDistance);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}