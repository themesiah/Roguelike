using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class PlayerMapAbilitiesBehaviour : MonoBehaviour
    {
        [Header("Abilities references")]
        [SerializeField]
        private ScriptableBoolReference doubleJumpReference = default;

        [Header("Other")]
        [SerializeField]
        private ScriptableIntReference jumpAmountReference = default;

        private void OnEnable()
        {
            doubleJumpReference.RegisterOnChangeAction(OnDoubleJumpObtained);
        }

        private void OnDisable()
        {
            doubleJumpReference.UnregisterOnChangeAction(OnDoubleJumpObtained);
        }

        private void Start()
        {
            OnDoubleJumpObtained(doubleJumpReference.GetValue());
        }

        private void OnDoubleJumpObtained(bool obtained)
        {
            if (obtained)
            {
                jumpAmountReference.SetValue(2);
            } else
            {
                jumpAmountReference.SetValue(1);
            }
        }
    }
}