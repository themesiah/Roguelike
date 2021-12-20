using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Core;
using Laresistance.Data;
using Laresistance.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class EquipmentObtainedBehaviour : RewardPanelBehaviour
    {
        [Header("General")]
        [SerializeField]
        private GameObject[] equipmentPanels = default;
        [Header("Current equipments")]
        [SerializeField]
        private Text[] currentEquipmentsName = default;
        [SerializeField]
        private Text[] currentEquipmentsDescription = default;
        [SerializeField]
        private Image[] currentEquipmentsImages = default;
        [SerializeField]
        private Text[] currentEquipmentsButtonPressNotice = default;
        [Header("New equip panel")]
        [SerializeField]
        private LocalizedStringTextBehaviour newEquipNameText = default;
        [SerializeField]
        private Text newEquipDescriptionText = default;
        [Header("Droped equipment")]
        [SerializeField]
        private GameObject mapEquipmentPrefab = default;
        [SerializeField]
        private float force = 10f;

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            newEquipNameText.ChangeVariable(rewardData.equip.Name);
            newEquipDescriptionText.text = rewardData.equip.GetEquipmentEffectDescription();

            foreach(var panel in equipmentPanels)
            {
                panel.SetActive(false);
            }

            var equipments = player.GetEquipments();
            for(int i = 0; i <= player.EquipmentMaxSlotAllowed; ++i)
            {
                equipmentPanels[i].SetActive(true);
                if (equipments[i] != null)
                {
                    currentEquipmentsName[i].text = equipments[i].Name;
                    currentEquipmentsDescription[i].text = equipments[i].GetEquipmentEffectDescription();
                    currentEquipmentsImages[i].sprite = equipments[i].Data.SpriteReference;
                    currentEquipmentsImages[i].enabled = true;
                    currentEquipmentsButtonPressNotice[i].text = Texts.GetText("REWARD_EQUIPMENT_002");
                } else
                {
                    currentEquipmentsName[i].text = "NO EQUIP";
                    currentEquipmentsDescription[i].text = "";
                    currentEquipmentsImages[i].sprite = null;
                    currentEquipmentsImages[i].enabled = false;
                    currentEquipmentsButtonPressNotice[i].text = Texts.GetText("REWARD_EQUIPMENT_002-2");
                }
            }

            yield return base.StartingTween(rewardData);
        }

        protected override IEnumerator ExecutePanelProcess(RewardData rewardData)
        {
            Player player = playerDataReference.Get().player;
            selectedOptionIndex = -2;
            Equipment dropedEquipment = null;
            while (selectedOptionIndex < -1)
            {
                yield return null;
            }
            if (selectedOptionIndex >= 0)
            {
                dropedEquipment = player.GetEquipments()[selectedOptionIndex];
                if (dropedEquipment != null)
                    player.UnequipEquipment(selectedOptionIndex);
                player.EquipEquipment(rewardData.equip, selectedOptionIndex);

                AnalyticsSystem.Instance.CustomEvent("EquipmentObtained", new Dictionary<string, object>() {
                    { "Obtained", true },
                    { "Overwrited", dropedEquipment != null },
                    { "EquipName", rewardData.equip.Data.name }
                });
            }
            else
            {
                dropedEquipment = rewardData.equip;
                AnalyticsSystem.Instance.CustomEvent("EquipmentObtained", new Dictionary<string, object>() {
                    { "Obtained", false },
                    { "Overwrited", false },
                    { "EquipName", rewardData.equip.Data.name }
                });
            }

            if (dropedEquipment != null)
            {
                //GameObject go = Instantiate(mapEquipmentPrefab);
                //go.transform.position = playerDataReference.Get().transform.position;
                //go.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force, ForceMode2D.Impulse);
                //go.GetComponent<MapEquipment>().SetData(dropedEquipment.Data);
            }
        }

        public override RewardUIType RewardType => RewardUIType.Equip;
    }
}