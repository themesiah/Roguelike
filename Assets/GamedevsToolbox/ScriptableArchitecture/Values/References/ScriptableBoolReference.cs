using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableBoolReference : ScriptableReference<bool, ScriptableBoolValue>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableBoolReference), true)]
    public class ScriptableBoolReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            position.position += Vector2.right * 5;
            bool value = property.FindPropertyRelative("constantValue").boolValue;
            value = EditorGUI.Toggle(position, value);
            property.FindPropertyRelative("constantValue").boolValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableBoolValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableBoolValue;
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