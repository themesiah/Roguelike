using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Systems
{
    public class BloodLossSystem
    {
        private ScriptableIntReference bloodRef;

        private EquipmentEvents equipmentEvents;
        private float timer = 0f;

        public delegate void OnBloodLostHandler(int bloodLost);
        public OnBloodLostHandler OnBloodLost;

        public BloodLossSystem(EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef)
        {
            this.equipmentEvents = equipmentEvents;
            this.bloodRef = bloodRef;
        }

        public void Tick(float delta)
        {
            timer += delta;
            if (timer >= 1f)
            {
                timer -= 1f;
                int bloodLost = 0;
                equipmentEvents?.OnBloodLossPerSecond?.Invoke(ref bloodLost);
                if (bloodLost > 0)
                {
                    OnBloodLost?.Invoke(bloodLost);
                    bloodRef.SetValue(bloodRef.GetValue() - bloodLost);
                }
            }
        }
    }
}