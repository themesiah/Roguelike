using Laresistance.Core;

namespace Laresistance.Data
{
    public class RewardData
    {
        public int bloodAmount { get; private set; }
        public int hardCurrencyAmount { get; private set; }
        public Minion minion { get; private set; }
        public Consumable consumable { get; private set; }
        public Equipment equip { get; private set; }
        public MapAbilityData mapAbilityData { get; private set; }

        public RewardData(int bloodAmount, int hardCurrencyAmount, Minion minion, Consumable consumable, Equipment equip, MapAbilityData mapAbilityData)
        {
            this.bloodAmount = bloodAmount;
            this.hardCurrencyAmount = hardCurrencyAmount;
            this.minion = minion;
            this.consumable = consumable;
            this.equip = equip;
            this.mapAbilityData = mapAbilityData;
        }
    }
}