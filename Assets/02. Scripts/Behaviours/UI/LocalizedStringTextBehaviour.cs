using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class LocalizedStringTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text text = default;

        [SerializeField]
        private string textId = default;

        [SerializeField]
        private string value = default;

        private void Start()
        {
            SetText();
        }

        public void ChangeVariable(string variable)
        {
            value = variable;
            SetText();
        }

        private void SetText()
        {
            text.text = Texts.GetText(textId, value);
        }
    }
}