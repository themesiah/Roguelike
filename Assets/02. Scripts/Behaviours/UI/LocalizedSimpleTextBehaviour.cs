using GamedevsToolbox.ScriptableArchitecture.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class LocalizedSimpleTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text text = default;

        [SerializeField]
        private ScriptableTextRef textRef = default;

        private void Start()
        {
            SetText();
        }

        private void SetText()
        {
            text.text = textRef.GetText();
        }
    }
}