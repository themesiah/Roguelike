using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class SelectorRandomPositionSelectObjectExample : MonoBehaviour
    {
        [SerializeField]
        private KeyCode spawnKey = KeyCode.S;

        [SerializeField]
        private ScriptableArchitecture.Selectors.ScriptableGameObjectSelector gameObjectSelector = default;

        [SerializeField]
        private ScriptableArchitecture.Selectors.ScriptableVector3Selector positionSelector = default;

        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableIntValue gameObjectSelectionValue = default;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gameObjectSelectionValue.SetValue(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gameObjectSelectionValue.SetValue(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                gameObjectSelectionValue.SetValue(2);
            }
            if (Input.GetKeyDown(spawnKey))
            {
                Instantiate(gameObjectSelector.Get(), positionSelector.Get().GetValue(), Quaternion.identity);
            }
        }
    }
}