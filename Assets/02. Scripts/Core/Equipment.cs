namespace Laresistance.Core
{
    public class Equipment : ISlot
    {
        private int slot = -1;
        public int Slot
        {
            get
            {
                return slot;
            }
        }

        public Equipment(int slot)
        {
            this.slot = slot;
        }

        public bool SetInSlot(Player player)
        {
            return player.EquipEquipment(this);
        }
    } 
}