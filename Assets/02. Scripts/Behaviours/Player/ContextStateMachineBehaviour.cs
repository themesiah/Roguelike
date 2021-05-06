using UnityEngine;
using GamedevsToolbox.StateMachine;
using Laresistance.StateMachines;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AddressableAssets;

namespace Laresistance.Behaviours
{
    public class ContextStateMachineBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AssetReference targetScene = default;
        [SerializeField]
        private AssetReference menuScene = default;

        private SimpleSignalStateMachine stateMachine = default;

        void Start()
        {
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("MainMenu", new GameManagerMainMenuState(menuScene));
            states.Add("Map", new GameManagerMapState(targetScene));

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
    }
}