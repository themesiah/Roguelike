using Laresistance.Systems.Dialog;

namespace Laresistance.Systems
{
    [System.Serializable]
    public class SavedGame
    {
        // Basic data
        public int hardCurrency = 0;
        // Movement abilities unlocked
        public bool doubleJumpUnlocked = false;

        // Dialogs
        public DialogVariablesStatus.VariableData[] dialogData = new DialogVariablesStatus.VariableData[] { };
    }
}