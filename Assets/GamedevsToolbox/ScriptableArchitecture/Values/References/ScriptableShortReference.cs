using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableShortReference : ScriptableReference<short, ScriptableShortValue>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableShortReference), true)]
    public class ScriptableShortReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            short value = (short)property.FindPropertyRelative("constantValue").intValue;
            value = (short)EditorGUI.IntField(position, value);
            property.FindPropertyRelative("constantValue").intValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableShortValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableShortValue;
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