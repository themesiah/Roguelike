﻿using Laresistance.Battle;
using System.Collections.Generic;
using Laresistance.Data;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class EnemyBattleBehaviour : CharacterBattleBehaviour
    {
        [SerializeField]
        private EnemyData enemyData = default;
        [SerializeField]
        private ScriptableIntReference currentLevel = default;

        protected override void SetupStatusManager()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(enemyData.MaxHealth * (int)(1f + (currentLevel.GetValue() - 1) * 0.1f)));
        }

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = (IAbilityExecutor)AbilityInputProcessor;
        }

        protected override void SetupAbilityInputProcessor()
        {
            BattleAbility[] abilities = new BattleAbility[enemyData.AbilitiesData.Length];
            for(int i = 0; i < enemyData.AbilitiesData.Length; ++i)
            {
                abilities[i] = BattleAbilityFactory.GetBattleAbility(enemyData.AbilitiesData[i], null);
            }

            AbilityInputProcessor = new EnemyAbilityManager(abilities, 1);
        }
    }
}