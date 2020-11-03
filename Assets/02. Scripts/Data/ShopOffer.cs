using Laresistance.Core;
using static Laresistance.Core.EquipmentEvents;

namespace Laresistance.Data
{
    public class ShopOffer
    {
        public int Cost { get; private set; }
        public int OriginalCost { get; private set; }
        public bool UseHardCurrency { get; private set; }
        public RewardData Reward { get; private set; }

        public ShopOffer(int cost, bool useHardCurrency, RewardData reward)
        {
            Cost = cost;
            OriginalCost = cost;
            UseHardCurrency = useHardCurrency;
            Reward = reward;
        }

        public void SetNewCost(OnShopPriceHandler onShopPriceHandler)
        {
            int temp = OriginalCost;
            onShopPriceHandler?.Invoke(ref temp);
            Cost = temp;
        }
    }
}