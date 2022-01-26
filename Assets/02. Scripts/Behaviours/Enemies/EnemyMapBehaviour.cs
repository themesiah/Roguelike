using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Laresistance.Movement;
using Laresistance.Battle;
using Laresistance.Data;
using Laresistance.StateMachines;
using System.Collections.Generic;
using GamedevsToolbox.StateMachine;

namespace Laresistance.Behaviours
{
    public class EnemyMapBehaviour : MapBehaviour
    {
        [SerializeField]
        private EnemyMapData enemyMapData = default;
        [SerializeField]
        private LayerMask raycastLayerMask = default;
        [SerializeField]
        private Transform raycastPivot = default;
        [SerializeField]
        private Transform visibilityPivot = default;
        [SerializeField]
        private bool partyMember = false;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private UnityEvent OnPlayerDiscovered = default;
        [SerializeField]
        private UnityEvent OnPlayerLost = default;

        private IMovementManager movementManager;
        private SimpleSignalStateMachine stateMachine;

        protected void Awake()
        {
            IPlayerCollidable playerCollidable = GetComponent<IPlayerCollidable>();
            GameObject playerObject = playerDataRef.Get().gameObject;
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();

            states.Add("Move", new EnemyMapSimpleMovementState(characterController, enemyMapData, raycastLayerMask.value, raycastPivot, visibilityPivot, playerObject, playerCollidable, OnPlayerDiscovered, OnPlayerLost));
            if (enemyMapData.DiscoverBehaviour == EnemyMapData.PlayerDiscoveredBehaviour.Chase)
            {
                states.Add("PlayerDiscover", new EnemyMapChaseState(characterController, enemyMapData, raycastLayerMask.value, raycastPivot, visibilityPivot, playerObject, playerCollidable));
            } else if (enemyMapData.DiscoverBehaviour == EnemyMapData.PlayerDiscoveredBehaviour.DistanceAttack)
            {
                states.Add("PlayerDiscover", new EnemyMapDistanceAttack(characterController, enemyMapData, raycastLayerMask.value, raycastPivot, visibilityPivot, playerObject, playerCollidable));
            }

            stateMachine.SetStates(states);
            StartCoroutine(StateMachineCoroutine());
            movementManager = new EnemySimpleMovementManager(characterController, enemyMapData, raycastLayerMask.value, raycastPivot);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            if (!partyMember)
            {
                try
                {
                    bool enoughSpace = BattlePosition.CheckSpace(transform.position, raycastLayerMask.value);
                    if (!enoughSpace)
                    {
                        GamedevsToolbox.Utils.Logger.Logger.LogError(string.Format("Enemy {0} have no space to battle", name), gameObject);
                    }
                } catch(System.Exception e)
                {
                    GamedevsToolbox.Utils.Logger.Logger.LogError(e.ToString());
                    GamedevsToolbox.Utils.Logger.Logger.LogError(string.Format("Enemy {0} have no space to battle in room {1}", name, transform.parent.parent.parent.name));
                }
            }
        }

        private void FixedUpdate()
        {
            if (!partyMember)
            {
                //movementManager.Tick(Time.deltaTime);
            }
        }

        public override void PauseMapBehaviour()
        {
            //base.PauseMapBehaviour();
            movementManager?.Pause();
            stateMachine.Pause();
        }

        public override void ResumeMapBehaviour()
        {
            //base.ResumeMapBehaviour();
            movementManager?.Resume();
            stateMachine.Resume();
        }

        public void ReceiveSignal(string signal)
        {

            stateMachine.ReceiveSignal(signal);
        }

        IEnumerator StateMachineCoroutine()
        {
            while (true)
            {
                yield return stateMachine.Update(null);
            }
        }
    }
}