using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GamedevsToolbox.Utils;
using Laresistance.Data;

namespace Laresistance.Systems
{
    public static class SaveSystem
    {
        private static string SAVED_PREFERENCES_FILE = "preferences.sav";
        private static string SAVED_GAME_FILE = "game.sav";
        private static string SAVED_RUN_FILE = "run.sav";
        private static string[] SAVE_FILES = { SAVED_PREFERENCES_FILE, SAVED_GAME_FILE, SAVED_RUN_FILE };

        // La partida debería guardarse al salir del menú del Peregrino, al terminar una batalla o al morir.
        /*public static void SavePreferences(SavedPreferences data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(GetFullPath(0), FileMode.Create);
            formatter.Serialize(file, data);
            file.Close();
        }

        public static SavedPreferences LoadPreferences()
        {
            if (SaveGameExists(0))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = new FileStream(GetFullPath(0), FileMode.Open);
                SavedPreferences data = formatter.Deserialize(file) as SavedPreferences;
                file.Close();
                return data;
            } else
            {
                return null;
            }
        }*/

        public static void Save<T>(T data, int saveType) where T : class
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(GetFullPath(saveType), FileMode.Create);
            formatter.Serialize(file, data);
            file.Close();
        }

        public static T Load<T>(int saveType) where T : class
        {
            if (SaveGameExists(saveType))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = new FileStream(GetFullPath(saveType), FileMode.Open);
                T data = formatter.Deserialize(file) as T;
                file.Close();
                return data;
            }
            else
            {
                return null;
            }
        }

        private static string GetFullPath(int fileType)
        {
            return Utils.Path(SAVE_FILES[fileType]);
        }

        private static bool SaveGameExists(int fileType)
        {
            return Utils.Exists(SAVE_FILES[fileType]);
        }
    }
}