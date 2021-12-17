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
        public StatsType[] statsTypeList { get; private set; }
        public Player player { get; private set; }
		public bool showPanel {get; private set; }

        public RewardData(int bloodAmount, int hardCurrencyAmount, Minion minion, Consumable consumable, Equipment equip, MapAbilityData mapAbilityData, StatsType[] statsTypeList, Player player, bool showPanel = true)
        {
            this.bloodAmount = bloodAmount;
            this.hardCurrencyAmount = hardCurrencyAmount;
            this.minion = minion;
            this.consumable = consumable;
            this.equip = equip;
            this.mapAbilityData = mapAbilityData;
            this.player = player;
			this.showPanel = showPanel;
            this.statsTypeList = statsTypeList;
        }
    }
}