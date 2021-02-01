using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Scriptable Int Add")]
    public class ScriptableIntAddEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private int value = default;

        public override ScriptableIntReference ModificationStrategy(ScriptableIntReference reference)
        {
            if (reference != null)
            {
                reference.SetValue(reference.GetValue() + value);
            }
            return reference;
        }
    }
}