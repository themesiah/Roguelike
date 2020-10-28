using Laresistance.Data;
using Laresistance.Core;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections;
using GamedevsToolbox.Utils;
using UnityEngine;
using Laresistance.Behaviours;

namespace Laresistance.Systems
{
    public class RewardSystem
    {
        private Player player;
        private ScriptableIntReference bloodReference;
        private ScriptableIntReference hardCurrencyReference;
        private RewardUILibrary rewardUILibrary;

        public RewardSystem(Player player, ScriptableIntReference bloodReference, ScriptableIntReference hardCurrencyReference, RewardUILibrary rewardUILibrary)
        {
            this.player = player;
            this.bloodReference = bloodReference;
            this.hardCurrencyReference = hardCurrencyReference;
            this.rewardUILibrary = rewardUILibrary;
            UnityEngine.Assertions.Assert.IsNotNull(player, "Player value in Reward System can not be null");
            UnityEngine.Assertions.Assert.IsNotNull(bloodReference, "Blood reference in Reward System can not be null");
            UnityEngine.Assertions.Assert.IsNotNull(hardCurrencyReference, "Hard currency reference in Reward System can not be null");
        }

        public IEnumerator GetReward(RewardData reward)
        {
            if (reward.bloodAmount > 0)
            {
                // Show message telling you obtained blood
                bloodReference.SetValue(bloodReference.GetValue() + reward.bloodAmount);
                yield return rewardUILibrary?.GetBehaviour(RewardUIType.Blood).StartPanel(reward);
            }

            if (reward.hardCurrencyAmount > 0)
            {
                // Show message telling you obtained hard currency
                hardCurrencyReference.SetValue(hardCurrencyReference.GetValue() + reward.hardCurrencyAmount);
                yield return rewardUILibrary?.GetBehaviour(RewardUIType.HardCurrency).StartPanel(reward);
            }

            if (reward.minion != null)
            {
                bool autoEquipped = reward.minion.SetInSlot(player);

                if (autoEquipped)
                {
                    // Show message telling you obtained the minion
                    yield return rewardUILibrary?.GetBehaviour(RewardUIType.Minion).StartPanel(reward);
                }
                else
                {
                    // Show options to switch minions
                    yield return rewardUILibrary?.GetBehaviour(RewardUIType.MinionFull).StartPanel(reward);
                }
            }

            if (reward.consumable != null)
            {
                bool autoEquipped = reward.consumable.SetInSlot(player);

                if (autoEquipped)
                {
                    // Show message telling you obtained the consumable
                    yield return rewardUILibrary?.GetBehaviour(RewardUIType.Consumable).StartPanel(reward);
                }
                else
                {
                    // Show options to switch consumables
                    yield return rewardUILibrary?.GetBehaviour(RewardUIType.ConsumableFull).StartPanel(reward);
                }
            }

            if (reward.equip != null)
            {
                // Equips are never autoequipped, as they may not be always good
                yield return rewardUILibrary?.GetBehaviour(RewardUIType.Equip).StartPanel(reward);
            }

            if (reward.mapAbilityData != null)
            {
                reward.mapAbilityData.AbilityObtainedReference.SetValue(true);
                yield return rewardUILibrary?.GetBehaviour(RewardUIType.MapAbility).StartPanel(reward);
            }
            yield return null;
        }
    }
}
