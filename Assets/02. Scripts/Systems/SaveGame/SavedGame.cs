using Laresistance.Systems.Dialog;

namespace Laresistance.Systems
{
    /*
    VERSIONS
    Version 1: hardCurrency
    */
    [System.Serializable]
    public class SavedGame
    {
        // Basic data
        public int hardCurrency = 0;
        // Movement abilities unlocked
        public bool doubleJumpUnlocked = false;

        // Options
        public bool rumbleActive = true;
        public float gameMasterBusVolume = 1f;
        public float masterBusVolume = 1f;
        public float effectsBusVolume = 1f;
        public float musicBusVolume = 1f;

        // Dialogs
        public DialogVariablesStatus.VariableData[] dialogData = new DialogVariablesStatus.VariableData[] { };
    }
}