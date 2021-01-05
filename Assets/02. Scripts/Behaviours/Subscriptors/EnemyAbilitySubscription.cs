using Laresistance.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class EnemyAbilitySubscription : MonoBehaviour
    {
        [SerializeField]
        private EnemyBattleBehaviour enemyBattleBehaviour = default;
        [SerializeField]
        private UnityEvent<float> onAbilityCooldownProgress = default;
        [SerializeField]
        private UnityEvent<Sprite> onAbilitySpriteChanged = default;
        [SerializeField]
        private UnityEvent<Sprite> onAbilityFrameChanged = default;

        private EnemyAbilityManager enemyAbilityManager;
        private BattleStatusManager battleStatusManager;
        private Sprite nextSprite;
        private Sprite nextFrame;

        private bool initialized = false;

        private void Start()
        {
            enemyAbilityManager = ((EnemyAbilityManager)enemyBattleBehaviour.AbilityExecutor);
            battleStatusManager = enemyBattleBehaviour.StatusManager;

            RegisterEvents();
            initialized = true;
        }

        private void OnEnable()
        {
            if (initialized)
            {
                RegisterEvents();
            }
        }

        private void OnDisable()
        {
            enemyAbilityManager.OnAbilityCooldownProgress -= OnAbilityCooldownProgress;
            battleStatusManager.OnNextAbilityChanged -= OnNextAbilityChanged;
        }

        private void RegisterEvents()
        {
            enemyAbilityManager.OnAbilityCooldownProgress += OnAbilityCooldownProgress;
            battleStatusManager.OnNextAbilityChanged += OnNextAbilityChanged;
        }

        private void OnAbilityCooldownProgress(EnemyAbilityManager enemyAbilityManager, float progress)
        {
            onAbilityCooldownProgress?.Invoke(progress);
            if (progress < 1f && nextSprite != null)
            {
                onAbilitySpriteChanged?.Invoke(nextSprite);
            }
            if (progress < 1f && nextFrame != null)
            {
                onAbilityFrameChanged?.Invoke(nextFrame);
            }
        }

        private void OnNextAbilityChanged(BattleAbility ability)
        {
            nextSprite = ability.AbilityIcon;
            nextFrame = ability.AbilityFrame;
        }
    }
}