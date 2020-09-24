using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableVector3Reference : ScriptableReference<Vector3, ScriptableVector3Value> { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableVector3Reference), true)]
    public class ScriptableVector3ReferenceDrawer : ScriptableReferenceDrawer
    {
        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            position.position += Vector2.right * 5;
            Vector3 value = property.FindPropertyRelative("constantValue").vector3Value;
            float[] values = new float[3] { value.x, value.y, value.z };
            EditorGUI.MultiFloatField(position, new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z") }, values);
            value.x = values[0];
            value.y = values[1];
            value.z = values[2];
            property.FindPropertyRelative("constantValue").vector3Value = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableVector3Value val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableVector3Value;
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