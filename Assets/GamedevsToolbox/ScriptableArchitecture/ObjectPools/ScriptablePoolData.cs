using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Pools
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Object pool")]
    public class ScriptablePoolData : ScriptableObject
    {
        public enum InstancesFullBehaviour
        {
            ReturnNull,
            ReturnFirstUsed,
            DynamicThenNull,
            DynamicThenFirstUsed,
            DynamicInfinite
        }

        [SerializeField]
        private GameObject prefabToInstantiate = default;

        [SerializeField]
        private Values.ScriptableIntReference numberOfInstances = default;

        [SerializeField]
        private InstancesFullBehaviour instancesFullBehaviour = InstancesFullBehaviour.ReturnNull;

        [SerializeField]
        [Tooltip("If the behaviour is Dynamic, it will create more instances whenever there are not free instances, up to a new limit")]
        private Values.ScriptableIntReference dynamicMaxInstances = default;

        [SerializeField]
        [Tooltip("If the behaviour is Dynamic, how many instances it creates every time there is need to")]
        private Values.ScriptableIntReference dynamicInstancePace = default;

        public int NumberOfInstances => numberOfInstances.GetValue();
        public InstancesFullBehaviour Behaviour => instancesFullBehaviour;
        public int DynamicMaxInstances => dynamicMaxInstances.GetValue();
        public int DynamicInstancePace => dynamicInstancePace.GetValue();
        public GameObject Prefab => prefabToInstantiate;
    }
}