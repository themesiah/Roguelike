using System.Collections;
using System.Collections.Generic;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Laresistance.Core;

namespace Laresistance.Behaviours
{
    public class MinionObtainedBehaviour : RewardPanelBehaviour
    {
        [SerializeField]
        private LocalizedStringTextBehaviour minionRewardText1 = default;
        [SerializeField]
        private Text minionRewardText2 = default;

        private bool inputDone = false;

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

        public override RewardUIType RewardType => RewardUIType.Minion;
    }
}