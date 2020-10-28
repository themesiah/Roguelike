using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using System.Collections.Generic;

namespace Laresistance.Systems
{
    public class ShopSystem
    {
        private ScriptableIntReference bloodReference = default;
        private ScriptableIntReference hardCurrencyReference = default;
        private List<ShopOffer> offers = default;

        public ShopSystem(ScriptableIntReference blood, ScriptableIntReference hardCurrency)
        {
            offers = new List<ShopOffer>();
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

        public List<ShopOffer> GetOffers()
        {
            return offers;
        }

        public RewardData BuyOffer(ShopOffer offer)
        {
            if (offers.Contains(offer))
            {
                ScriptableIntReference referenceToUse = offer.UseHardCurrency ? hardCurrencyReference : bloodReference;
                if (referenceToUse.GetValue() >= offer.Cost)
                {
                    RemoveOffer(offer);
                    referenceToUse.SetValue(referenceToUse.GetValue() - offer.Cost);
                    return offer.Reward;
                }
            }
            return null;
        }

        public RewardData BuyOffer(int offerIndex)
        {
            if (offers.Count >= offerIndex + 1)
            {
                return BuyOffer(offers[offerIndex]);
            }
            else
            {
                return null;
            }
        }
    }
}