﻿using System.Collections;
using System.Collections.Generic;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    public class MinionObtainedBehaviour : RewardPanelBehaviour
    {
        public static MinionObtainedBehaviour instance = null;

        [SerializeField]
        private LocalizedStringTextBehaviour minionRewardText1 = default;
        [SerializeField]
        private Text minionRewardText2 = default;

        private bool inputDone = false;

        private void Awake()
        {
            instance = this;
        }

        public void AnyInput(InputAction.CallbackContext context)
        {
            inputDone = true;
        }

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            minionRewardText1.ChangeVariable(rewardData.minion.Name);
            minionRewardText2.text = rewardData.minion.GetAbilityText();
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
    }
}