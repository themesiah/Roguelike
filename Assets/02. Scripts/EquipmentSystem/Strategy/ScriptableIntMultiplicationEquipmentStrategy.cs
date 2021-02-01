using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Scriptable Int Multiplication")]
    public class ScriptableIntMultiplicationEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private float value = default;

        public override ScriptableIntReference ModificationStrategy(ScriptableIntReference reference)
        {
            if (reference != null)
            {
                reference.SetValue((int)(reference.GetValue() * value));
            }
            return reference;
        }
    }
}