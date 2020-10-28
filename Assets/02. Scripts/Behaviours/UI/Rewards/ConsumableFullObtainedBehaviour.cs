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
        [SerializeField]
        private Image[] panels = default;
        [SerializeField]
        private Color unselectedColor = default;
        [SerializeField]
        private Color selectedColor = default;


        

        private int consumableIndexSelected = -1;

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            consumableRewardText1.ChangeVariable(rewardData.consumable.Name);
            consumableRewardText2.text = rewardData.consumable.GetAbilityText();

            foreach (Image panel in panels)
            {
                panel.color = unselectedColor;
            }

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
            consumableIndexSelected = -2;
            while (consumableIndexSelected < -1)
            {
                yield return null;
            }
            if (consumableIndexSelected >= 0)
            {
                player.DisposeConsumable(consumableIndexSelected);
                player.AddConsumable(rewardData.consumable);
                panels[consumableIndexSelected].color = selectedColor;
            }
            else
            {
                panels[3].color = selectedColor;
            }
        }

        public void ConsumableASelected(InputAction.CallbackContext context)
        {
            consumableIndexSelected = 2;
        }

        public void ConsumableDSelected(InputAction.CallbackContext context)
        {
            consumableIndexSelected = 0;
        }

        public void ConsumableSSelected(InputAction.CallbackContext context)
        {
            consumableIndexSelected = 1;
        }

        public void IgnoreSelected(InputAction.CallbackContext context)
        {
            consumableIndexSelected = -1;
        }

        public override RewardUIType RewardType => RewardUIType.ConsumableFull;
    }
}