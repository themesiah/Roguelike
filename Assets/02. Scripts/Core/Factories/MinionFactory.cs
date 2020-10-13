using Laresistance.Data;
using Laresistance.Battle;

namespace Laresistance.Core
{
    public class MinionFactory
    {
        public static Minion GetMinion(MinionData minionData, int level, EquipmentEvents events, BattleStatusManager battleStatus)
        {
            UnityEngine.Assertions.Assert.IsTrue(minionData.AbilitiesData.Length > 0);
            BattleAbility ability = BattleAbilityFactory.GetBattleAbility(minionData.AbilitiesData[0], events, battleStatus);
            Minion minion = new Minion(minionData, ability, level);
            return minion;
        }
    }
}