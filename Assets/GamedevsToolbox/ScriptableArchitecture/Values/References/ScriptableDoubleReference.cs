using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableDoubleReference : ScriptableReference<double, ScriptableDoubleValue>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableDoubleReference), true)]
    public class ScriptableDoubleReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            double value = property.FindPropertyRelative("constantValue").doubleValue;
            value = EditorGUI.DoubleField(position, value);
            property.FindPropertyRelative("constantValue").doubleValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableDoubleValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableDoubleValue;
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