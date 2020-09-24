using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class DictTest : MonoBehaviour
    {
        public GamedevsToolbox.Dictionary.SerializableDictionary<string, GamedevsToolbox.ScriptableArchitecture.Values.ScriptableIntReference> dictionary;

        private void Awake()
        {
            foreach(var values in dictionary)
            {
                Debug.LogFormat("{0}: {1}", values.Key, values.Value.GetValue());
            }
        }
    }
}