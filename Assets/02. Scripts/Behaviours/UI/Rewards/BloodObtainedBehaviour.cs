using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Localization;
using DigitalRuby.Tween;
using UnityEngine.InputSystem;
using Laresistance.Data;
using Laresistance.Core;

namespace Laresistance.Behaviours
{
    public class BloodObtainedBehaviour : RewardPanelBehaviour
    {
        [SerializeField]
        private LocalizedIntTextBehaviour textBehaviour = default;

        private bool inputDone = false;

        public void AnyInput(InputAction.CallbackContext context)
        {
            inputDone = true;
        }

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            textBehaviour.ChangeVariable(rewardData.bloodAmount);
            yield return base.StartingTween(rewardData);
        }

        protected override IEnumerator ExecutePanelProcess(RewardData rewardData)
        {
            inputDone = false;
            while (!inputDone)
            {
                yield return null;
            }
        }

        public override RewardUIType RewardType => RewardUIType.Blood;
    }
}