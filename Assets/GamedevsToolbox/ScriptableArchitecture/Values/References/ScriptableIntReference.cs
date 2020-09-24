using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableIntReference : ScriptableReference<int, ScriptableIntValue> {
        [SerializeField]
        private bool useRandom = false;

        [SerializeField]
        [Tooltip("Minimum inclusive and maximum exclusive")]
        private ScriptableVector2IntReference minMax = new ScriptableVector2IntReference();

        public override int GetValue()
        {
            if (useRandom)
            {
                return Random.Range(minMax.GetValue()[0], minMax.GetValue()[1]);
            }
            else
            {
                return base.GetValue();
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableIntReference), true)]
    public class ScriptableIntReferenceDrawer : ScriptableReferenceDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            bool useConstant = property.FindPropertyRelative("useConstant").boolValue;
            bool useRandom = property.FindPropertyRelative("useRandom").boolValue;

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect rect = new Rect(position.position, Vector2.one * 20f);

            if (EditorGUI.DropdownButton(rect, new GUIContent("O"), FocusType.Keyboard, new GUIStyle() { fixedWidth = 50f, border = new RectOffset(1, 1, 1, 1) }))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Constant"), useConstant, () => {
                    SetConstantProperty(property, true);
                    SetRandomProperty(property, false);
                });
                menu.AddItem(new GUIContent("Value"), !useConstant && !useRandom, () => {
                    SetConstantProperty(property, false);
                    SetRandomProperty(property, false);
                });
                menu.AddItem(new GUIContent("Random"), useRandom, () => {
                    SetConstantProperty(property, false);
                    SetRandomProperty(property, true);
                });
                menu.ShowAsContext();
            }

            position.position += Vector2.right * 15f;

            if (useConstant)
            {
                ChangeConstantValue(position, property);
            }
            else if (useRandom)
            {
                position.width -= 30f;
                position.position -= Vector2.right * 0f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("minMax"), new GUIContent(""));
            }
            else
            {
                position.width -= 15f;
                position.height /= 2f;
                EditorGUI.ObjectField(position, property.FindPropertyRelative("value"), GUIContent.none);
                position.position += Vector2.up * 18f;
                EditorGUI.LabelField(position, string.Format("Current value: {0}", GetValue(property)));
            }
            EditorGUI.EndProperty();
        }

        private void SetConstantProperty(SerializedProperty property, bool v)
        {
            var propRelative = property.FindPropertyRelative("useConstant");
            propRelative.boolValue = v;
            property.serializedObject.ApplyModifiedProperties();
        }

        private void SetRandomProperty(SerializedProperty property, bool v)
        {
            var propRelative = property.FindPropertyRelative("useRandom");
            propRelative.boolValue = v;
            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void ChangeConstantValue(Rect position, SerializedProperty property)
        {
            int value = property.FindPropertyRelative("constantValue").intValue;
            value = EditorGUI.IntField(position, value);
            property.FindPropertyRelative("constantValue").intValue = value;
        }

        protected override string GetValue(SerializedProperty property)
        {
            ScriptableIntValue val = property.FindPropertyRelative("value").objectReferenceValue as ScriptableIntValue;
            if (val != null)
            {
                return val.GetValue().ToString();
            } else
            {
                return "";
            }
        }
    }
#endif
}