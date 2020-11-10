using Laresistance.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Simulator
{
    [CreateAssetMenu(menuName = "Laresistance/Simulator/Equipments override list")]
    public class EquipmentOverrideList : ScriptableObject
    {
        [SerializeField]
        private List<EquipmentData> equipments = default;
        public List<EquipmentData> Equipments { get { return equipments; } }
    }
}