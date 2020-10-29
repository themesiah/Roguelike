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
            if (offerKey == null)
            {
                keyImageReference.enabled = false;
            }
            else
            {
                keyImageReference.enabled = true;
                keyImageReference.sprite = offerKey;
            }
        }

        public void SetupOffer(ShopOffer offer)
        {
            if (offer.Cost > 0)
            {
                bloodTextReference.enabled = true;
                bloodTextReference.text = Texts.GetText("CONSUMABLE_PANEL_001", offer.Cost);
            } else
            {
                bloodTextReference.enabled = false;
            }
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