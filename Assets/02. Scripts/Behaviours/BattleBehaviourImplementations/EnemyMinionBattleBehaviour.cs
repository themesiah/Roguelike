using Laresistance.Battle;
using System.Collections.Generic;
using Laresistance.Data;
using Laresistance.Core;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class EnemyMinionBattleBehaviour : CharacterBattleBehaviour
    {
        [SerializeField]
        private MinionData minionData = default;

        [SerializeField]
        private ScriptableIntReference currentLevel = default;

        private Minion minion;

        protected override void SetupStatusManager()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(minionData.MaxHealth * (int)(1f + (currentLevel.GetValue() - 1) * 0.1f)));
        }

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = (IAbilityExecutor)AbilityInputProcessor;
        }

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new EnemyAbilityManager(minion.Abilities, minion.Level);
        }

        protected override void Start()
        {
            minion = MinionFactory.GetMinion(minionData, currentLevel.GetValue(), null);
            base.Start();
        }
    }
}