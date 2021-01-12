using Laresistance.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class HealthSubscription : MonoBehaviour
    {
        [SerializeField]
        private CharacterBattleBehaviour battleBehaviour = default;

        [SerializeField]
        private UnityEvent<int> OnMaxHealth = default;
        [SerializeField]
        private UnityEvent<int> OnDamageReceived = default;
        [SerializeField]
        private UnityEvent<int> OnHealReceived = default;
        [SerializeField]
        private UnityEvent<int> OnShieldPerformed = default;
        [SerializeField]
        private UnityEvent<int> OnHealthChanged = default;
        [SerializeField]
        private UnityEvent<float> OnHealthChangedPercent = default;
        [SerializeField]
        private UnityEvent OnDie = default;
        [SerializeField]
        private UnityEvent<string> OnHealthChangedString = default;
        [SerializeField]
        private UnityEvent<string> OnMaxHealthChangedString = default;

        private void OnEnable()
        {
            if (battleBehaviour.StatusManager == null)
            {
                battleBehaviour.OnStatusGenerated += EnableSuscriptions;
                return;
            }
            EnableSuscriptions();
        }

        private void OnDisable()
        {
            battleBehaviour.StatusManager.health.OnDamageTaken -= OnDamageTaken;
            battleBehaviour.StatusManager.health.OnHealed -= OnHealed;
            battleBehaviour.StatusManager.health.OnShieldsChanged -= OnShieldsChanged;
            battleBehaviour.StatusManager.health.OnDeath -= OnDeath;
            battleBehaviour.StatusManager.health.OnMaxHealthChanged -= MaxHealthChanged;
            battleBehaviour.StatusManager.health.OnHealthChanged -= HealthChanged;
        }

        private void EnableSuscriptions()
        {
            battleBehaviour.StatusManager.health.OnDamageTaken += OnDamageTaken;
            battleBehaviour.StatusManager.health.OnHealed += OnHealed;
            battleBehaviour.StatusManager.health.OnShieldsChanged += OnShieldsChanged;
            battleBehaviour.StatusManager.health.OnDeath += OnDeath;
            battleBehaviour.StatusManager.health.OnMaxHealthChanged += MaxHealthChanged;
            battleBehaviour.StatusManager.health.OnHealthChanged += HealthChanged;
            OnMaxHealth?.Invoke(battleBehaviour.StatusManager.health.GetMaxHealth());
            OnMaxHealthChangedString?.Invoke(battleBehaviour.StatusManager.health.GetMaxHealth().ToString());
            OnHealthChanged?.Invoke(battleBehaviour.StatusManager.health.GetCurrentHealth());
            OnHealthChangedString?.Invoke(battleBehaviour.StatusManager.health.GetCurrentHealth().ToString());
            OnHealthChangedPercent?.Invoke(battleBehaviour.StatusManager.health.GetPercentHealth());
        }

        private void OnDamageTaken(CharacterHealth sender, int damageTaken, int currentHealth)
        {
            OnDamageReceived?.Invoke(damageTaken);
            OnHealthChanged?.Invoke(currentHealth);
            OnHealthChangedString?.Invoke(currentHealth.ToString());
            HealthPercent(sender);
        }

        private void OnHealed(CharacterHealth sender, int healAmount, int currentHealth)
        {
            OnHealReceived?.Invoke(healAmount);
            OnHealthChanged?.Invoke(currentHealth);
            OnHealthChangedString?.Invoke(currentHealth.ToString());
            HealthPercent(sender);
        }

        private void OnShieldsChanged(CharacterHealth sender, int delta, int totalShields)
        {
            OnShieldPerformed?.Invoke(delta);
        }

        private void OnDeath(CharacterHealth sender)
        {
            OnDie.Invoke();
        }

        private void HealthPercent(CharacterHealth sender)
        {
            OnHealthChangedPercent?.Invoke(sender.GetPercentHealth());
        }

        private void MaxHealthChanged(CharacterHealth sender, int maxHealth)
        {
            OnMaxHealth?.Invoke(maxHealth);
            OnMaxHealthChangedString?.Invoke(maxHealth.ToString());
            OnHealthChangedPercent?.Invoke(sender.GetPercentHealth());
        }

        private void HealthChanged(CharacterHealth sender, int health)
        {
            OnHealthChanged?.Invoke(health);
            OnHealthChangedString?.Invoke(health.ToString());
            OnHealthChangedPercent?.Invoke(sender.GetPercentHealth());
        }
    }
}