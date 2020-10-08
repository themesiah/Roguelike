﻿using NUnit.Framework;
using Laresistance.Battle;

namespace Laresistance.Tests
{
    public class Battle_Effect_Tests
    {
        private static int BASE_LEVEL = 1;

        private BattleStatusManager GetStatus()
        {
            return new BattleStatusManager(new CharacterHealth(100));
        }

        [Test]
        public void When_ObtainingEffectPowerAtBaseLevel()
        {
            int power = 5;
            BattleEffectDamage effect = new BattleEffectDamage(power, Data.EffectTargetType.Enemy, GetStatus());
            Assert.AreEqual(power, effect.GetPower(BASE_LEVEL, null));
        }

        [Test]
        public void When_ObtainingEffectPowerAtIncorrectLevel()
        {
            int power = 5;
            BattleEffectDamage effect = new BattleEffectDamage(power, Data.EffectTargetType.Enemy, GetStatus());
            Assert.Catch(()=> { effect.GetPower(0, null); });
            Assert.Catch(()=> { effect.GetPower(-1, null); });
        }

        [Test]
        public void When_ObtainingEffectPowerAtDifferentLevels()
        {
            int power = 5;
            BattleEffectDamage effect = new BattleEffectDamage(power, Data.EffectTargetType.Enemy, GetStatus());
            Assert.AreEqual(6, effect.GetPower(2, null));
            Assert.AreEqual(6, effect.GetPower(3, null));
            Assert.AreEqual(7, effect.GetPower(4, null));
            Assert.AreEqual(7, effect.GetPower(5, null));
        }
    }
}