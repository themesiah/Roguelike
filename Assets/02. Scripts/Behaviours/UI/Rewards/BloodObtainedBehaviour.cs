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
        public static BloodObtainedBehaviour instance = null;

        [SerializeField]
        private LocalizedIntTextBehaviour textBehaviour = default;

        private bool inputDone = false;

        private void Awake()
        {
            instance = this;
        }

        public void AnyInput(InputAction.CallbackContext context)
        {
            inputDone = true;
        }

        protected override IEnumerator StartingTween(RewardData rewardData, Player player)
        {
            textBehaviour.ChangeVariable(rewardData.bloodAmount);
            yield return base.StartingTween(rewardData, player);
        }

        protected override IEnumerator ExecutePanelProcess(RewardData rewardData, Player player)
        {
            inputDone = false;
            while (!inputDone)
            {
                yield return null;
            }
        }
    }
}