using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Localization
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Localization/Language data")]
    public class ScriptableLanguageData : ScriptableObject
    {
        [SerializeField]
        private SystemLanguage language = default;

        [SerializeField]
        [Tooltip("The money symbol with {0}, so when formatting the string the number appears after or before the symbol.")]
        private string moneySymbol = "{0}€";

        [SerializeField]
        private string dateFormat = "yyyy/mm/dd";

        public SystemLanguage GetLanguage()
        {
            return language;
        }

        public string GetMoneySymbol()
        {
            return moneySymbol;
        }

        public string GetDateFormat()
        {
            return dateFormat;
        }
    }
}