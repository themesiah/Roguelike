using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.StateMachine;
using GamedevsToolbox.UISolution;

namespace GamedevsToolbox.UISolution
{
    public class AsyncMenuSolution : MonoBehaviour
    {
        private AsyncMenuStateMachine stateMachine = default;

        [SerializeField]
        private UIMenuData[] menuData = default;
        
        void Start()
        {
            stateMachine = new AsyncMenuStateMachine();
            Dictionary<string, IAsyncState> states = new Dictionary<string, IAsyncState>();
            foreach(UIMenuData data in menuData)
            {
                states.Add(data.menuName, new AsyncUIState(data));
            }
            stateMachine.SetStates(states);
        }

        public void ReceiveSignal(string signal)
        {
            stateMachine.ReceiveSignal(signal);
        }

        async private void Update()
        {
            await stateMachine.Update();
        }
    }
}