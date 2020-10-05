using Laresistance.Battle;

namespace Laresistance.Core
{
    public class Consumable : ISlot
    {
        public BattleAbility Ability { get; private set; }
        public Consumable()
        {
            Ability = null; // TODO
        }

        public virtual void Use()
        {

        }

        public bool SetInSlot(Player player)
        {
            return player.AddConsumable(this);
        }
    } 
}