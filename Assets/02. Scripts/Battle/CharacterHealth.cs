using Laresistance.Behaviours;
using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class CharacterHealth
    {
        #region Inner Classes
        public class Shield
        {
            public int remainingAmount;
            public float timer;
        }
        #endregion

        #region Private Variables
        private int originalMaxHealth = 0;
        private int maxHealth = 0;
        private int currentHealth = 0;
        private List<Shield> currentShields = default;
        #endregion

        #region Events
        public delegate void OnDamageTakenHandler(CharacterHealth sender, int damageTaken, int currentHealth);
        public event OnDamageTakenHandler OnDamageTaken;
        public delegate void OnHealedHandler(CharacterHealth sender, int healAmount, int currentHealth);
        public event OnHealedHandler OnHealed;
        public delegate void OnShieldsChangedHandler(CharacterHealth sender, int delta, int totalShields, bool isDamage);
        public event OnShieldsChangedHandler OnShieldsChanged;
        public delegate void OnDeathHandler(CharacterHealth sender);
        public event OnDeathHandler OnDeath;
        public delegate void OnMaxHealthChangedHandler(CharacterHealth sender, int maxHealth);
        public event OnMaxHealthChangedHandler OnMaxHealthChanged;
        public delegate void OnHealthChangedHandler(CharacterHealth sender, int health);
        public event OnHealthChangedHandler OnHealthChanged;
        public delegate void OnAttackMissedHandler(CharacterHealth sender);
        public event OnAttackMissedHandler OnAttackMissed;
        #endregion

        #region Public API
        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetPercentHealth()
        {
            return (float)currentHealth / (float)maxHealth;
        }

        public int TotalShields()
        {
            int totalShields = 0;
            currentShields.ForEach((s) => {
                totalShields += s.remainingAmount;
            });
            return totalShields;
        }

        public CharacterHealth(int health)
        {
            maxHealth = health;
            originalMaxHealth = health;
            currentHealth = health;
            currentShields = new List<Shield>();
        }

        public void Heal(int power)
        {
            if (currentHealth <= 0)
                return;
            currentHealth += power;
            currentHealth = System.Math.Min(currentHealth, maxHealth);
            OnHealed?.Invoke(this, power, currentHealth);
        }

        public int TakeDamage(int power, EquipmentsContainer equipments)
        {
            if (currentHealth <= 0)
                return 0;
            int remainingPower = power;
            
            for (int i = currentShields.Count-1; i >= 0; --i)
            {
                Shield s = currentShields[i];
                if (s.remainingAmount > 0)
                {
                    if (s.remainingAmount >= remainingPower)
                    {
                        int last = remainingPower;
                        remainingPower = 0;
                        s.remainingAmount -= last;
                        OnShieldsChanged?.Invoke(this, -last, TotalShields(), true);
                        break;
                    } else
                    {
                        int last = s.remainingAmount;
                        remainingPower -= s.remainingAmount;
                        s.remainingAmount = 0;
                        currentShields.Remove(s);
                        OnShieldsChanged?.Invoke(this, -last, TotalShields(), true);
                    }
                } else
                {
                    currentShields.Remove(s);
                }
            }

            remainingPower = equipments.ModifyValue(Equipments.EquipmentSituation.DamageReceived, remainingPower);
            currentHealth -= remainingPower;
            currentHealth = System.Math.Max(currentHealth, 0);

            if (remainingPower > 0)
            {
                OnDamageTaken?.Invoke(this, remainingPower, currentHealth);
            }

            if (currentHealth <= 0)
            {
                OnDeath?.Invoke(this);
            }
            return remainingPower;
        }

        public void MissAttack()
        {
            OnAttackMissed?.Invoke(this);
        }

        public void AddShield(int power)
        {
            if (currentHealth <= 0)
                return;
            currentShields.Add(new Shield() { remainingAmount = power, timer = 0f });
            OnShieldsChanged?.Invoke(this, power, TotalShields(), true);
        }

        public void Tick(float delta)
        {
            if (!BattleAbilityManager.Executing || BattleAbilityManager.executingBasicSkill)
            {
                currentShields.ForEach((s) =>
                {
                    s.timer += delta;
                });
            }

            for (int i = currentShields.Count-1; i >= 0; --i)
            {
                if (currentShields[i].timer >= GameConstantsBehaviour.Instance.shieldDuration.GetValue())
                {
                    int amount = -currentShields[i].remainingAmount;
                    currentShields.RemoveAt(i);
                    OnShieldsChanged?.Invoke(this, amount, TotalShields(), false);
                }
            }
        }

        public void RecalculateMaxHealth(EquipmentsContainer equipments)
        {
            maxHealth = originalMaxHealth;
            maxHealth = equipments.ModifyValue(Equipments.EquipmentSituation.MaxHealth, maxHealth);
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
                OnHealthChanged?.Invoke(this, currentHealth);
            }
            OnMaxHealthChanged?.Invoke(this, maxHealth);
        }
        #endregion
    }
}