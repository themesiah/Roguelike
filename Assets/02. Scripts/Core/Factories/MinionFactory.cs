using Laresistance.Data;
using Laresistance.Battle;

namespace Laresistance.Core
{
    public class MinionFactory
    {
        public static Minion GetMinion(MinionData minionData, int level, EquipmentsContainer equipments, BattleStatusManager battleStatus)
        {
            UnityEngine.Assertions.Assert.IsTrue(minionData.AbilitiesData.Length > 0);
            UnityEngine.Assertions.Assert.IsNotNull(equipments);
            BattleAbility[] abilities = new BattleAbility[4];
            for (int i = 0; i < 4; i++)
            {
                abilities[i] = BattleAbilityFactory.GetBattleAbility(minionData.AbilitiesData[i], equipments, battleStatus);
            }
            
            Minion minion = new Minion(minionData, abilities, level);
            return minion;
        }
    }
}