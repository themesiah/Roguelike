using Laresistance.Battle;
using System.Collections;
using Laresistance.Data;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Text;

namespace Laresistance.Core
{
    public class Minion : ISlot
    {
        private static int MAX_MINION_LEVEL = 10;
        public string Name { get { return Texts.GetText(Data.NameRef); }}
        public MinionData Data { get; private set; }
        private BattleAbility[] abilities = default;
        public int Level { get; private set; }

        public Minion(MinionData data, BattleAbility[] abilities, int level)
        {
            Data = data;
            if (abilities == null)
                throw new System.Exception("A minion should have an ability");
            this.abilities = abilities;
            if (level <= 0 || level > MAX_MINION_LEVEL)
                throw new System.Exception("A minion level must be at least 1 and " + MAX_MINION_LEVEL + " at max");
            Level = level;
        }

        public void SetEquipmentEvents(EquipmentEvents equipmentEvents)
        {
            foreach(var ability in abilities)
            {
                ability.SetEquipmentEvents(equipmentEvents);
            }
        }

        public void SetStatusManager(BattleStatusManager selfStatus)
        {
            foreach (var ability in abilities)
            {
                ability.SetStatusManager(selfStatus);
            }
        }

        public int GetEffectPower(int index)
        {
            return 0;
            //return ability.GetEffectPower(index, Level);
        }

        public bool SetInSlot(Player player)
        {
            return player.EquipMinion(this);
        }

        public IEnumerator ExecuteAbility(int skillIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies, IBattleAnimator animator, ScriptableIntReference bloodRef)
        {
            yield return abilities[skillIndex].ExecuteAbility(allies, enemies, Level, animator, bloodRef);
        }

        public BattleAbility[] Abilities => abilities;
        public string GetAbilityText()
        {
            //return Abilities[0].GetAbilityText(Level);
            StringBuilder builder = new StringBuilder();
            foreach (var ability in Abilities)
            {
                builder.Append("- ");
                builder.Append(ability.GetShortAbilityText(Level));
                builder.Append("\n");
            }
            return builder.ToString();
        }
        public string GetNextLevelAbilityText()
        {
            //return Abilities[0].GetAbilityText(Level+1);
            StringBuilder builder = new StringBuilder();
            foreach (var ability in Abilities)
            {
                builder.Append("- ");
                builder.Append(ability.GetShortAbilityText(Level+1));
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public int GetUpgradeCost()
        {
            //return Data.BaseBloodPrice + Data.BaseBloodPrice / 10 * (Level-1);
            return Data.BaseBloodPrice * Level;
        }

        public bool Upgrade()
        {
            if (CanUpgrade())
            {
                Level++;
                return true;
            }
            return false;
        }

        public bool CanUpgrade()
        {
            return Level < MAX_MINION_LEVEL;
        }
    } 
}