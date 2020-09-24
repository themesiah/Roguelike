using UnityEngine;
using UnityEditor;

namespace GamedevsToolbox.ScriptableArchitecture.Pools
{
    [CustomPropertyDrawer(typeof(ScriptablePool))]
    public class ScriptablePoolDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("data"), label);
            EditorGUI.EndProperty();
        }
    }
}