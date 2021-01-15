using UnityEngine;
using GamedevsToolbox.StateMachine;
using Laresistance.StateMachines;
using System.Collections.Generic;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine.Assertions;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using GamedevsToolbox.Utils;
using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine.InputSystem;
using Laresistance.Systems;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class PlayerContextBehaviour : MonoBehaviour, IPausable
    {
        [SerializeField]
        private StringGameEvent actionMapSwitchEvent = default;
        [SerializeField]
        private RuntimeMapBehaviourSet mapBehavioursRef = default;
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
        [SerializeField]
        private ScriptableIntReference battlePositionIntegerReference = default;
        [SerializeField]
        private UnityEvent OnTimeStopActivated = default;
        [SerializeField]
        private UnityEvent OnTimeStopDeactivated = default;

        private SimpleSignalStateMachine stateMachine;
        private GameContextBattleState battleState;
        private GameContextRoomChangeState roomChangeState;

        private void Start()
        {
            Camera camera = cameraReference.Get();
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("Map", new GameContextMapState(gameObject, camera, actionMapSwitchEvent, bloodReference, mapBehavioursRef));
            battleState = new GameContextBattleState(gameObject, camera, actionMapSwitchEvent, bloodReference, hardCurrencyReference, centerCheckLayerMask.value, rewardUILibrary, battlePositionIntegerReference);
            states.Add("Battle", battleState);
            roomChangeState = new GameContextRoomChangeState(gameObject, camera, playerMovementData);
            states.Add("RoomChange", roomChangeState);
            states.Add("UI", new GameContextUIState(actionMapSwitchEvent));

            stateMachine.SetStates(states);
            StartCoroutine(StateMachineCoroutine());

            battleState.battleSystem.OnTimeStopActivation += OnTimeStopActivate;
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
                RoomChange(rcb);
            }
        }

        public void RoomChange(RoomChangeBehaviour rcb)
        {
            RoomChangeData rcd = rcb.GetRoomChangeData();
            roomChangeState.SetRoomData(rcd);
            stateMachine.ReceiveSignal("RoomChange");
        }

        public void PerformTimeStop(InputAction.CallbackContext context)
        {
            bool activate = false;
            if (context.performed)
            {
                activate = true;
            } else if (context.canceled)
            {
                activate = false;
            } else
            {
                return;
            }
            battleState.PerformTimeStop(activate);
        }

        private void OnTimeStopActivate(BattleSystem sender, bool activation)
        {
            if (activation)
            {
                OnTimeStopActivated?.Invoke();
            } else
            {
                OnTimeStopDeactivated?.Invoke();
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