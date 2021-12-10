using Laresistance.Core;
using Laresistance.Data;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine.Events;

namespace Laresistance.Battle
{
    public abstract class BattleEffect
    {
        protected int power;
        protected EffectTargetType TargetType;
        protected BattleStatusManager SelfStatus;
        private EffectData effectData;
        private BattleAbility parentAbility;

        protected delegate int CalculatePower(int finalPower);

        virtual protected CalculatePower calculatePowerFunction 
        { 
            get
            {
                return SelfStatus.battleStats.DummyCalculation;
            }
        }

        protected virtual int Power {
            get
            {
                int finalPower = calculatePowerFunction.Invoke(power);
                return finalPower;
            }
        }

        public BattleEffect(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData)
        {
            SetPower(power);
            TargetType = targetType;
            SelfStatus = selfStatus;
            this.effectData = effectData;
        }

        public void SetStatusManager(BattleStatusManager selfStatus)
        {
            SelfStatus = selfStatus;
        }

        public void SetParentAbility(BattleAbility ability)
        {
            parentAbility = ability;
        }

        protected bool IsComboAbility()
        {
            return parentAbility.data.IsComboSkill;
        }

        public BattleStatusManager GetStatusManager()
        {
            return SelfStatus;
        }

        public virtual int GetPower(int level, EquipmentsContainer equipments)
        {
            if (level <= 0)
                throw new System.Exception("Effect level should be greater than 0");
            return 0;
        }

        public void SetPower(int power)
        {
            this.power = power;
        }

        public virtual bool EffectCanBeUsedStunned()
        {
            return false;
        }

        public IEnumerator PerformEffect(BattleStatusManager[] allies, BattleStatusManager[] enemies, int level, EquipmentsContainer equipments, IBattleAnimator animator, ScriptableIntReference bloodRef, UnityAction onEffectFinished, UnityAction<int> signalsAmount)
        {
            List<BattleStatusManager> targets = GetTargets(allies, enemies);
            targets.ForEach((target) => PerformEffectOnTarget(target, level, equipments, bloodRef));
            SpawnSelfPrefabs(allies[0], onEffectFinished);

            targets.ForEach((target)=> {
                SpawnAbilityPrefabs(target, onEffectFinished);
            });

            signalsAmount.Invoke(effectData.SelfEffectPrefabs.Length + effectData.TargetEffectPrefabs.Length * targets.Count);
            yield return new WaitForSeconds(effectData.Delay);
        }

        private void SpawnSelfPrefabs(BattleStatusManager self, UnityAction callback)
        {
            foreach (var prefab in effectData.SelfEffectPrefabs)
            {
                var op = prefab.prefabReference.InstantiateAsync(self.TargetPivot, true);
                op.Completed += (obj) => {
                    GameObject go = obj.Result;
                    go.transform.localPosition = prefab.offset;
                    //go.transform.localScale = prefab.prefab.transform.localScale;
                    var effectCallback = go.GetComponent<BattleEffectCallback>();
                    UnityEngine.Assertions.Assert.IsNotNull(effectCallback);
                    effectCallback.ConfigureBattleEffectCallback(self, callback, prefab.delay);
                };
            }
        }

        private void SpawnAbilityPrefabs(BattleStatusManager target, UnityAction callback)
        {
            foreach(var prefab in effectData.TargetEffectPrefabs)
            {
                var op = prefab.prefabReference.InstantiateAsync(target.TargetPivot, true);
                op.Completed += (obj) =>
                {
                    GameObject go = obj.Result;
                    go.transform.localPosition = prefab.offset;
                    //go.transform.localScale = prefab.prefab.transform.localScale;
                    var effectCallback = go.GetComponent<BattleEffectCallback>();
                    UnityEngine.Assertions.Assert.IsNotNull(effectCallback);
                    effectCallback.ConfigureBattleEffectCallback(target, callback, prefab.delay);
                };                
            }
        }

        protected abstract void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef);

        public abstract EffectType EffectType { get; }

        public List<BattleStatusManager> GetTargets(BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            List<BattleStatusManager> statuses = new List<BattleStatusManager>();
            switch(TargetType)
            {
                case EffectTargetType.Self:
                    statuses.Add(allies[0]);
                    break;
                case EffectTargetType.Enemy:
                    statuses.Add(enemies[0]);
                    break;
                case EffectTargetType.AllAllies:
                    foreach(var ally in allies)
                    {
                        statuses.Add(ally);
                    }
                    break;
                case EffectTargetType.AllEnemies:
                    foreach (var enemy in enemies)
                    {
                        statuses.Add(enemy);
                    }
                    break;
                case EffectTargetType.AllCharacters:
                    foreach (var ally in allies)
                    {
                        statuses.Add(ally);
                    }
                    foreach (var enemy in enemies)
                    {
                        statuses.Add(enemy);
                    }
                    break;
                case EffectTargetType.RandomAlly:
                    statuses.Add(allies[Random.Range(0, allies.Length)]);
                    break;
                case EffectTargetType.RandomEnemy:
                    statuses.Add(enemies[Random.Range(0, enemies.Length)]);
                    break;
                case EffectTargetType.RandomCharacter:
                    List<BattleStatusManager> statusesTemp = new List<BattleStatusManager>();
                    foreach (var ally in allies)
                    {
                        statusesTemp.Add(ally);
                    }
                    foreach (var enemy in enemies)
                    {
                        statusesTemp.Add(enemy);
                    }
                    statuses.Add(statusesTemp[Random.Range(0, statusesTemp.Count)]);
                    break;
            }
            return statuses;
        }

        public abstract string GetEffectString(int level, EquipmentsContainer equipments);
        public abstract string GetShortEffectString(int level, EquipmentsContainer equipments);
        protected string GetTargetString()
        {
            string textId = "";
            switch (TargetType)
            {
                case EffectTargetType.Self:
                    textId = "EFF_TARGET_SELF";
                    break;
                case EffectTargetType.Enemy:
                    textId = "EFF_TARGET_ENEMY";
                    break;
                case EffectTargetType.AllAllies:
                    textId = "EFF_TARGET_SELF";
                    break;
                case EffectTargetType.AllEnemies:
                    textId = "EFF_TARGET_ENEMIES";
                    break;
                case EffectTargetType.AllCharacters:
                    textId = "EFF_TARGET_ALL_CHARS";
                    break;
                case EffectTargetType.RandomAlly:
                    textId = "NOTEXT";
                    break;
                case EffectTargetType.RandomEnemy:
                    textId = "EFF_TARGET_RANDOM_ENEMY";
                    break;
                case EffectTargetType.RandomCharacter:
                    textId = "EFF_TARGET_RANDOM_CHAR";
                    break;
            }
            return Texts.GetText(textId);
        }

        public abstract string GetAnimationTrigger();

        public virtual bool IsPrioritary => false;
    }
}