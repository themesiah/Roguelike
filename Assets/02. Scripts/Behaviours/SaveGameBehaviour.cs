using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Systems;
using Laresistance.Systems.Dialog;
using Laresistance.Data;
using Laresistance.Core;
using System.Collections.Generic;

namespace Laresistance.Behaviours
{
    [System.Serializable]
    public class SaveGameBehaviour : MonoBehaviour
    {
        [Header("Preferences")]
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
        // Graphics
        // Analytics preferences
        // Key bindings

        [Header("GameData")]
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        [SerializeField]
        private ScriptableBoolReference doubleJumpReference = default;
        [SerializeField]
        private DialogVariablesStatus dialogsStatus = default;
        // Unlocked things
        // Save game name

        [Header("RunData")]
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        // seed
        // save game name
        // character (int)
        // shards and their levels
        // character level
        // equipments
        // stats
        // current hp
        // current level (biome 1, 2, 3, 4 or boss 1, 2, 3, 4)
        // enemies defeated in the map
        // obtained equipment in the map
        // used altars, sanctuary, nests in the map
        // position in the map

        #region Preferences
        public void SavePreferences()
        {
            SavedPreferences data = new SavedPreferences();
            data.rumbleActive = rumbleActiveReference.GetValue();
            data.gameMasterBusVolume = audioGameMasterBusVolume.GetValue();
            data.masterBusVolume = audioMasterBusVolume.GetValue();
            data.effectsBusVolume = audioEffectsBusVolume.GetValue();
            data.musicBusVolume = audioMusicBusVolume.GetValue();
            SaveSystem.Save(data, 0);
            Debug.Log("Preferences saved");
        }

        public void LoadPreferences()
        {
            SavedPreferences data = SaveSystem.Load<SavedPreferences>(0);
            if (data == null)
                data = new SavedPreferences();

            rumbleActiveReference.SetValue(data.rumbleActive);
            audioGameMasterBusVolume.SetValue(data.gameMasterBusVolume);
            audioMasterBusVolume.SetValue(data.masterBusVolume);
            audioEffectsBusVolume.SetValue(data.effectsBusVolume);
            audioMusicBusVolume.SetValue(data.musicBusVolume);
            Debug.Log("Preferences loaded");
        }
        #endregion

        #region GameData
        public void SaveGame()
        {
            SavedGame data = new SavedGame();
            data.hardCurrency = hardCurrencyReference.GetValue();

            data.doubleJumpUnlocked = doubleJumpReference.GetValue();

            data.dialogData = dialogsStatus.GetAll();
            SaveSystem.Save(data, 1);
            Debug.Log("Game saved");
        }

        public void LoadGame()
        {
            SavedGame data = SaveSystem.Load<SavedGame>(1);
            if (data == null)
                data = new SavedGame();
            hardCurrencyReference.SetValue(data.hardCurrency);

            doubleJumpReference.SetValue(data.doubleJumpUnlocked);

            dialogsStatus.SetAll(data.dialogData);
            Debug.Log("Game loaded");
        }
        #endregion

        #region RunData
        public void SaveRun()
        {
            SavedRun data = new SavedRun();

            Player player = playerDataRef.Get().player;
            data.seed = 0; // TODO
            data.name = "save"; // TODO
            data.characterType = player.playerCharacterType;
            data.characterLevel = player.Level;

            List<SavedRun.MinionSerializedData> minionSerializedData = new List<SavedRun.MinionSerializedData>();
            foreach(var minion in player.GetMinions())
            {
                minionSerializedData.Add(new SavedRun.MinionSerializedData() { minionId = minion.minionId, minionLevel = minion.Level });
            }
            data.minions = minionSerializedData;

            List<SavedRun.MinionSerializedData> minionReserveSerializedData = new List<SavedRun.MinionSerializedData>();
            foreach(var minion in player.GetMinionReserve())
            {
                minionReserveSerializedData.Add(new SavedRun.MinionSerializedData() { minionId = minion.minionId, minionLevel = minion.Level });
            }
            data.minionsReserve = minionReserveSerializedData;

            List<string> equipments = new List<string>();
            foreach(var equip in player.GetEquipments())
            {
                equipments.Add(equip.Data.EquipmentNameReference);
            }
            data.equipments = equipments;

            List<int> stats = new List<int>(player.statusManager.battleStats.GetStats());
            data.stats = stats;

            data.currentHp = player.statusManager.health.GetCurrentHealth();
            data.currentLevel = 0; // TODO


            SaveSystem.Save(data, 2);
            Debug.Log("Run saved");
        }

        public void LoadRun()
        {
            SavedRun data = SaveSystem.Load<SavedRun>(2);
            if (data == null)
                data = new SavedRun();
            // TODO
            Debug.Log("Run loaded");
        }
        #endregion
    }
}