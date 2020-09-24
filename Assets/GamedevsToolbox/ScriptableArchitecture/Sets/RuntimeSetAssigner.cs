using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Sets
{
    public class RuntimeSetAssigner<T, T2> : MonoBehaviour where T : RuntimeSet<T2> where T2 : new()
    {
        [SerializeField]
        private T runtimeSet = default;
        [SerializeField]
        private T2 component = default;

        protected void OnEnable()
        {
            runtimeSet.Add(component);
        }

        protected void OnDisable()
        {
            runtimeSet.Remove(component);
        }
    }
}