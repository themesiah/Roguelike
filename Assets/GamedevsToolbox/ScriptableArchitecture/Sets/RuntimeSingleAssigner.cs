using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Sets
{
    public class RuntimeSingleAssigner<T, T2> : MonoBehaviour where T : RuntimeSingle<T2>
    {
        [SerializeField]
        private T runtimeSingle = default;
        [SerializeField]
        private T2 component = default;

        protected void OnEnable()
        {
            runtimeSingle.Set(component);
        }

        protected void OnDisable()
        {
            runtimeSingle.Set(default);
        }
    }
}