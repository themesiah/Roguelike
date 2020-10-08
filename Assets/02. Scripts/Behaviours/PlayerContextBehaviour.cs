using UnityEngine;
using GamedevsToolbox.StateMachine;
using Laresistance.StateMachines;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class PlayerContextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput = default;
        [SerializeField]
        private LayerMask centerCheckLayerMask = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;

        private SimpleSignalStateMachine stateMachine;
        private GameContextBattleState battleState;

        private void Start()
        {
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("Map", new GameContextMapState(gameObject, Camera.main, playerInput));
            battleState = new GameContextBattleState(gameObject, Camera.main, playerInput, bloodReference, hardCurrencyReference, centerCheckLayerMask.value);
            states.Add("Battle", battleState);

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
                battleState.SetEnemyObjects(new GameObject[] { collision.gameObject });
                stateMachine.ReceiveSignal("Battle");
            }
        }
    }
}