using Laresistance.Behaviours.Platforms;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Laresistance.Interaction
{
    public class PlayerScenarioInteraction
    {
        public void Interact(InputAction.CallbackContext context, ScenarioInteraction interaction, bool isEquip)
        {
            if (context.performed && interaction != null && interaction.IsEquip == isEquip)
            {
                interaction.Interact();
            }
        }
    }
}