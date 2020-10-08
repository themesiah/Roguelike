using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Localization
{
    public class ScriptableLanguageLocator : MonoBehaviour
    {
        [SerializeField]
        private ScriptableLanguage languageRef = default;

        public static ScriptableLanguage languageInstance;

        private void Awake()
        {
            languageInstance = languageRef;
        }

        public ScriptableLanguage GetLanguageRef()
        {
            return languageRef;
        }
    }
}