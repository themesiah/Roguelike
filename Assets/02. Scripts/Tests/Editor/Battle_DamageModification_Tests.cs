﻿using NUnit.Framework;
using Laresistance.Core;
using Laresistance.Battle;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using Laresistance.Data;
using UnityEngine;

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
            float cooldown = 5f;
            BattleStatusManager status = GetStatus();
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamageModification(-20, Data.EffectTargetType.AllEnemies, status));
            BattleAbility ability = new BattleAbility(effects, cooldown, status);
            return ability;
        }

        [Test]
        public void When_ApplyingDamageModification()
        {
            BattleAbility damageModAbility = GetAbility();
            BattleStatusManager enemyStatus = GetStatus();
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
        }

        [Test]
        public void When_ApplyingDamageModificationAfterCooldown()
        {
            BattleAbility damageModAbility = GetAbility();
            BattleStatusManager enemyStatus = GetStatus();
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(5f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
        }

        [Test]
        public void When_ApplyingMultipleDamageModification()
        {
            BattleAbility damageModAbility = GetAbility();
            BattleStatusManager enemyStatus = GetStatus();
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, null);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, null);
            Assert.IsTrue(Mathf.Approximately(0.6f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            damageModAbility.Perform(new BattleStatusManager[] { damageModAbility.GetStatusManager() }, new BattleStatusManager[] { enemyStatus }, 1, null);
            Assert.IsTrue(Mathf.Approximately(0.4f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            Assert.IsTrue(Mathf.Approximately(0.6f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            Assert.IsTrue(Mathf.Approximately(0.8f, enemyStatus.GetDamageModifier()));
            enemyStatus.ProcessStatus(1f);
            Assert.IsTrue(Mathf.Approximately(1f, enemyStatus.GetDamageModifier()));
        }

        
    }
}