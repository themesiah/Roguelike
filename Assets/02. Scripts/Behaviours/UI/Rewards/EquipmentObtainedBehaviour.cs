using Laresistance.Core;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class EquipmentObtainedBehaviour : RewardPanelBehaviour
    {
        [Header("New equip panel")]
        [SerializeField]
        private Text newEquipNameText = default;
        [SerializeField]
        private Text newEquipDescriptionText = default;
        [SerializeField]
        private Text newEquipSlotText = default;
        [SerializeField]
        private Image newEquipImage = default;
        [Header("Current equip panel")]
        [SerializeField]
        private Text currentEquipNameText = default;
        [SerializeField]
        private Text currentEquipDescriptionText = default;
        [SerializeField]
        private Text currentEquipSlotText = default;
        [SerializeField]
        private Image currentEquipImage = default;
        [Header("No current equip panel")]
        [SerializeField]
        private Text noEquipSlotText = default;
        [Header("General")]
        [SerializeField]
        private Image[] panels = default;
        [SerializeField]
        private Color unselectedColor = default;
        [SerializeField]
        private Color selectedColor = default;
        [Header("Droped equipment")]
        [SerializeField]
        private GameObject mapEquipmentPrefab = default;
        [SerializeField]
        private float force = 10f;

        private int equipSelectedIndex = -2;

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            newEquipNameText.text = rewardData.equip.Name;
            newEquipDescriptionText.text = rewardData.equip.GetEquipmentEffectDescription();
            newEquipSlotText.text = rewardData.equip.SlotName;
            newEquipImage.sprite = rewardData.equip.Data.SpriteReference;
            noEquipSlotText.text = rewardData.equip.SlotName;

            foreach (Image panel in panels)
            {
                panel.color = unselectedColor;
            }

            // Show current equipment
            Equipment currentEquip = player.GetEquipments()[rewardData.equip.Slot];
            if (currentEquip == null)
            {
                panels[1].gameObject.SetActive(false);
                panels[2].gameObject.SetActive(true);
            } else
            {
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(false);
                currentEquipNameText.text = currentEquip.Name;
                currentEquipDescriptionText.text = currentEquip.GetEquipmentEffectDescription();
                currentEquipSlotText.text = currentEquip.SlotName;
                currentEquipImage.sprite = currentEquip.Data.SpriteReference;
            }

            yield return base.StartingTween(rewardData);
        }

        protected override IEnumerator ExecutePanelProcess(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            equipSelectedIndex = -2;
            Equipment dropedEquipment = null;
            while (equipSelectedIndex < -1)
            {
                yield return null;
            }
            if (equipSelectedIndex >= 0)
            {
                dropedEquipment = player.GetEquipments()[rewardData.equip.Slot];
                if (dropedEquipment != null)
                    player.UnequipEquipment(dropedEquipment);
                player.EquipEquipment(rewardData.equip);
                panels[equipSelectedIndex].color = selectedColor;
            }
            else
            {
                dropedEquipment = rewardData.equip;
                panels[1].color = selectedColor;
            }

            if (dropedEquipment != null)
            {
                GameObject go = Instantiate(mapEquipmentPrefab);
                go.transform.position = playerDataReference.Get().transform.position;
                go.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force, ForceMode2D.Impulse);
                go.GetComponent<MapEquipment>().SetData(dropedEquipment.Data);
            }
        }

        public void NewEquipSelected(InputAction.CallbackContext context)
        {
            equipSelectedIndex = 0;
        }

        public void CurrentEquipSelected(InputAction.CallbackContext context)
        {
            equipSelectedIndex = -1;
        }

        public override RewardUIType RewardType => RewardUIType.Equip;
    }
}