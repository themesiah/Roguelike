using GamedevsToolbox.ScriptableArchitecture.Localization;
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
        private ScriptableTextRef textRef = default;

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
            text.text = textRef.GetText(value);
        }
    }
}