using UnityEngine;
using UnityEditor;
using Laresistance.Systems;

namespace Laresistance.EditorTools
{
    public class SaveGameEditorTest : Editor
    {
        [MenuItem("Laresistance/SaveTest/SaveGameTest1")]
        public static void SaveGameTest1()
        {
            SavedGame data = new SavedGame();
            data.hardCurrency = 5;
            SaveSystem.Save(data);
            Debug.Log("Saved game with hard currency = 5");
        }

        [MenuItem("Laresistance/SaveTest/LoadGameTest1")]
        public static void LoadGameTest1()
        {
            try
            {
                SavedGame data = SaveSystem.Load();
                if (data != null)
                {
                    Debug.LogFormat("Loaded game with hard currency = {0}", data.hardCurrency);
                } else
                {
                    Debug.LogWarning("Save game didn't exist");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        [MenuItem("Laresistance/SaveTest/SaveGameTest2")]
        public static void SaveGameTest2()
        {
            SavedGame data = new SavedGame();
            data.dialogData = new Systems.Dialog.DialogVariablesStatus.VariableData[] { new Systems.Dialog.DialogVariablesStatus.VariableData() { name = "Pilgrim", value = 5 } };
            SaveSystem.Save(data);
            Debug.Log("Saved game with Pilgrim = 5");
        }

        [MenuItem("Laresistance/SaveTest/LoadGameTest2")]
        public static void LoadGameTest2()
        {
            try
            {
                SavedGame data = SaveSystem.Load();
                if (data != null)
                {
                    Debug.LogFormat("Loaded game with {0} = {1}", data.dialogData[0].name, data.dialogData[0].value);
                } else
                {
                    Debug.LogWarning("Save game didn't exist");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        [MenuItem("Laresistance/SaveTest/LoadGameTestReal")]
        public static void LoadGameTestReal()
        {
            try
            {
                SavedGame data = SaveSystem.Load();
                if (data != null)
                {
                    Debug.Log("Loaded game");
                    Debug.LogFormat("Hard currency: {0}", data.hardCurrency);
                    Debug.LogFormat("Double jump unlocked: {0}", data.doubleJumpUnlocked);
                    Debug.LogFormat("Rumble: {0}", data.rumbleActive);
                    Debug.LogFormat("Volumes: GameMaster {0}, Master {1}, Music {2}, Effects {3}", data.gameMasterBusVolume, data.masterBusVolume, data.musicBusVolume, data.effectsBusVolume);
                    Debug.Log("Dialog entries:");
                    foreach(var dialog in data.dialogData)
                    {
                        Debug.LogFormat("{0}: {1}", dialog.name, dialog.value);
                    }
                } else
                {
                    Debug.LogWarning("Save game didn't exist");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}