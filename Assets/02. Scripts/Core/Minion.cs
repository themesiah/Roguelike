using Laresistance.Battle;
using System.Collections;

namespace Laresistance.Core
{
    public class Minion : ISlot
    {
        private static int MAX_MINION_LEVEL = 10;
        public string Name { get; private set; }
        private BattleAbility ability = default;
        public int Level { get; private set; }

        public Minion(string name, BattleAbility ability, int level)
        {
            Name = name;
            if (ability == null)
                throw new System.Exception("A minion should have an ability");
            this.ability = ability;
            if (level <= 0 || level > MAX_MINION_LEVEL)
                throw new System.Exception("A minion level must be at least 1 and " + MAX_MINION_LEVEL + " at max");
            Level = level;
        }

        public int GetEffectPower(int index)
        {
            return ability.GetEffectPower(index, Level);
        }

        public bool SetInSlot(Player player)
        {
            return player.EquipMinion(this);
        }

        public IEnumerator ExecuteAbility(BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            yield return ability.ExecuteAbility(allies, enemies, Level);
        }

        public BattleAbility[] Abilities => new BattleAbility[] { ability };
        public string GetAbilityText()
        {
            return Abilities[0].GetAbilityText(Level);
        }
    } 
}