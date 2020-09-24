using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Localization
{
    [System.Serializable]
    public class ScriptableTextRef
    {
        [SerializeField]
        private bool useConstantText = true;

        [SerializeField]
        private bool useTextId = false;

        [SerializeField]
        private string constantText = default;

        [SerializeField]
        private string textId = default;

        [SerializeField]
        private ScriptableLocalizedText textRef = default;

        public string GetText()
        {
            return GetText(new object[] {"" });
        }

        public string GetText(object arg0)
        {
            return GetText(new object[] {arg0});
        }

        public string GetText(object arg0, object arg1)
        {
            return GetText(new object[] { arg0, arg1 });
        }

        public string GetText(object[] args)
        {
            if (useConstantText == true)
            {
                return string.Format(constantText, args);
            } else if (useTextId)
            {
                return string.Format(ScriptableLanguageLocator.languageInstance.GetText(textId), args);
            } else
            {
                return string.Format(ScriptableLanguageLocator.languageInstance.GetText(textRef), args);
            }
        }

        public string GetDateFormat()
        {
            if (useConstantText == true)
            {
                return "yyyy/MM/dd";
            }
            else if (useTextId)
            {
                return ScriptableLanguageLocator.languageInstance.GetDateFormat();
            }
            else
            {
                return ScriptableLanguageLocator.languageInstance.GetDateFormat();
            }
        }
    }
}