using Laresistance.Data;
using Laresistance.Core;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class ConsumableFullObtainedBehaviour : RewardPanelBehaviour
    {
        [SerializeField]
        private LocalizedStringTextBehaviour consumableRewardText1 = default;
        [SerializeField]
        private Text consumableRewardText2 = default;

        [SerializeField]
        private Text[] currentConsumablesNames = default;
        [SerializeField]
        private Text[] currentConsumablesAbilities = default;
        [SerializeField]
        private Image[] currentConsumablesImages = default;


        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            consumableRewardText1.ChangeVariable(rewardData.consumable.Name);
            consumableRewardText2.text = rewardData.consumable.GetAbilityText();

            // Show 2 current consumables
            for (int i = 0; i < player.GetConsumables().Length; ++i)
            {
                Consumable c = player.GetConsumables()[i];
                currentConsumablesNames[i].text = c.Name;
                currentConsumablesAbilities[i].text = c.GetAbilityText();
                currentConsumablesImages[i].sprite = c.Data.SpriteReference;
            }
            // End show 2 current consumables

            yield return base.StartingTween(rewardData);
        }

        protected override IEnumerator ExecutePanelProcess(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            selectedOptionIndex = -2;
            while (selectedOptionIndex < -1)
            {
                yield return null;
            }
            if (selectedOptionIndex >= 0)
            {
                player.DisposeConsumable(selectedOptionIndex);
                player.AddConsumable(rewardData.consumable);
            }
        }

        public override RewardUIType RewardType => RewardUIType.ConsumableFull;
    }
}