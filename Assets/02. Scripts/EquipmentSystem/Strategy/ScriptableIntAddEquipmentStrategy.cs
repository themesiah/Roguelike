using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Scriptable Int Add")]
    public class ScriptableIntAddEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private int value = default;
        [SerializeField]
        private bool cantBeZero = false;
        [SerializeField]
        private bool cantBeNegative = false;

        public override ScriptableIntReference ModificationStrategy(ScriptableIntReference reference)
        {
            if (reference != null)
            {
                int val = reference.GetValue() + value;
                if (cantBeZero)
                {
                    val = System.Math.Max(1, val);
                }
                else if (cantBeNegative)
                {
                    val = System.Math.Max(0, val);
                }
                reference.SetValue(val);
            }
            return reference;
        }
    }
}