using UnityEngine;
using GamedevsToolbox.StateMachine;
using Laresistance.StateMachines;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    public class PlayerContextBehaviour : MonoBehaviour
    {
        //[SerializeField]
        //private PlayerMapBehaviour mapBehaviour = default;
        //[SerializeField]
        //private PlayerBattleBehaviour battleBehaviour = default;
        [SerializeField]
        private PlayerInput playerInput = default;

        private SimpleSignalStateMachine stateMachine;
        private GameContextBattleState battleState;

        private void Awake()
        {
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("Map", new GameContextMapState(gameObject, Camera.main, playerInput));
            battleState = new GameContextBattleState(gameObject, Camera.main, playerInput);
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