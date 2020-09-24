using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Selectors
{
    public class ScriptableArraySelector<T> : ScriptableObject
    {
        [SerializeField]
        private T[] elements = default;

        [SerializeField]
        [Tooltip("This integer will be the index that the Get function will use to return a element.")]
        private Values.ScriptableIntReference indexForSelection = default;

        public T Get()
        {
            int index = indexForSelection.GetValue();
            if (index >= elements.Length || index < 0)
            {
                index = 0;
                Debug.LogWarning("ScriptableArraySelect " + name + " tried to use an incorrect index and defaulted to 0");
            }
            if (elements.Length == 0)
            {
                Debug.LogWarning("ScriptableArraySelect " + name + " does not have any element and will return a default value");
                return default;
            }
            return elements[index];
        }
    }
}