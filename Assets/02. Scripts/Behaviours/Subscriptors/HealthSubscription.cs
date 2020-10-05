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

        private void OnEnable()
        {
            Debug.Log(name);
            battleBehaviour.StatusManager.health.OnDamageTaken += OnDamageTaken;
            battleBehaviour.StatusManager.health.OnHealed += OnHealed;
            battleBehaviour.StatusManager.health.OnShieldsChanged += OnShieldsChanged;
            battleBehaviour.StatusManager.health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            battleBehaviour.StatusManager.health.OnDamageTaken -= OnDamageTaken;
            battleBehaviour.StatusManager.health.OnHealed -= OnHealed;
            battleBehaviour.StatusManager.health.OnShieldsChanged -= OnShieldsChanged;
            battleBehaviour.StatusManager.health.OnDeath -= OnDeath;
        }

        private void OnDamageTaken(CharacterHealth sender, int damageTaken, int currentHealth)
        {
            OnDamageReceived?.Invoke(damageTaken);
            OnHealthChanged?.Invoke(currentHealth);
            HealthPercent(sender);
        }

        private void OnHealed(CharacterHealth sender, int healAmount, int currentHealth)
        {
            OnHealReceived?.Invoke(healAmount);
            OnHealthChanged?.Invoke(currentHealth);
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
    }
}