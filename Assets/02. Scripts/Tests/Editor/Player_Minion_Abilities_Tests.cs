using NUnit.Framework;
using Laresistance.Core;
using Laresistance.Battle;
using System.Collections.Generic;

namespace Laresistance.Tests
{
    public class Player_Minion_Abilities_Tests
    {
        //private BattleStatusManager GetStatus()
        //{
        //    return new BattleStatusManager(new CharacterHealth(100));
        //}
        //
        //[Test]
        //public void When_SettingAbilityOnMinion()
        //{
        //    List<BattleEffect> effects = new List<BattleEffect>();
        //    effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy, GetStatus()));
        //    BattleAbility ability = new BattleAbility(effects, 5, 0, 1f, null);
        //    Minion minion = new Minion(null, new BattleAbility[] { ability }, 1);
        //}
        //
        //[Test]
        //public void When_GettingAbilityPower()
        //{
        //    List<BattleEffect> effects = new List<BattleEffect>();
        //    effects.Add(new BattleEffectDamage(5, Data.EffectTargetType.Enemy, GetStatus()));
        //    effects.Add(new BattleEffectDamage(7, Data.EffectTargetType.Enemy, GetStatus()));
        //    BattleAbility ability = new BattleAbility(effects, 5, 0, 1f, null);
        //    Minion minion = new Minion(null, new BattleAbility[] { ability }, 1);
        //    Minion minion2 = new Minion(null, new BattleAbility[] { ability }, 2);
        //    Minion minion3 = new Minion(null, new BattleAbility[] { ability }, 3);
        //    Assert.AreEqual(5, minion.GetEffectPower(0));
        //    Assert.AreEqual(7, minion.GetEffectPower(1));
        //    Assert.AreEqual(6, minion2.GetEffectPower(0));
        //    Assert.AreEqual(8, minion2.GetEffectPower(1));
        //    Assert.AreEqual(6, minion3.GetEffectPower(0));
        //    Assert.AreEqual(9, minion3.GetEffectPower(1));
        //}
        //
        //[Test]
        //public void When_CreatingMinionWithInvalidLevel()
        //{
        //    Assert.Catch(() => { Minion minion = new Minion(null, null, 0); });
        //    Assert.Catch(() => { Minion minion2 = new Minion(null, null, -1); });
        //    Assert.Catch(() => { Minion minion3 = new Minion(null, null, 11); });
        //}
    }
}