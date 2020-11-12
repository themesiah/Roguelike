using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public interface IShopOfferUI
    {
        void SetupOffer(ShopOffer offer);
        void SetOfferKey(KeySetSelector offerKey);
        void SetPanelColor(Color color);
        void SetCost(int cost);
        void SetButtonAction(UnityAction action);
        void SelectButton();
    }
}