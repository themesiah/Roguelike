using NUnit.Framework;
using Laresistance.Core;
using Laresistance.Battle;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using Laresistance.Data;
using UnityEngine;
using Laresistance.Behaviours;

namespace Laresistance.Tests
{
    public class Battle_DamageModification_Tests
    {
        private BattleStatusManager GetStatus()
        {
            return new BattleStatusManager(new CharacterHealth(100));
        }

        private BattleAbility GetAbility()
        {
            int cost = 5;
            BattleStatusManager status = GetStatus();
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamageModification(-20, Data.EffectTargetType.AllEnemies, status));
            BattleAbility ability = new BattleAbility(effects, cost, 0, 1f, status);
            return ability;
        }

        [Test]
        public void When_ApplyingDamageModification()
        {
            BattleAbility damageModAbility = GetAbility();
            BattleStatusManager enemyStatus = GetStatus();
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, new DummyBattleAnimator(), null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f, 1f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
        }

        [Test]
        public void When_ApplyingDamageModificationAfterCooldown()
        {
            BattleAbility damageModAbility = GetAbility();
            BattleStatusManager enemyStatus = GetStatus();
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, new DummyBattleAnimator(), null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f, 1f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, new DummyBattleAnimator(), null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f, 1f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, new DummyBattleAnimator(), null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f, 1f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
        }

        [Test]
        public void When_ApplyingMultipleDamageModification()
        {
            BattleAbility damageModAbility = GetAbility();
            BattleStatusManager enemyStatus = GetStatus();
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, new DummyBattleAnimator(), null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, new DummyBattleAnimator(), null);
            Assert.IsTrue(Mathf.Approximately(0.6f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, new DummyBattleAnimator(), null);
            Assert.IsTrue(Mathf.Approximately(0.4f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            Assert.IsTrue(Mathf.Approximately(0.6f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f, 1f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
        }

        
    }
}