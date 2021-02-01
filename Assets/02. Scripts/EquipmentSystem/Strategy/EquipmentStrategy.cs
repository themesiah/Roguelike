using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;

namespace Laresistance.Equipments
{
    public abstract class EquipmentStrategy : ScriptableObject
    {
        public virtual float ModificationStrategy(float originalValue) { return originalValue; }
        public virtual int ModificationStrategy(int originalValue) { return originalValue; }
        public virtual ScriptableIntReference ModificationStrategy(ScriptableIntReference originalValue) { return originalValue; }
        [SerializeField][Range(0,3)]
        private int priority = default;
        public int Priority { get { return priority; } }
    }
}