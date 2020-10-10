using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class LocalizedSimpleTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text text = default;

        [SerializeField]
        private string textId = default;

        private void Start()
        {
            SetText();
        }

        private void SetText()
        {
            text.text = Texts.GetText(textId);
        }
    }
}