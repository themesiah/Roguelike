using Laresistance.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class ConsumableObtainedBehaviour : RewardPanelBehaviour
    {
        [SerializeField]
        private LocalizedStringTextBehaviour consumableRewardText1 = default;
        [SerializeField]
        private Text consumableRewardText2 = default;

        private bool inputDone = false;

        public override RewardUIType RewardType => RewardUIType.Consumable;

        public void AnyInput(InputAction.CallbackContext context)
        {
            inputDone = true;
        }

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            consumableRewardText1.ChangeVariable(rewardData.consumable.Name);
            consumableRewardText2.text = rewardData.consumable.GetAbilityText();
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