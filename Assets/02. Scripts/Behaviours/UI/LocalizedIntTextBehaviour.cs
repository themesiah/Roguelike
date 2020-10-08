using GamedevsToolbox.ScriptableArchitecture.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class LocalizedIntTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text text = default;

        [SerializeField]
        private ScriptableTextRef textRef = default;

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
            text.text = textRef.GetText(value);
        }
    }
}