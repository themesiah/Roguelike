using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class EnemyAbilityManager : IAbilityInputProcessor, IAbilityExecutor
    {
        private static float LAST_ABILITY_REACTED_TO_TIMER_START = 1f;
        private BattleAbility[] abilities;
        private int level;
        private IBattleAnimator animator;
        private BattleStatusManager selfStatus;
        private CharacterBattleManager selfBattleManager;

        private BattleAbility nextAbility = null;
        private float nextAbilityTimer;
        private float nextAbilityCooldown;
        private float lastAbilityReactedToTimer = 0f; // This timer avoids to stop reacting to the same ability. After the timer reaches 0, the last ability reacted to is considered expired.

        public float CooldownProgress { get { return 1f-(nextAbilityTimer / nextAbilityCooldown); } }

        public delegate void OnAbilityCooldownProgressHandler(EnemyAbilityManager enemyAbilityManager, float progress);
        public event OnAbilityCooldownProgressHandler OnAbilityCooldownProgress;

        private BattleAbility lastAbilityReactedTo = null;


        public EnemyAbilityManager(BattleAbility[] abilities, int level, BattleStatusManager selfStatus)
        {
            this.abilities = abilities;
            this.level = level;
            this.selfStatus = selfStatus;
            nextAbilityTimer = GameConstantsBehaviour.Instance.nextAbilityCooldownDefault.GetValue();
            nextAbilityCooldown = GameConstantsBehaviour.Instance.nextAbilityCooldownDefault.GetValue();
            this.selfStatus.SetBattleManager += SetCharacterBattleManager;
        }

        public void SetAnimator(IBattleAnimator animator)
        {
            this.animator = animator;
        }

        public void SetCharacterBattleManager(CharacterBattleManager battleManager)
        {
            selfBattleManager = battleManager;
        }

        public AbilityExecutionData GetAbilitiesToExecute(BattleStatusManager battleStatus, float delta)
        {
            // If there is no ability in queue we select one and configure the timers.
            if (nextAbility == null)
            {
                nextAbility = GetRandomAbilityFromWeights(AbilityDataAISpecification.Always);
                if (nextAbility != null)
                {
                    battleStatus.SetNextAbility(nextAbility);
                    float variance = GameConstantsBehaviour.Instance.nextAbilityCooldownVariance.GetValue();
                    if (nextAbility.data.CastTime != 0)
                    {
                        nextAbilityTimer = nextAbility.data.CastTime + Random.Range(-variance, variance);
                    }
                    else
                    {
                        nextAbilityTimer = GameConstantsBehaviour.Instance.nextAbilityCooldownDefault.GetValue() + Random.Range(-variance, variance);
                    }
                    nextAbilityCooldown = nextAbilityTimer;
                }
            }


            // Cooldowns. This is the ability charge process. When it finishes, the enemy executes the ability.
            for(int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == null)
                    continue;
                abilities[i].Tick(delta);
            }

            // Last ability reaction expiration time
            if (lastAbilityReactedToTimer > 0f)
            {
                lastAbilityReactedToTimer -= delta;
                lastAbilityReactedToTimer = Mathf.Max(0f, lastAbilityReactedToTimer);
                if (lastAbilityReactedToTimer == 0f)
                {
                    lastAbilityReactedTo = null;
                }
            }

            // Check if any ability can be used right now (timer at 0 AND is the next ability) and returns the data.
            for (int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == null)
                    continue;

                if (abilities[i].CanBeUsed() && nextAbilityTimer <= 0f)
                {
                    if (abilities[i] == nextAbility)
                    {
                        return new AbilityExecutionData() { index = i, selectedTarget = null }; ;
                    } else
                    {
                    }
                }
            }


            if (!selfStatus.Stunned)
            {
                // Situational skills
                var abilityToCast = CheckSelfDebuffAbilities();
                if (abilityToCast != -1)
                {
                    return new AbilityExecutionData() { index = abilityToCast, selectedTarget = null };
                }
                abilityToCast = CheckEnemyBuffAbilities();
                if (abilityToCast != -1)
                {
                    return new AbilityExecutionData() { index = abilityToCast, selectedTarget = null };
                }
                abilityToCast = CheckNotFullLife();
                if (abilityToCast != -1)
                {
                    return new AbilityExecutionData() { index = abilityToCast, selectedTarget = null };
                }

                // Update shield timers and set status if necessary
                UpdateShieldTimers();
                // Update parry timers and set status if necessary
                UpdateParryTimers();
                // Check if should execute shield
                abilityToCast = CheckUseShield();
                if (abilityToCast != -1)
                {
                    return new AbilityExecutionData() { index = abilityToCast, selectedTarget = null };
                }
                // Check if should execute parry
                abilityToCast = CheckUseParry();
                if (abilityToCast != -1)
                {
                    return new AbilityExecutionData() { index = abilityToCast, selectedTarget = null };
                }
            }

            // The timers and cooldowns only advance if there is no other ability executing right now.
            if (!BattleAbilityManager.Instance.Executing && !battleStatus.Stunned && BattleAbilityManager.Instance.QueueIsEmpty && !BattleAbilityManager.Instance.AbilityInQueue)
            {
                nextAbilityTimer -= delta;
                OnAbilityCooldownProgress?.Invoke(this, CooldownProgress);
                // Internal Cooldowns. This is the ability cooldown if the ability is executed in an special situation.
                for (int i = 0; i < abilities.Length; ++i)
                {
                    if (abilities[i] == null)
                        continue;
                    abilities[i].TickInternalCooldown(delta);
                }
            }

            // Default return data. This means no ability is going to be used this frame by this enemy.
            return new AbilityExecutionData() { index = -1, selectedTarget = null};
        }

        public IEnumerator ExecuteAbility(int abilityIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            if (abilityIndex < 0 || abilityIndex > abilities.Length - 1)
                throw new System.Exception("Invalid index for executing ability");
            yield return abilities[abilityIndex].ExecuteAbility(allies, enemies, level, animator);
            if (abilities[abilityIndex].data.AiSpecification == AbilityDataAISpecification.Always)
                nextAbility = null;
        }

        public BattleAbility[] GetAbilities()
        {
            return abilities;
        }

        public void BattleStart()
        {

        }

        public void BattleEnd()
        {

        }

        private BattleAbility GetRandomAbilityFromWeights(AbilityDataAISpecification aiSpecification, bool requireInternalTimer = false)
        {
            List<BattleAbility> weightedAbilities = new List<BattleAbility>();
            foreach (var ability in abilities)
            {
                if (ability != null)
                {
                    if (!requireInternalTimer || ability.CanBeUsedInternalTimer())
                    {
                        for (int i = 0; i < ability.Weight; ++i)
                        {
                            if (ability.data.AiSpecification == aiSpecification)
                            {
                                weightedAbilities.Add(ability);
                            }
                        }
                    }
                }
            }
            if (weightedAbilities.Count == 0)
            {
                return null; // This should only be the case for an enemy with only a reactive skill, like a shield minion
            }

            // Default case in case something goes wrong.
            return weightedAbilities[Random.Range(0, weightedAbilities.Count)];
        }

        public void PerformTimeStop(bool activate)
        {
        }

        private int GetAbilityIndex(BattleAbility ability)
        {
            for (int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == ability)
                    return i;
            }
            return -1;
        }

        private int CheckSelfDebuffAbilities()
        {
            // Check if self debuff ability exists and can be used
            var ability = GetRandomAbilityFromWeights(AbilityDataAISpecification.WhenSelfHaveDebuff, true);
            if (ability != null)
            {
                // Check if self have debuffs
                if (selfStatus.HaveDebuff())
                {
                    return GetAbilityIndex(ability);
                }
            }
            return -1;
        }

        private int CheckEnemyBuffAbilities()
        {
            // Check if enemy buff ability exists and can be used
            var ability = GetRandomAbilityFromWeights(AbilityDataAISpecification.WhenEnemyHaveBuff, true);
            if (ability != null && selfBattleManager != null && selfBattleManager.Enemies.Length > 0)
            {
                // Check if enemy have buffs
                if (selfBattleManager.Enemies[0].StatusManager.HaveBuff())
                {
                    return GetAbilityIndex(ability);
                }
            }
            return -1;
        }

        private void UpdateShieldTimers()
        {
            // Check if self shield ability exists and can be used
            var ability = GetRandomAbilityFromWeights(AbilityDataAISpecification.WhenAllyAttacked, true);
            if (ability != null && !selfStatus.WillBlock && !selfStatus.WillParry)
            {
                selfStatus.PrepareShield(()=> { ability.SetCooldownAsUsed(); });
            }
        }

        private void UpdateParryTimers()
        {
            // Check if self parry ability exists and can be used
            var ability = GetRandomAbilityFromWeights(AbilityDataAISpecification.WhenAttacked, true);
            if (ability != null && !selfStatus.WillParry && !selfStatus.WillBlock)
            {
                selfStatus.PrepareParry(() => { ability.SetCooldownAsUsed(); });
            }
        }

        private int CheckUseShield()
        {
            var ability = GetRandomAbilityFromWeights(AbilityDataAISpecification.WhenAllyAttacked, true);
            if (ability != null && selfStatus.WillBlock)
            {
                // Check if enemy is attacking self or ally
                if (BattleAbilityManager.Instance.currentAbility != null
                    && BattleAbilityManager.Instance.currentAbility.IsOffensiveAbility
                    && lastAbilityReactedTo != BattleAbilityManager.Instance.currentAbility
                    && !BattleAbilityManager.Instance.currentAbility.IsPrioritary()
                    )
                {
                    foreach (var target in BattleAbilityManager.Instance.currentTargets)
                    {
                        if (selfBattleManager != null && selfBattleManager.IsAlly(target))
                        {
                            selfStatus.BlockExecuted();
                            lastAbilityReactedTo = BattleAbilityManager.Instance.currentAbility;
                            lastAbilityReactedToTimer = LAST_ABILITY_REACTED_TO_TIMER_START;
                            return GetAbilityIndex(ability);
                        }
                    }
                }
            }
            return -1;
        }

        private int CheckUseParry()
        {
            var ability = GetRandomAbilityFromWeights(AbilityDataAISpecification.WhenAttacked, true);
            if (ability != null && selfStatus.WillParry)
            {
                // Check if enemy is attacking self
                if (BattleAbilityManager.Instance.currentAbility != null
                    && BattleAbilityManager.Instance.currentAbility.IsOffensiveAbility
                    && lastAbilityReactedTo != BattleAbilityManager.Instance.currentAbility
                    && !BattleAbilityManager.Instance.currentAbility.IsPrioritary()
                    )
                {
                    if (BattleAbilityManager.Instance.currentTargets[0] == selfStatus)
                    {
                        selfStatus.ParryExecuted();
                        lastAbilityReactedTo = BattleAbilityManager.Instance.currentAbility;
                        lastAbilityReactedToTimer = LAST_ABILITY_REACTED_TO_TIMER_START;
                        return GetAbilityIndex(ability);
                    }
                }
            }
            return -1;
        }

        private int CheckNotFullLife()
        {
            // Check if heal ability exists and can be used
            var ability = GetRandomAbilityFromWeights(AbilityDataAISpecification.WhenNotFullLife, true);
            if (ability != null)
            {
                // Check if self health is less than 100%
                if (selfStatus.health.GetPercentHealth() < 1f)
                {
                    return GetAbilityIndex(ability);
                }
            }
            return -1;
        }
    }
}