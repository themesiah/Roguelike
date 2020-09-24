using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableVector2IntReference : ScriptableReference<Vector2Int, ScriptableVector2IntValue>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableVector2IntReference), true)]
    public class ScriptableVector2IntReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            position.position += Vector2.right * 5;
            Vector2Int value = property.FindPropertyRelative("constantValue").vector2IntValue;
            int[] values = new int[2] { value.x, value.y };
            EditorGUI.MultiIntField(position, new GUIContent[] { new GUIContent("X"), new GUIContent("Y") }, values);
            value.x = values[0];
            value.y = values[1];
            property.FindPropertyRelative("constantValue").vector2IntValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableVector2IntValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableVector2IntValue;
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