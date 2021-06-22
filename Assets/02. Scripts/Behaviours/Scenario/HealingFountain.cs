using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class HealingFountain : MonoBehaviour
    {
        [SerializeField]
        private IntGameEvent healingEvent = default;
        [SerializeField]
        private int healingAmount = 75;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataReference = default;
        [SerializeField]
        private BloodObtain bloodObtain = default;

        public void Heal()
        {
            var equipments = playerDataReference.Get().player.GetEquipmentContainer();
            int finalHeal = equipments.ModifyValue(Equipments.EquipmentSituation.FountainHealing, healingAmount);
            if (finalHeal > 0)
                healingEvent?.Raise(finalHeal);

            int bloodObtained = 0;
            bloodObtained = equipments.ModifyValue(Equipments.EquipmentSituation.FountainBlood, bloodObtained);
            if (bloodObtained > 0)
            {
                bloodObtain.Obtain();
            } else
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }
}