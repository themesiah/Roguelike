using Laresistance.Behaviours.Platforms;
using UnityEngine.InputSystem;

namespace Laresistance.Interaction
{
    public class PlayerScenarioInteraction
    {
        public void Interact(InputAction.CallbackContext context, ScenarioInteraction interaction)
        {
            if (context.performed && interaction != null)
            {
                interaction.Interact();
            }
        }
    }
}