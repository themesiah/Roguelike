using Laresistance.Core;

namespace Laresistance.Data
{
    public class RewardData
    {
        public int bloodAmount { get; private set; }
        public Minion minion { get; private set; }
        public Consumable consumable { get; private set; }

        public RewardData(int bloodAmount, Minion minion, Consumable consumable)
        {
            this.bloodAmount = bloodAmount;
            this.minion = minion;
            this.consumable = consumable;
        }
    }
}