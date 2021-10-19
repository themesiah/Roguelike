using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Systems;
using Laresistance.Systems.Dialog;
using Laresistance.Audio;

namespace Laresistance.Behaviours
{
    public class SaveGameBehaviour : MonoBehaviour
    {
        // Basic data
        [Header("Basic Data")]
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        // Movement abilities
        [Header("Movement Abilities")]
        [SerializeField]
        private ScriptableBoolReference doubleJumpReference = default;
        // Options
        [Header("Options")]
        [SerializeField]
        private ScriptableBoolReference rumbleActiveReference = default;
        [SerializeField]
        private ScriptableFloatReference audioGameMasterBusVolume = default;
        [SerializeField]
        private ScriptableFloatReference audioMasterBusVolume = default;
        [SerializeField]
        private ScriptableFloatReference audioEffectsBusVolume = default;
        [SerializeField]
        private ScriptableFloatReference audioMusicBusVolume = default;
        // Dialogs
        [Header("Dialogs")]
        [SerializeField]
        private DialogVariablesStatus dialogsStatus = default;

        private void Start()
        {
        }

        public void SaveGame()
        {
            SavedGame data = new SavedGame();
            data.hardCurrency = hardCurrencyReference.GetValue();

            data.doubleJumpUnlocked = doubleJumpReference.GetValue();

            data.rumbleActive = rumbleActiveReference.GetValue();
            data.gameMasterBusVolume = audioGameMasterBusVolume.GetValue();
            data.masterBusVolume = audioMasterBusVolume.GetValue();
            data.effectsBusVolume = audioEffectsBusVolume.GetValue();
            data.musicBusVolume = audioMusicBusVolume.GetValue();

            data.dialogData = dialogsStatus.GetAll();
            SaveSystem.Save(data);
            Debug.Log("Game saved");
        }

        public void LoadGame()
        {
            SavedGame data = SaveSystem.Load();
            if (data == null)
                data = new SavedGame();
            hardCurrencyReference.SetValue(data.hardCurrency);

            doubleJumpReference.SetValue(data.doubleJumpUnlocked);

            rumbleActiveReference.SetValue(data.rumbleActive);
            audioGameMasterBusVolume.SetValue(data.gameMasterBusVolume);
            audioMasterBusVolume.SetValue(data.masterBusVolume);
            audioEffectsBusVolume.SetValue(data.effectsBusVolume);
            audioMusicBusVolume.SetValue(data.musicBusVolume);

            dialogsStatus.SetAll(data.dialogData);
            Debug.Log("Game loaded");
        }

    }
}