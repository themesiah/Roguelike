using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class SinglesAndSetsExamples : MonoBehaviour
    {
        [SerializeField]
        private ScriptableArchitecture.Sets.RuntimeSingleGameObject gameObjectSingle = default;

        [SerializeField]
        private ScriptableArchitecture.Sets.RuntimeSetGameObject gameObjectSet = default;

        [SerializeField]
        private KeyCode singleToRed = KeyCode.A;

        [SerializeField]
        private KeyCode singleToBlue = KeyCode.A;

        [SerializeField]
        private KeyCode setToRed = KeyCode.A;

        [SerializeField]
        private KeyCode setToBlue = KeyCode.A;
        
        private void Update()
        {
            if (Input.GetKeyDown(singleToRed))
            {
                gameObjectSingle.Get().GetComponent<Renderer>().material.color = Color.red;
            }
            if (Input.GetKeyDown(singleToBlue))
            {
                gameObjectSingle.Get().GetComponent<Renderer>().material.color = Color.blue;
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