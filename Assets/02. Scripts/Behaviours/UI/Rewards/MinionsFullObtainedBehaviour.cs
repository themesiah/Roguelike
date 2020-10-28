using System.Collections;
using System.Collections.Generic;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Laresistance.Core;

namespace Laresistance.Behaviours
{
    public class MinionsFullObtainedBehaviour : RewardPanelBehaviour
    {
        [SerializeField]
        private LocalizedStringTextBehaviour minionRewardText1 = default;
        [SerializeField]
        private Text minionRewardText2 = default;

        [SerializeField]
        private Text[] currentMinionNames = default;
        [SerializeField]
        private Text[] currentMinionAbilities = default;
        [SerializeField]
        private Transform[] currentMinionPrefabHolders = default;
        [SerializeField]
        private Image[] panels = default;
        [SerializeField]
        private int sortingOrder = 101;
        [SerializeField]
        private float scaleMultiplier = 40f;
        [SerializeField]
        private Material unlitMaterial = default;
        [SerializeField]
        private Color unselectedColor = default;
        [SerializeField]
        private Color selectedColor = default;

        private int minionIndexSelected = -2;

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            minionRewardText1.ChangeVariable(rewardData.minion.Name);
            minionRewardText2.text = rewardData.minion.GetAbilityText();

            foreach(Image panel in panels)
            {
                panel.color = unselectedColor;
            }

            // Show 3 current minions
            for(int i = 0; i < player.GetMinions().Length; ++i)
            {
                Minion m = player.GetMinions()[i];
                currentMinionNames[i].text = m.Name;
                currentMinionAbilities[i].text = m.GetAbilityText();
                foreach(Transform child in currentMinionPrefabHolders[i])
                {
                    Destroy(child.gameObject);
                }
                GameObject go = Instantiate(m.Data.Prefab, currentMinionPrefabHolders[i]);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = go.transform.localScale * scaleMultiplier;
                SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = sortingOrder;
                renderer.material = unlitMaterial;
            }
            // End show 3 current minions

            yield return base.StartingTween(rewardData);
        }

        protected override IEnumerator ExecutePanelProcess(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            minionIndexSelected = -2;
            while (minionIndexSelected < -1)
            {
                yield return null;
            }
            if (minionIndexSelected >= 0)
            {
                player.UnequipMinion(minionIndexSelected);
                player.EquipMinion(rewardData.minion);
                panels[minionIndexSelected].color = selectedColor;
            } else
            {
                panels[3].color = selectedColor;
                player.AddMinionToReserve(rewardData.minion);
            }
        }

        public void MinionASelected(InputAction.CallbackContext context)
        {
            minionIndexSelected = 2;
        }

        public void MinionSSelected(InputAction.CallbackContext context)
        {
            minionIndexSelected = 1;
        }

        public void MinionDSelected(InputAction.CallbackContext context)
        {
            minionIndexSelected = 0;
        }

        public void IgnoreSelected(InputAction.CallbackContext context)
        {
            minionIndexSelected = -1;
        }

        public override RewardUIType RewardType => RewardUIType.MinionFull;
    }
}