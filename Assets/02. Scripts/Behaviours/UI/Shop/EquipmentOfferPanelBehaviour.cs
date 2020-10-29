using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class EquipmentOfferPanelBehaviour : MonoBehaviour, IShopOfferUI
    {
        [SerializeField]
        private Image panelImage = default;
        [SerializeField]
        private Text hardCurrencyTextReference = default;
        [SerializeField]
        private Text equipmentNameReference = default;
        [SerializeField]
        private Text abilityTextReference = default;
        [SerializeField]
        private Text slotTextReference = default;
        [SerializeField]
        private Image keyImageReference = default;
        [SerializeField]
        private Image equipmentSprite = default;

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
                hardCurrencyTextReference.enabled = true;
                hardCurrencyTextReference.text = Texts.GetText("EQUIPMENT_PANEL_001", offer.Cost);
            } else
            {
                hardCurrencyTextReference.enabled = false;
            }
            equipmentNameReference.text = Texts.GetText(offer.Reward.equip.Name);
            abilityTextReference.text = offer.Reward.equip.GetEquipmentEffectDescription();
            slotTextReference.text = offer.Reward.equip.SlotName;
            equipmentSprite.sprite = offer.Reward.equip.Data.SpriteReference;
        }

        public void SetPanelColor(Color color)
        {
            panelImage.color = color;
        }
    }
}