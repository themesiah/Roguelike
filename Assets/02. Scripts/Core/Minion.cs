using Laresistance.Battle;
using System.Collections;
using Laresistance.Data;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;

namespace Laresistance.Core
{
    public class Minion : ISlot
    {
        private static int MAX_MINION_LEVEL = 10;
        public string Name { get { return Texts.GetText(Data.name); }}
        public MinionData Data { get; private set; }
        private BattleAbility ability = default;
        public int Level { get; private set; }

        public Minion(MinionData data, BattleAbility ability, int level)
        {
            Data = data;
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

        public IEnumerator ExecuteAbility(BattleStatusManager[] allies, BattleStatusManager[] enemies, AnimatorWrapperBehaviour animator)
        {
            yield return ability.ExecuteAbility(allies, enemies, Level, animator);
        }

        public BattleAbility[] Abilities => new BattleAbility[] { ability };
        public string GetAbilityText()
        {
            return Abilities[0].GetAbilityText(Level);
        }
    } 
}