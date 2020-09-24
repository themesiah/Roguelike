using UnityEngine;
using System.Collections.Generic;

namespace GamedevsToolbox.ScriptableArchitecture.Localization
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Localization/Language")]
    public class ScriptableLanguage : ScriptableObject
    {
        [SerializeField]
        private ScriptableLanguageData[] availableLanguages = default;

        [SerializeField]
        private ScriptableLanguageData currentLanguage = default;

        [SerializeField]
        [Tooltip("If a text is not present in the current language, the fallback language will be used instead." +
            "You also will be forced to have at least all texts in the fallback language and can not be changed in runtime.")]
        private SystemLanguage fallbackLanguage = default;

        [SerializeField]
        private List<ScriptableLocalizedText> localizedTexts = default;

        public SystemLanguage GetLanguage()
        {
            return currentLanguage.GetLanguage();
        }

        public void SetLanguage(ScriptableLanguageData language)
        {
            this.currentLanguage = language;
        }

        public void SetLanguage(SystemLanguage language)
        {
            foreach(ScriptableLanguageData lan in availableLanguages)
            {
                if (lan.GetLanguage() == language)
                {
                    currentLanguage = lan;
                    break;
                }
            }
        }

        public SystemLanguage[] GetAvailableLanguages()
        {
            SystemLanguage[] languages = new SystemLanguage[availableLanguages.Length];
            for (int i = 0; i < availableLanguages.Length; ++i)
            {
                languages[i] = availableLanguages[i].GetLanguage();
            }
            return languages;
        }

        public SystemLanguage GetFallbackLanguage()
        {
            return fallbackLanguage;
        }

        public string GetMoneySymbol()
        {
            return currentLanguage.GetMoneySymbol();
        }

        public string GetDateFormat()
        {
            return currentLanguage.GetDateFormat();
        }

        public void AddText(ScriptableLocalizedText localizedText)
        {
            if (localizedTexts == null)
            {
                localizedTexts = new List<ScriptableLocalizedText>();
            }

            if (!localizedTexts.Contains(localizedText))
            {
                localizedTexts.Add(localizedText);
            }
        }

        public string GetText(string textId)
        {
            ScriptableLocalizedText locText = localizedTexts.Find(t => t.GetTextId() == textId);
            if (locText == null)
            {
                return textId;
            } else
            {
                return GetText(locText);
            }
        }

        public string GetText(ScriptableLocalizedText textRef)
        {
            if (textRef != null)
            {
                return textRef.GetText();
            } else
            {
                return "Missing text ref!";
            }
        }
    }
}