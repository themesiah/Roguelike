using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public interface IShopOfferUI
    {
        void SetupOffer(ShopOffer offer);
        void SetOfferKey(Sprite offerKey);
        void SetPanelColor(Color color);
    }
}