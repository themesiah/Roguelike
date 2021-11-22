using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GamedevsToolbox.Utils;

namespace Laresistance.Systems
{
    public static class SaveSystem
    {
        // La partida debería guardarse al salir del menú del Peregrino, al terminar una batalla o al morir.
        public static void Save(SavedGame data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(GetFullPath(), FileMode.Create);
            formatter.Serialize(file, data);
            file.Close();
        }

        public static SavedGame Load()
        {
            if (SaveGameExists())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = new FileStream(GetFullPath(), FileMode.Open);
                SavedGame data = formatter.Deserialize(file) as SavedGame;
                file.Close();
                return data;
            } else
            {
                return null;
            }
        }

        private static string GetFullPath()
        {
            return Utils.Path("saveGame.data");
        }

        private static bool SaveGameExists()
        {
            return Utils.Exists("saveGame.data");
        }
    }
}