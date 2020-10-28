using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class ConsumableOfferPanelBehaviour : MonoBehaviour, IShopOfferUI
    {
        [SerializeField]
        private Image panelImage = default;
        [SerializeField]
        private Text bloodTextReference = default;
        [SerializeField]
        private Text consumableNameReference = default;
        [SerializeField]
        private Text abilityTextReference = default;
        [SerializeField]
        private Image keyImageReference = default;
        [SerializeField]
        private Image consumableSprite = default;

        public void SetOfferKey(Sprite offerKey)
        {
            keyImageReference.sprite = offerKey;
        }

        public void SetupOffer(ShopOffer offer)
        {
            bloodTextReference.text = Texts.GetText("CONSUMABLE_PANEL_001", offer.Reward.consumable.Data.BaseBloodPrice);
            consumableNameReference.text = Texts.GetText(offer.Reward.consumable.Name);
            abilityTextReference.text = offer.Reward.consumable.GetAbilityText();
            consumableSprite.sprite = offer.Reward.consumable.Data.SpriteReference;
        }

        public void SetPanelColor(Color color)
        {
            panelImage.color = color;
        }
    }
}