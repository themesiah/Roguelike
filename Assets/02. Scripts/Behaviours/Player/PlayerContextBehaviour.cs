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
using Laresistance.Systems.Dialog;

namespace Laresistance.Behaviours
{
    public class PlayerContextBehaviour : MonoBehaviour, IPausable
    {
        [Header("Events")]
        [SerializeField]
        private GameEvent saveGameEvent = default;
        [SerializeField]
        private Collider2DGameEvent boundsChangeEvent = default;
        [SerializeField]
        private StringGameEvent virtualCameraChangeEvent = default;
        [SerializeField]
        private StringGameEvent actionMapSwitchEvent = default;
        [SerializeField]
        private UnityEvent OnTimeStopActivated = default;
        [SerializeField]
        private UnityEvent OnTimeStopDeactivated = default;
        [SerializeField]
        private UnityEvent<float> OnTimeStopDeltaModified = default;
        [SerializeField]
        private GameEvent finishedChangingRoomEvent = default;
        [Header("References")]
        [SerializeField]
        private RewardEvent rewardEvent = default;
        [SerializeField]
        private RuntimeMapBehaviourSet mapBehavioursRef = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private RuntimeSingleCamera cameraReference = default;
        [SerializeField]
        private PlayerMovementData playerMovementData = default;
        [SerializeField]
        private ScriptableIntReference battlePositionIntegerReference = default;
        [SerializeField]
        private ScriptableFloatReference gainValueRef = default;
        [Tooltip("Group of entities followed by the combat camera")]
        [SerializeField]
        private RuntimeSingleCinemachineTargetGroup targetGroupRef = default;
        [SerializeField]
        private ScriptableIntReference difficultyRef = default;
        [SerializeField]
        private ScriptableIntReference playerSelectionIndexRef = default;
        [Header("Configuration")]
        [SerializeField]
        private LayerMask centerCheckLayerMask = default;
        [Header("Dialog")]
        [SerializeField]
        private CharacterDialogEvent dialogEvent = default;
        [SerializeField]
        private CharacterDialog systemDialog = default;
        [SerializeField]
        private DialogVariablesStatus dialogVariablesStatus = default;
        [SerializeField]
        private GameEvent loadBossRoomEvent = default;

        private SimpleSignalStateMachine stateMachine;
        private GameContextBattleState battleState;
        private GameContextRoomChangeState roomChangeState;

        private void Start()
        {
            Camera camera = cameraReference.Get();
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("Map", new GameContextMapState(gameObject, camera, actionMapSwitchEvent, bloodReference, mapBehavioursRef, virtualCameraChangeEvent));
            battleState = new GameContextBattleState(gameObject, actionMapSwitchEvent, rewardEvent, centerCheckLayerMask.value,
                battlePositionIntegerReference, virtualCameraChangeEvent, targetGroupRef, saveGameEvent,
                difficultyRef, playerSelectionIndexRef.GetValue());
            states.Add("Battle", battleState);
            roomChangeState = new GameContextRoomChangeState(gameObject, camera, playerMovementData, boundsChangeEvent, finishedChangingRoomEvent, gainValueRef);
            states.Add("RoomChange", roomChangeState);
            states.Add("UI", new GameContextUIState(actionMapSwitchEvent));

            stateMachine.SetStates(states);
            StartCoroutine(StateMachineCoroutine());

            battleState.battleSystem.OnTimeStopActivation += OnTimeStopActivate;
            battleState.battleSystem.OnTimeStopDeltaModifier += OnTimeStopDeltaModifier;
        }

        public void ReceiveSignal(string signal)
        {

            Debug.LogFormat("Received signal {0} for state machine", signal);
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
                BattleContextStart(collision.gameObject, true);
            }
        }

        public void OnPlayerWeaponTrigger(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                IPlayerCollidable playerCollidable = collider.gameObject.GetComponent<IPlayerCollidable>();
                if (playerCollidable.PlayerAttacked(transform))
                {
                    BattleContextStart(collider.gameObject, false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("ChangeRoom"))
            {
                RoomChangeBehaviour rcb = collider.gameObject.GetComponent<RoomChangeBehaviour>();
                Assert.IsNotNull(rcb);
                if (rcb.ChangeRoomOnEnterTrigger)
                {
                    RoomChange(rcb);
                }
            }
        }

        private void BattleContextStart(GameObject enemyObject, bool enemyAdvantage)
        {
            PartyManagerBehaviour pmb = enemyObject.GetComponent<PartyManagerBehaviour>();
            GameObject[] enemies;
            if (pmb == null)
            {
                enemies = new GameObject[] { enemyObject };
            }
            else
            {
                enemies = pmb.GetFullParty();
            }

            battleState.SetEnemyObjects(enemies, enemyAdvantage);
            stateMachine.ReceiveSignal("Battle");
        }

        public void RoomChange(RoomChangeBehaviour rcb)
        {
            if (rcb.IsLevelEnd)
            {
                StartCoroutine(BossRoomCoroutine());
            }
            else
            {
                RoomChangeData rcd = rcb.GetRoomChangeData();
                roomChangeState.SetRoomData(rcd);
                stateMachine.ReceiveSignal("RoomChange");
            }
        }

        private IEnumerator BossRoomCoroutine()
        {
            yield return dialogEvent?.Raise(systemDialog);
            if (dialogVariablesStatus.GetVariable("RoomChange") == 2)
            {
                loadBossRoomEvent?.Raise();
            }
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

        private void OnTimeStopDeltaModifier(BattleSystem sender, float modifier)
        {
            OnTimeStopDeltaModified?.Invoke(modifier);
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