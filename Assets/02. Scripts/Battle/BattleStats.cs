using Laresistance.Behaviours;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleStats
    {
        private BattleStatusManager statusManager;

        private int damageStat = 0;
        private int shieldStat = 0;
        private int healStat = 0;
        private int statusTimeStat = 0;
        private int maxHealthStat = 0;

        public BattleStats(BattleStatusManager statusManager)
        {
            this.statusManager = statusManager;
        }

        public void UpgradeStat(StatsType statType)
        {
            Debug.LogFormat("Upgrading stat {0}", statType.ToString());
            switch(statType)
            {
                case StatsType.Damage:
                    UpgradeDamage();
                    break;
                case StatsType.Shield:
                    UpgradeShield();
                    break;
                case StatsType.Heal:
                    UpgradeHeal();
                    break;
                case StatsType.StatusTime:
                    UpgradeStatusTime();
                    break;
                case StatsType.MaxHealth:
                    UpgradeMaxHealth();
                    break;
            }
        }

        public void UpgradeDamage()
        {
            damageStat++;
        }

        public void UpgradeShield()
        {
            shieldStat++;
        }

        public void UpgradeHeal()
        {
            healStat++;
        }

        public void UpgradeStatusTime()
        {
            statusTimeStat++;
        }

        public void UpgradeMaxHealth()
        {
            maxHealthStat++;
            int lastMaxHealth = statusManager.health.GetMaxHealth();
            statusManager.health.RecalculateMaxHealth(statusManager.GetEquipmentsContainer());
            int deltaMaxHealth = statusManager.health.GetMaxHealth() - lastMaxHealth;
            statusManager.health.Heal(deltaMaxHealth);
        }

        public int DummyCalculation(int power)
        {
            return power;
        }

        public int CalculateDamage(int power)
        {
            return Mathf.FloorToInt(power * (1f + damageStat * GameConstantsBehaviour.Instance.damageStatCoeficient.GetValue()));
        }

        public int CalculateShield(int power)
        {
            return Mathf.FloorToInt(power * (1f + shieldStat * GameConstantsBehaviour.Instance.shieldStatCoeficient.GetValue()));
        }

        public float CalculateShieldTime(float time)
        {
            return time * (1f + shieldStat * GameConstantsBehaviour.Instance.shieldTimeStatCoeficient.GetValue());
        }

        public int CalculateHeal(int power)
        {
            return Mathf.FloorToInt(power * (1f + healStat * GameConstantsBehaviour.Instance.healStatCoeficient.GetValue()));
        }

        public float CalculateStatusTime(float time)
        {
            return time * (1f + statusTimeStat * GameConstantsBehaviour.Instance.statusTimeStatCoeficient.GetValue());
        }

        public int CalculateHealth(int maxHealth)
        {
            return maxHealth + (maxHealthStat * GameConstantsBehaviour.Instance.maxHealthStatAmount.GetValue());
        }

        public int[] GetStats()
        {
            return new int[] { damageStat, shieldStat, healStat, statusTimeStat, maxHealthStat };
        }
    }
}