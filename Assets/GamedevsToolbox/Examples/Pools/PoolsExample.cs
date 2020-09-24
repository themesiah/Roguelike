using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class PoolsExample : MonoBehaviour
    {
        [SerializeField]
        private ScriptableArchitecture.Pools.ScriptablePool examplePool = default;

        [SerializeField]
        private KeyCode keyToSpawn = KeyCode.A;
        [SerializeField]
        private KeyCode keyToRemove = KeyCode.A;

        private List<GameObject> cubes = new List<GameObject>();

        private void Awake()
        {
            examplePool.InitPool();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(keyToSpawn))
            {
                cubes.Add(examplePool.GetInstance(transform, Random.insideUnitSphere * 5f, Quaternion.identity));
            }

            if (Input.GetKeyDown(keyToRemove) && cubes.Count > 0)
            {
                examplePool.FreeInstance(cubes[0]);
                cubes.RemoveAt(0);
            }
        }
    }
}