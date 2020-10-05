using NUnit.Framework;
using Laresistance.Battle;
using System.Collections.Generic;

namespace Laresistance.Tests
{
    public class Battle_Ability_Tests
    {
        [Test]
        public void When_GeneratingAbilityWithOneEffect()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            BattleAbility ability = new BattleAbility(effects, 1);
        }

        [Test]
        public void When_GeneratingAbilityWithTwoEffects()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            BattleAbility ability = new BattleAbility(effects, 1);
        }

        [Test]
        public void When_GeneratingAbilityWithThreeEffects()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            Assert.Catch(()=> { BattleAbility ability = new BattleAbility(effects, 1); });
        }

        [Test]
        public void When_GettingPowerFromOneEffect()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            BattleAbility ability = new BattleAbility(effects, 1);
            Assert.AreEqual(5, ability.GetEffectPower(0, 1));
            Assert.AreEqual(6, ability.GetEffectPower(0, 2));
        }

        [Test]
        public void When_GettingPowerFromTwoEffects()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            effects.Add(new BattleEffectDamage(7, Data.EffectTargetType.Enemy));
            BattleAbility ability = new BattleAbility(effects, 1);
            Assert.AreEqual(5, ability.GetEffectPower(0, 1));
            Assert.AreEqual(6, ability.GetEffectPower(0, 2));
            Assert.AreEqual(7, ability.GetEffectPower(1, 1));
            Assert.AreEqual(8, ability.GetEffectPower(1, 2));
        }

        [Test]
        public void When_GettingPowerWithInvalidIndex()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            BattleAbility ability = new BattleAbility(effects, 1);
            Assert.Catch(()=>ability.GetEffectPower(1, 0));
            Assert.Catch(()=>ability.GetEffectPower(2, 0));
            Assert.Catch(()=>ability.GetEffectPower(-1, 0));
        }

        [Test]
        public void When_GettingPowerWithInvalidLevel()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy));
            BattleAbility ability = new BattleAbility(effects, 1);
            Assert.Catch(()=>ability.GetEffectPower(0, 0));
            Assert.Catch(()=>ability.GetEffectPower(0, -1));
        }
    }
}