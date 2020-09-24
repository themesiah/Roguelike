namespace Laresistance.Core
{
    public class Consumable : ISlot
    {
        public virtual void Use()
        {

        }

        public bool SetInSlot(Player player)
        {
            return player.AddConsumable(this);
        }
    } 
}