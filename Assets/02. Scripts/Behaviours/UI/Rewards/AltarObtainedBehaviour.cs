using Laresistance.Core;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class AltarObtainedBehaviour : RewardPanelBehaviour
    {
        [SerializeField]
        private GameObject[] statObjects = default;
        [SerializeField]
        private Image[] statIcons = default;
        [SerializeField]
        private Text[] statTitles = default;
        [SerializeField]
        private Text[] statDescriptions = default;
        [SerializeField]
        private StatsInfo[] statsInfo = default;

        public override RewardUIType RewardType => RewardUIType.Altar;

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            var stats = rewardData.statsTypeList;
            // Activate only necessary amount of stats
            foreach(var obj in statObjects)
            {
                obj.SetActive(false);
            }
            for (int i = 0; i < 3; ++i)
            {
                var statInfo = GetStatInfo(stats[i]);
                statObjects[i].SetActive(true);
                statIcons[i].sprite = statInfo.StatSprite;
                statTitles[i].text = statInfo.GetTitle();
                statDescriptions[i].text = statInfo.GetDescription();
            }

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

            player.statusManager.battleStats.UpgradeStat(rewardData.statsTypeList[selectedOptionIndex+1]);
        }

        private StatsInfo GetStatInfo(StatsType type)
        {
            foreach(var statInfo in statsInfo)
            {
                if (statInfo.StatType == type)
                    return statInfo;
            }
            return null;
        }
    }
}