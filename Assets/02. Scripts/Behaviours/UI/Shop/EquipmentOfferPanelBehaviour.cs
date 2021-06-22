using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class EquipmentOfferPanelBehaviour : ShopOfferUIBehaviour
    {
        [SerializeField]
        private Text hardCurrencyTextReference = default;
        [SerializeField]
        private Text equipmentNameReference = default;
        [SerializeField]
        private Text abilityTextReference = default;
        [SerializeField]
        private Image equipmentSprite = default;

        public override void SetupOffer(ShopOffer offer)
        {
            SetCost(offer.Cost);
            equipmentNameReference.text = Texts.GetText(offer.Reward.equip.Name);
            abilityTextReference.text = offer.Reward.equip.GetEquipmentEffectDescription();
            equipmentSprite.sprite = offer.Reward.equip.Data.SpriteReference;
        }

        public override void SetCost(int cost)
        {
            if (cost > 0)
            {
                hardCurrencyTextReference.enabled = true;
                hardCurrencyTextReference.text = Texts.GetText("EQUIPMENT_PANEL_001", cost);
            }
            else
            {
                hardCurrencyTextReference.enabled = false;
            }
        }
    }
}