using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableLongReference : ScriptableReference<long, ScriptableLongValue>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableLongReference), true)]
    public class ScriptableLongReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            long value = property.FindPropertyRelative("constantValue").longValue;
            value = EditorGUI.LongField(position, value);
            property.FindPropertyRelative("constantValue").longValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableLongValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableLongValue;
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