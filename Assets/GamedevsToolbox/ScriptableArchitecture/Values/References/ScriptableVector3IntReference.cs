using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableVector3IntReference : ScriptableReference<Vector3Int, ScriptableVector3IntValue>{}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableVector3IntReference), true)]
    public class ScriptableVector3IntReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            position.position += Vector2.right * 5;
            Vector3Int value = property.FindPropertyRelative("constantValue").vector3IntValue;
            int[] values = new int[3] { value.x, value.y, value.z };
            EditorGUI.MultiIntField(position, new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z") }, values);
            value.x = values[0];
            value.y = values[1];
            value.z = values[2];
            property.FindPropertyRelative("constantValue").vector3IntValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableVector3IntValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableVector3IntValue;
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