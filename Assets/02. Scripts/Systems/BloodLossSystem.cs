using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Systems
{
    public class BloodLossSystem
    {
        private ScriptableIntReference bloodRef;

        private EquipmentsContainer equipments;
        private float timer = 0f;

        public delegate void OnBloodLostHandler(int bloodLost);
        public OnBloodLostHandler OnBloodLost;

        public BloodLossSystem(EquipmentsContainer equipments, ScriptableIntReference bloodRef)
        {
            this.equipments = equipments;
            this.bloodRef = bloodRef;
        }

        public void Tick(float delta)
        {
            timer += delta;
            if (timer >= 1f)
            {
                timer -= 1f;
                int bloodLost = 0;
                bloodLost = equipments.ModifyValue(Equipments.EquipmentSituation.BloodLoss, bloodLost);
                if (bloodLost > 0)
                {
                    OnBloodLost?.Invoke(bloodLost);
                    bloodRef.SetValue(System.Math.Max(bloodRef.GetValue() - bloodLost, 0));
                }
            }
        }
    }
}