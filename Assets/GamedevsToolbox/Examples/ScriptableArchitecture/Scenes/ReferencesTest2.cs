using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace GamedevsToolbox.Examples
{
    public class ReferencesTest2 : MonoBehaviour
    {
        [SerializeField]
        public ScriptableFloatReference floatValue;
        [SerializeField]
        public ScriptableBoolReference boolValue;
        [SerializeField]
        public ScriptableDoubleReference doubleValue;
        [SerializeField]
        public ScriptableIntReference intValue;
        [SerializeField]
        public ScriptableLongReference longValue;
        [SerializeField]
        public ScriptableShortReference shortValue;
        [SerializeField]
        public ScriptableStringReference stringValue;
        [SerializeField]
        public ScriptableVector2Reference vec2Value;
        [SerializeField]
        public ScriptableVector3Reference vec3Value;

        private void Start()
        {
            Debug.Log(floatValue.GetValue().ToString());
            Debug.Log(boolValue.GetValue().ToString());
            Debug.Log(doubleValue.GetValue().ToString());
            Debug.Log(intValue.GetValue().ToString());
            Debug.Log(longValue.GetValue().ToString());
            Debug.Log(shortValue.GetValue().ToString());
            Debug.Log(stringValue.GetValue().ToString());
            Debug.Log(vec2Value.GetValue().ToString());
            Debug.Log(vec3Value.GetValue().ToString());
        }
    }
}