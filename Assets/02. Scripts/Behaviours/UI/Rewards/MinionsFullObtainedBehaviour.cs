using System.Collections;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.UI;
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
        private int sortingOrder = 101;
        [SerializeField]
        private float scaleMultiplier = 40f;
        [SerializeField]
        private Material unlitMaterial = default;

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            player.statusManager.ResetStatus();
            rewardData.minion.StatusManager?.ResetStatus();
            minionRewardText1.ChangeVariable(rewardData.minion.Name);
            minionRewardText2.text = rewardData.minion.GetAbilityText();
            

            // Show 3 current minions
            for (int i = 0; i < player.GetMinions().Length; ++i)
            {
                Minion m = player.GetMinions()[i];
                currentMinionNames[i].text = m.Name;
                currentMinionAbilities[i].text = m.GetAbilityText();
                foreach(Transform child in currentMinionPrefabHolders[i])
                {
                    Destroy(child.gameObject);
                }
                m.Data.PrefabReference.InstantiateAsync(currentMinionPrefabHolders[i]).Completed += (handler) => {
                    GameObject go = handler.Result;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = go.transform.localScale * scaleMultiplier;
                    SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
                    renderer.sortingOrder = sortingOrder;
                    renderer.material = unlitMaterial;
                };
            }
            // End show 3 current minions

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
                player.UnequipMinion(selectedOptionIndex);
                player.EquipMinion(rewardData.minion);
            } else
            {
                player.AddMinionToReserve(rewardData.minion);
            }
        }

        public override RewardUIType RewardType => RewardUIType.MinionFull;
    }
}