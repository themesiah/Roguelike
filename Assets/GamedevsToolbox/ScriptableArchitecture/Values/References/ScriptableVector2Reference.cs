using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableVector2Reference : ScriptableReference<Vector2, ScriptableVector2Value>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableVector2Reference), true)]
    public class ScriptableVector2ReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            position.position += Vector2.right * 5;
            Vector2 value = property.FindPropertyRelative("constantValue").vector2Value;
            float[] values = new float[2] { value.x, value.y };
            EditorGUI.MultiFloatField(position, new GUIContent[] { new GUIContent("X"), new GUIContent("Y") }, values);
            value.x = values[0];
            value.y = values[1];
            property.FindPropertyRelative("constantValue").vector2Value = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableVector2Value val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableVector2Value;
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