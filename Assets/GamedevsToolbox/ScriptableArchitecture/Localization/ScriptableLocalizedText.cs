using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Localization
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Scriptable Architecture/Localization/Text")]
    public class ScriptableLocalizedText : ScriptableObject
    {
        [System.Serializable]
        public class LocalizedTextPair
        {
            public SystemLanguage language;
            [TextArea]
            public string text;
        }
        [SerializeField]
        private string localizedTextId = default;
        [SerializeField]
        private ScriptableLanguage languageReference = default;
        [SerializeField]
        private List<LocalizedTextPair> textsPerLanguage = new List<LocalizedTextPair>();

        private Dictionary<SystemLanguage, string> textsPerLanguageDictionary = new Dictionary<SystemLanguage, string>();

        public string GetText()
        {
            string finalString = string.Empty;
            if (textsPerLanguageDictionary.ContainsKey(languageReference.GetLanguage()))
            {
                finalString = textsPerLanguageDictionary[languageReference.GetLanguage()];
            } else if (textsPerLanguageDictionary.ContainsKey(languageReference.GetFallbackLanguage()))
            {
                finalString = textsPerLanguageDictionary[languageReference.GetFallbackLanguage()];
            } else
            {
                finalString = name;
            }

            finalString = finalString.Replace("{100}", languageReference.GetMoneySymbol());

            return finalString;
        }

        public string GetDateFormat()
        {
            return languageReference.GetDateFormat();
        }

        public string GetTextId()
        {
            return localizedTextId;
        }

        private void Awake()
        {
#if UNITY_EDITOR
            OnValidate();
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (languageReference == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableLanguage).Name);
                ScriptableLanguage[] a = new ScriptableLanguage[guids.Length];
                for (int i = 0; i < guids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    a[i] = AssetDatabase.LoadAssetAtPath<ScriptableLanguage>(path);
                }

                if (a[0] == null)
                {
                    Utils.Logger.Logger.LogWarning("There is no ScriptableLanguage instance created yet. Right click in any folder and create one.");
                }
                else
                {
                    languageReference = a[0];
                }
            }

            if (textsPerLanguageDictionary == null)
            {
                textsPerLanguageDictionary = new Dictionary<SystemLanguage, string>();
            }

            foreach(var text in textsPerLanguage)
            {
                textsPerLanguageDictionary[text.language] = text.text;
            }

            //if (languageReference != null && !string.IsNullOrEmpty(name) && !textsPerLanguageDictionary.ContainsKey(languageReference.GetFallbackLanguage()))
            //{
            //    Utils.Logger.Logger.LogWarning(string.Format("You have to write at least the fallback language for every localized text. Missing: {0}", name));
            //}

            if (languageReference != null)
            {
                languageReference.AddText(this);
            }
        }

        public void AddText(LocalizedTextPair textPair)
        {
            LocalizedTextPair ltp = textsPerLanguage.Find(pair => pair.language == textPair.language);
            if (ltp != null)
            {
                ltp.text = textPair.text;
            } else {
                textsPerLanguage.Add(textPair);
            }
        }

        public void SetId(string id)
        {
            localizedTextId = id;
        }
#endif
    }
}