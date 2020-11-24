using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours.Platforms
{
    public class ScenarioInteraction : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent interactionEvent = default;
        [SerializeField]
        private GameObject controlsObject = default;
        [SerializeField]
        private bool isEquip = false;

        public void Interact()
        {
            interactionEvent?.Invoke();
        }

        public void EnterInteractionZone()
        {
            controlsObject?.SetActive(true);
        }

        public void ExitInteractionZone()
        {
            controlsObject?.SetActive(false);
        }

        public bool IsEquip => isEquip;
    }
}