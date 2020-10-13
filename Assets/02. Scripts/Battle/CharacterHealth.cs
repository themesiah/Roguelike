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

        #region Constants
        public static float SHIELD_DURATION = 0.5f;
        #endregion

        #region Private Variables
        private int maxHealth = 0;
        private int currentHealth = 0;
        private List<Shield> currentShields = default;
        #endregion

        #region Events
        public delegate void OnDamageTakenHandler(CharacterHealth sender, int damageTaken, int currentHealth);
        public event OnDamageTakenHandler OnDamageTaken;
        public delegate void OnHealedHandler(CharacterHealth sender, int healAmount, int currentHealth);
        public event OnHealedHandler OnHealed;
        public delegate void OnShieldsChangedHandler(CharacterHealth sender, int delta, int totalShields);
        public event OnShieldsChangedHandler OnShieldsChanged;
        public delegate void OnDeathHandler(CharacterHealth sender);
        public event OnDeathHandler OnDeath;
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
            currentHealth = health;
            currentShields = new List<Shield>();
        }

        public void Heal(int power)
        {
            currentHealth += power;
            currentHealth = System.Math.Min(currentHealth, maxHealth);
            OnHealed?.Invoke(this, power, currentHealth);
        }

        public void TakeDamage(int power)
        {
            Debug.Log("Damage taken: " + power);
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
                        OnShieldsChanged?.Invoke(this, -last, TotalShields());
                        break;
                    } else
                    {
                        int last = s.remainingAmount;
                        remainingPower -= s.remainingAmount;
                        s.remainingAmount = 0;
                        currentShields.Remove(s);
                        OnShieldsChanged?.Invoke(this, -last, TotalShields());
                    }
                } else
                {
                    currentShields.Remove(s);
                }
            }

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
        }

        public void AddShield(int power)
        {
            currentShields.Add(new Shield() { remainingAmount = power, timer = 0f });
            OnShieldsChanged?.Invoke(this, power, TotalShields());
        }

        public void Tick(float delta)
        {
            currentShields.ForEach((s) => {
                s.timer += delta;
            });

            for (int i = currentShields.Count-1; i >= 0; --i)
            {
                if (currentShields[i].timer >= SHIELD_DURATION)
                {
                    currentShields.RemoveAt(i);
                }
            }
        }
        #endregion
    }
}