using UnityEngine;
using GamedevsToolbox.StateMachine;
using Laresistance.StateMachines;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine.Assertions;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using GamedevsToolbox.Utils;

namespace Laresistance.Behaviours
{
    public class PlayerContextBehaviour : MonoBehaviour, IPausable
    {
        [SerializeField]
        private PlayerInput playerInput = default;
        [SerializeField]
        private LayerMask centerCheckLayerMask = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        [SerializeField]
        private RuntimeSingleCamera cameraReference = default;
        [SerializeField]
        private PlayerMovementData playerMovementData = default;
        [SerializeField]
        private RewardUILibrary rewardUILibrary = default;

        private SimpleSignalStateMachine stateMachine;
        private GameContextBattleState battleState;
        private GameContextRoomChangeState roomChangeState;

        private void Start()
        {
            Camera camera = cameraReference.Get();
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("Map", new GameContextMapState(gameObject, camera, playerInput));
            battleState = new GameContextBattleState(gameObject, camera, playerInput, bloodReference, hardCurrencyReference, centerCheckLayerMask.value, rewardUILibrary);
            states.Add("Battle", battleState);
            roomChangeState = new GameContextRoomChangeState(gameObject, camera, playerMovementData);
            states.Add("RoomChange", roomChangeState);
            states.Add("UI", new GameContextUIState());

            stateMachine.SetStates(states);
            StartCoroutine(StateMachineCoroutine());
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                PartyManagerBehaviour pmb = collision.gameObject.GetComponent<PartyManagerBehaviour>();
                GameObject[] enemies;
                if (pmb == null)
                {
                    enemies = new GameObject[] { collision.gameObject };
                } else
                {
                    enemies = pmb.GetFullParty();
                }

                foreach(GameObject enemy in enemies)
                {
                    if (enemy == enemies[0]) continue;
                    enemy.transform.SetParent(enemies[0].transform.parent);
                }
                battleState.SetEnemyObjects(enemies);
                stateMachine.ReceiveSignal("Battle");
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("ChangeRoom"))
            {
                RoomChangeBehaviour rcb = collider.gameObject.GetComponent<RoomChangeBehaviour>();
                Assert.IsNotNull(rcb);
                RoomChangeData rcd = rcb.GetRoomChangeData();
                roomChangeState.SetRoomData(rcd);
                stateMachine.ReceiveSignal("RoomChange");
            }
        }

        public void Pause()
        {
            stateMachine.Pause();
        }

        public void Resume()
        {
            stateMachine.Resume();
        }
    }
}