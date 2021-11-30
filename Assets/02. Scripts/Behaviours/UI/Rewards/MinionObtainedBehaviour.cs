using System.Collections;
using System.Collections.Generic;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Laresistance.Core;
using UnityEngine.Analytics;
using Laresistance.Systems;

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
            rewardData.minion.StatusManager?.ResetStatus();
            minionRewardText1.ChangeVariable(rewardData.minion.Name);
            minionRewardText2.text = rewardData.minion.GetAbilityText();
            AnalyticsSystem.Instance.CustomEvent("MinionRecruitment", new Dictionary<string, object>() {
                { "Recruited", true },
                { "Overwrited", false },
                { "Level", currentLevelRef.GetValue() },
                { "MinionName", rewardData.minion.Data.name },
                { "MinionLevel", rewardData.minion.Level }
            });
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