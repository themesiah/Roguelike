using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Battle;
using Laresistance.Data;

namespace Laresistance.Core
{
    public class Consumable : ShowableElement, ISlot
    {
        public ConsumableData Data { get; private set; }
        public BattleAbility Ability{ get; private set;}
        public string Name { get { return Texts.GetText(Data.NameRef); } }
        public Consumable(ConsumableData consumableData, BattleAbility ability)
        {
            Data = consumableData;
            Ability = ability;
        }

        public bool SetInSlot(Player player)
        {
            return player.AddConsumable(this);
        }

        public string GetAbilityText()
        {
            return Ability.GetAbilityText(1);
        }
    } 
}