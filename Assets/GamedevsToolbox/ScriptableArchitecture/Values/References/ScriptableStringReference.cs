using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableStringReference : ScriptableReference<string, ScriptableStringValue>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableStringReference), true)]
    public class ScriptableStringReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            string value = property.FindPropertyRelative("constantValue").stringValue;
            value = EditorGUI.TextField(position, value);
            property.FindPropertyRelative("constantValue").stringValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableStringValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableStringValue;
            if (val != null)
            {
                return val.GetValue().ToString();
            }
            else
            {
                return "";
            }
        }
    }
#endif
}