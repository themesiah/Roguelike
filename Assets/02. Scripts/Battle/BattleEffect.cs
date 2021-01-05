using Laresistance.Core;
using Laresistance.Data;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Battle
{
    public abstract class BattleEffect
    {
        protected int Power;
        protected EffectTargetType TargetType;
        protected BattleStatusManager SelfStatus;
        private bool primaryEffect = false;

        public BattleEffect(int power, EffectTargetType targetType, BattleStatusManager selfStatus)
        {
            SetPower(power);
            TargetType = targetType;
            SelfStatus = selfStatus;
        }

        public void SetStatusManager(BattleStatusManager selfStatus)
        {
            SelfStatus = selfStatus;
        }

        public BattleStatusManager GetStatusManager()
        {
            return SelfStatus;
        }

        public void SetAnimationPrimaryEffect()
        {
            primaryEffect = true;
        }

        public virtual int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            if (level <= 0)
                throw new System.Exception("Effect level should be greater than 0");
            return 0;
        }

        public void SetPower(int power)
        {
            Power = power;
        }

        public void PerformEffect(BattleStatusManager[] allies, BattleStatusManager[] enemies, int level, EquipmentEvents equipmentEvents, IBattleAnimator animator, ScriptableIntReference bloodRef = null)
        {
            List<BattleStatusManager> targets = GetTargets(allies, enemies);
            if (primaryEffect)
            {
                Vector3 targetPoint = Vector3.zero;
                targets.ForEach((target) =>
                {
                    if (target.TargetPivot != null)
                    {
                        targetPoint += target.TargetPivot.transform.position;
                        Debug.LogFormat("Position for character {0} is {1}", target.TargetPivot.name, target.TargetPivot.transform.position);
                    }
                });

                targetPoint = targetPoint / targets.Count;
                animator.SetAttackPosition(targetPoint);
            }
            targets.ForEach((target) => PerformEffectOnTarget(target, level, equipmentEvents, bloodRef));
        }

        protected abstract void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef);

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

        public abstract string GetEffectString(int level, EquipmentEvents equipmentEvents);
        public abstract string GetShortEffectString(int level, EquipmentEvents equipmentEvents);
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