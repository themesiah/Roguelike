using Laresistance.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class ShowableEquipment : MonoBehaviour, IShowableGameElement
    {
        [SerializeField]
        private Text equipmentName = default;
        [SerializeField]
        private Text equipmentDescription = default;
        [SerializeField]
        private Image equipmentIcon = default;

        public void SetupShowableElement(ShowableElement showableElement)
        {
            if (showableElement == null)
            {
                if (equipmentName != null)
                {
                    equipmentName.text = "";
                }
                if (equipmentDescription != null)
                {
                    equipmentDescription.text = "";
                }
                if (equipmentIcon != null)
                {
                    equipmentIcon.enabled = false;
                }
            }
            else
            {
                Equipment equipment = (Equipment)showableElement;
                if (equipmentName != null)
                {
                    equipmentName.text = equipment.Name;
                }
                if (equipmentDescription != null)
                {
                    equipmentDescription.text = equipment.GetEquipmentEffectDescription();
                }
                if (equipmentIcon != null)
                {
                    equipmentIcon.enabled = true;
                    equipmentIcon.sprite = equipment.Data.SpriteReference;
                }
            }
        }
    }
}