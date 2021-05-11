using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours.Platforms
{
    public class ScenarioInteraction : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent interactionEvent = default;
        [SerializeField]
        private UnityEvent OnInteractionZoneEnter = default;
        [SerializeField]
        private UnityEvent OnInteractionZoneExit = default;
        [SerializeField]
        private bool isEquip = false;

        public void Interact()
        {
            interactionEvent?.Invoke();
        }

        public void EnterInteractionZone()
        {
            OnInteractionZoneEnter?.Invoke();
        }

        public void ExitInteractionZone()
        {
            OnInteractionZoneExit?.Invoke();
        }

        public bool IsEquip => isEquip;
    }
}