using UnityEngine;
using System.Collections.Generic;

namespace GamedevsToolbox.ScriptableArchitecture.LocalizationV2
{
    public class Texts : MonoBehaviour
    {
        [System.Serializable]
        public class RootNode
        {
            public List<Language> languages;
        }

        [System.Serializable]
        public class Language
        {
            public string id;
            public List<LocalizedText> texts;
        }

        [System.Serializable]
        public class LocalizedText
        {
            public string id;
            public string text;
        }

        [SerializeField]
        private TextAsset textAsset = default;

        private RootNode rootNode;


        private static Dictionary<string, Dictionary<string, string>> localizer;
        private static string currentLanguage;

        private void Awake()
        {
            string rawJson = textAsset.text;
            rootNode = JsonUtility.FromJson(rawJson, typeof(RootNode)) as RootNode;
            localizer = new Dictionary<string, Dictionary<string, string>>();

            foreach(var language in rootNode.languages)
            {
                if (string.IsNullOrEmpty(currentLanguage))
                    currentLanguage = language.id;
                localizer.Add(language.id, new Dictionary<string, string>());
                foreach(var text in language.texts)
                {
                    localizer[language.id].Add(text.id, text.text);
                }
            }
            rootNode = null;
        }

        public static string GetText(string id)
        {
            return GetText(id, new object[] { });
        }

        public static string GetText(string id, object param)
        {
            return GetText(id, new object[] { param });
        }

        public static string GetText(string id, object[] param)
        {
            if (localizer[currentLanguage].ContainsKey(id))
            {
                return string.Format(localizer[currentLanguage][id], param);
            } else
            {
                return id;
            }
        }
    }
}