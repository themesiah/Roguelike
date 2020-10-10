using GamedevsToolbox.ScriptableArchitecture.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;

namespace Laresistance.Behaviours
{
    public class LocalizedIntTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text text = default;

        [SerializeField]
        private string textId = default;

        [SerializeField]
        private int value = default;

        private void Start()
        {
            SetText();
        }

        public void ChangeVariable(int variable)
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