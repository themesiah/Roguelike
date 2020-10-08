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


        private GameObject bloodObtainedPanel;

        public RewardSystem(Player player, ScriptableIntReference bloodReference, ScriptableIntReference hardCurrencyReference)
        {
            this.player = player;
            this.bloodReference = bloodReference;
            this.hardCurrencyReference = hardCurrencyReference;
            UnityEngine.Assertions.Assert.IsNotNull(player, "Player value in Reward System can not be null");
            UnityEngine.Assertions.Assert.IsNotNull(bloodReference, "Blood reference in Reward System can not be null");
            UnityEngine.Assertions.Assert.IsNotNull(hardCurrencyReference, "Hard currency reference in Reward System can not be null");
        }

        public IEnumerator GetReward(RewardData reward)
        {
            if (reward.bloodAmount > 0)
            {
                bloodReference.SetValue(bloodReference.GetValue() + reward.bloodAmount);
                yield return BloodObtainedBehaviour.instance.StartPanel(reward);
            }

            if (reward.minion != null)
            {
                var minions = player.GetMinions();
                bool autoEquipped = false;
                for (int i = 0; i < minions.Length; ++i)
                {
                    if (minions[i] == null)
                    {
                        player.EquipMinion(reward.minion);
                        autoEquipped = true;
                        break;
                    }
                }

                if (autoEquipped)
                {
                    // Show message telling you obtained the minion
                    yield return MinionObtainedBehaviour.instance.StartPanel(reward);
                }
                else
                {
                    // Show options to switch minions
                }
            }

            if (reward.consumable != null)
            {
                var consumables = player.GetConsumables();
                bool autoEquipped = false;
                for (int i = 0; i < consumables.Length; ++i)
                {
                    if (consumables[i] == null)
                    {
                        player.AddConsumable(reward.consumable);
                        autoEquipped = true;
                    }
                }

                if (autoEquipped)
                {
                    // Show message telling you obtained the consumable
                }
                else
                {
                    // Show options to switch consumables
                }
            }
            yield return null;
        }
    }
}
