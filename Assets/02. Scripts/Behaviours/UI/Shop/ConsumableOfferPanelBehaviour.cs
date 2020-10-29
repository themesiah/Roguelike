using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class ConsumableOfferPanelBehaviour : ShopOfferUIBehaviour
    {
        [SerializeField]
        private Text bloodTextReference = default;
        [SerializeField]
        private Text consumableNameReference = default;
        [SerializeField]
        private Text abilityTextReference = default;
        [SerializeField]
        private Image consumableSprite = default;

        public override void SetupOffer(ShopOffer offer)
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
    }
}