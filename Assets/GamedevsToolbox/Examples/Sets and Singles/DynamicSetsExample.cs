using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class DynamicSetsExample : MonoBehaviour
    {
        [SerializeField]
        private GameObject cubePrefab = default;

        [SerializeField]
        private ScriptableArchitecture.Sets.RuntimeSetGameObject gameObjectSet = default;

        [SerializeField]
        private KeyCode spawnCube = KeyCode.A;

        [SerializeField]
        private KeyCode setToRed = KeyCode.A;

        [SerializeField]
        private KeyCode setToBlue = KeyCode.A;



        void Update()
        {
            if (Input.GetKeyDown(spawnCube))
            {
                Instantiate(cubePrefab, Random.insideUnitSphere * 5f, Quaternion.identity);
            }
            if (Input.GetKeyDown(setToRed))
            {
                gameObjectSet.ForEach(go => go.GetComponent<Renderer>().material.color = Color.red);
            }
            if (Input.GetKeyDown(setToBlue))
            {
                gameObjectSet.ForEach(go => go.GetComponent<Renderer>().material.color = Color.blue);
            }
        }
    }
}