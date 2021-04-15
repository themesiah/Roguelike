using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.StateMachine;
using GamedevsToolbox.UISolution;

namespace GamedevsToolbox.UISolution
{
    public class MenuSolution : MonoBehaviour
    {
        private MenuStateMachine stateMachine = default;

        [SerializeField]
        private UIMenuData[] menuData = default;
        
        void Start()
        {
            stateMachine = new MenuStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            foreach(UIMenuData data in menuData)
            {
                states.Add(data.menuName, new UIState(data));
            }
            stateMachine.SetStates(states);
        }

        public void ReceiveSignal(string signal)
        {
            stateMachine.ReceiveSignal(signal);
        }

        IEnumerator StateMachineCoroutine()
        {
            while (true)
            {
                if (stateMachine != null)
                {
                    yield return stateMachine.Update(null);
                } else
                {
                    yield return null;
                }
            }
        }

        private void OnEnable()
        {
            StartCoroutine(StateMachineCoroutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}