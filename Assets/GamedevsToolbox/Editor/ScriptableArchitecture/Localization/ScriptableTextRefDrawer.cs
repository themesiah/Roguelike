using UnityEngine;
using UnityEditor;

namespace GamedevsToolbox.ScriptableArchitecture.Localization
{
    [CustomPropertyDrawer(typeof(ScriptableTextRef))]
    public class ScriptableTextRefDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            bool useConstantText = property.FindPropertyRelative("useConstantText").boolValue;
            bool useTextId = property.FindPropertyRelative("useTextId").boolValue;

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect rect = new Rect(position.position, Vector2.one * 20f);

            if (EditorGUI.DropdownButton(rect, new GUIContent("O"), FocusType.Keyboard, new GUIStyle() { fixedWidth = 50f, border = new RectOffset(1, 1, 1, 1) }))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Constant"), useConstantText, () => {
                    SetConstantProperty(property, true);
                    SetTextIdProperty(property, false);
                });
                menu.AddItem(new GUIContent("Text Id"), useTextId, () => {
                    SetConstantProperty(property, false);
                    SetTextIdProperty(property, true);
                });
                menu.AddItem(new GUIContent("Text reference"), !useConstantText && !useTextId, () => {
                    SetConstantProperty(property, false);
                    SetTextIdProperty(property, false);
                });
                menu.ShowAsContext();
            }

            position.position += Vector2.right * 15f;

            if (useConstantText)
            {
                ChangeConstantValue(position, property);
            }
            else if (useTextId)
            {
                position.width -= 15f;
                position.height /= 2f;
                ChangeTextId(position, property);
                position.position += Vector2.up * 18f;
                EditorGUI.LabelField(position, GetStringValue(property, true));
            }
            else
            {
                position.width -= 15f;
                position.height /= 2f;
                EditorGUI.ObjectField(position, property.FindPropertyRelative("textRef"), GUIContent.none);
                position.position += Vector2.up * 18f;
                EditorGUI.LabelField(position, GetStringValue(property, false));
            }
            EditorGUI.EndProperty();
        }

        private void SetConstantProperty(SerializedProperty property, bool v)
        {
            var propRelative = property.FindPropertyRelative("useConstantText");
            propRelative.boolValue = v;
            property.serializedObject.ApplyModifiedProperties();
        }

        private void SetTextIdProperty(SerializedProperty property, bool v)
        {
            var propRelative = property.FindPropertyRelative("useTextId");
            propRelative.boolValue = v;
            property.serializedObject.ApplyModifiedProperties();
        }

        private void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            string value = property.FindPropertyRelative("constantText").stringValue;
            value = EditorGUI.TextField(position, value);
            property.FindPropertyRelative("constantText").stringValue = value;
        }

        private void ChangeTextId(Rect position, SerializedProperty property)
        {
            string value = property.FindPropertyRelative("textId").stringValue;
            value = EditorGUI.TextField(position, value);
            property.FindPropertyRelative("textId").stringValue = value;
        }

        private string GetStringValue(SerializedProperty property, bool useTextId)
        {
            ScriptableLanguage lan = GetInstance();
            if (lan == null)
            {
                return "Language reference not found. Create a GameObject and attach the ScriptableLanguageLocator script, and then create a ScriptableLanguage and assign it to the locator.";
            }
            if (useTextId)
            {
                return lan.GetText(property.FindPropertyRelative("textId").stringValue);
            } else
            {
                SerializedProperty prop = property.FindPropertyRelative("textRef");
                ScriptableLocalizedText locText = prop.objectReferenceValue as ScriptableLocalizedText;
                return lan.GetText(locText);
            }
        }

        public static ScriptableLanguage GetInstance()
        {
            ScriptableLanguageLocator locator = Editor.FindObjectOfType<ScriptableLanguageLocator>();
            if (locator == null)
            {
                return null;
            }

            return locator.GetLanguageRef();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool useConstant = property.FindPropertyRelative("useConstantText").boolValue;

            if (useConstant)
            {
                return 18f;
            }
            else
            {
                return 36f;
            }
        }
    }
}