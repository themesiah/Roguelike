using GamedevsToolbox.ScriptableArchitecture.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class PoolInitializerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ScriptablePool[] poolList = default;
        [SerializeField]
        private string[] poolNames = default;

        private static Dictionary<string, ScriptablePool> poolDictionary;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.AreEqual(poolList.Length, poolNames.Length);
            poolDictionary = new Dictionary<string, ScriptablePool>();
            for (int i = 0; i < poolList.Length; ++i)
            {
                UnityEngine.Assertions.Assert.IsFalse(poolDictionary.ContainsKey(poolNames[i]));
                poolDictionary.Add(poolNames[i], poolList[i]);
                poolList[i].InitPool();
            }
        }

        public static ScriptablePool GetPool(string name)
        {
            ScriptablePool pool = null;
            poolDictionary.TryGetValue(name, out pool);
            return pool;
        }

        private void OnDestroy()
        {
            foreach(ScriptablePool pool in poolList)
            {
                pool.FreeAll();
            }
            poolDictionary = new Dictionary<string, ScriptablePool>();
        }
    }
}