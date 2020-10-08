using Laresistance.Core;

namespace Laresistance.Data
{
    public class ShopOffer
    {
        public int Cost { get; private set; }
        public bool UseHardCurrency { get; private set; }
        public ISlot SlotableItem { get; private set; }

        public ShopOffer(int cost, bool useHardCurrency, ISlot slotableItem)
        {
            Cost = cost;
            UseHardCurrency = useHardCurrency;
            SlotableItem = slotableItem;
        }
    }
}