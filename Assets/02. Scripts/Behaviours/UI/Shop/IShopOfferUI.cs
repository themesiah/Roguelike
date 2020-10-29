using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public interface IShopOfferUI
    {
        void SetupOffer(ShopOffer offer);
        void SetOfferKey(KeySetSelector offerKey);
        void SetPanelColor(Color color);
    }
}