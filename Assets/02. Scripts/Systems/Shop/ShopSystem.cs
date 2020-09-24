using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using System.Collections.Generic;

namespace Laresistance.Systems
{
    public class ShopSystem
    {
        private ScriptableIntReference bloodReference = default;
        private ScriptableIntReference hardCurrencyReference = default;
        private List<ShopOffer> offers = default;

        public ShopSystem()
        {
            offers = new List<ShopOffer>();
        }

        public void SetCurrencyReference(ScriptableIntReference blood, ScriptableIntReference hardCurrency)
        {
            bloodReference = blood;
            hardCurrencyReference = hardCurrency;
        }

        public void AddOffer(ShopOffer offer)
        {
            if (!offers.Contains(offer))
            {
                offers.Add(offer);
            }
        }

        public void RemoveOffer(ShopOffer offer)
        {
            if (offers.Contains(offer))
            {
                offers.Remove(offer);
            }
        }

        public ISlot BuyOffer(ShopOffer offer)
        {
            if (offers.Contains(offer))
            {
                ScriptableIntReference referenceToUse = offer.UseHardCurrency ? hardCurrencyReference : bloodReference;
                if (referenceToUse.GetValue() >= offer.Cost)
                {
                    RemoveOffer(offer);
                    referenceToUse.SetValue(referenceToUse.GetValue() - offer.Cost);
                    return offer.SlotableItem;
                }
            }
            return null;
        }
    }
}