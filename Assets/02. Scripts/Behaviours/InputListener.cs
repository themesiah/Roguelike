using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    public class InputListener : MonoBehaviour
    {

        [System.Serializable]
        public class CommandEventResponse : TemplatedStandaloneGameEventListener<InputAction.CallbackContext>
        {
        }

        [SerializeField]
        public List<CommandEventResponse> inputEventList = default;

        private void OnEnable()
        {
            foreach (CommandEventResponse cer in inputEventList)
            {
                cer.Register();
            }
        }

        private void OnDisable()
        {
            foreach(CommandEventResponse cer in inputEventList)
            {
                cer.Unregister();
            }
        }
    }
}