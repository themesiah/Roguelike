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
        protected Transform visibilityPivot;
        protected Character2DController characterController;
        protected GameObject playerObject;
        protected IPlayerCollidable playerCollidable;

        private int pauseStack = 0;
        protected bool Paused => pauseStack > 0;

        public EnemyMapState(Character2DController characterController, EnemyMapData enemyMapData, int raycastLayerMask, Transform raycastPivot, Transform visibilityPivot, GameObject playerObject, IPlayerCollidable playerCollidable)
        {
            this.characterController = characterController;
            this.enemyMapData = enemyMapData;
            this.raycastLayerMask = raycastLayerMask;
            this.raycastPivot = raycastPivot;
            this.playerObject = playerObject;
            this.visibilityPivot = visibilityPivot;
            this.playerCollidable = playerCollidable;
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
            Vector3 distance = (playerObject.transform.position + Vector3.up * 1.5f) - visibilityPivot.position;
            float angle = Vector3.Angle(distance, characterController.FacingRight ? visibilityPivot.right : -visibilityPivot.right);
            if (angle <= enemyMapData.ViewAngle)
            {
                RaycastHit2D hit = Physics2D.Raycast(visibilityPivot.position, distance.normalized, enemyMapData.ViewDistance);
                Debug.DrawLine(visibilityPivot.position, visibilityPivot.position + (distance.normalized * enemyMapData.ViewDistance), Color.red);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool CheckRaycastNeedToTurn()
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


        protected void Turn()
        {
            characterController.Flip();
        }

        protected void Turn(bool r)
        {
            characterController.Flip(r);
        }
    }
}