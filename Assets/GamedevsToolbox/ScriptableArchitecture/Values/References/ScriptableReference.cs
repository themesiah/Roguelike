using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableReference<T, T2> where T : System.IEquatable<T> where T2 : ScriptableValue<T>
    {
        [SerializeField]
        private bool useConstant = true;

        [SerializeField]
        private T2 value = default;

        [SerializeField]
        private T constantValue = default;

        public virtual T GetValue()
        {
            if (useConstant)
            {
                return constantValue;
            }
            else
            {
                return value.GetValue();
            }
        }

        public virtual void SetValue(T value) {
            if (!useConstant)
            {
                this.value.SetValue(value);
            }
        }

        public void RegisterOnChangeAction(UnityAction<T> action)
        {
            value?.RegisterOnChangeAction(action);
        }

        public void UnregisterOnChangeAction(UnityAction<T> action)
        {
            value?.UnregisterOnChangeAction(action);
        }

#if UNITY_EDITOR
        public void SetScriptableValueManually(T2 val)
        {
            if (UnityEditor.EditorApplication.isPlaying)
                Debug.LogError("If you are seeing this, someone used the SetScriptableValueManually outside an editor function. This will cause a build error and should be removed. If you are not in play mode, ignore this message.");
            value = val;
            useConstant = false;
        }
#endif
    }

#if UNITY_EDITOR
    public abstract class ScriptableReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            bool useConstant = property.FindPropertyRelative("useConstant").boolValue;

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect rect = new Rect(position.position, Vector2.one * 20f);
            
            if (EditorGUI.DropdownButton(rect, new GUIContent("O"), FocusType.Keyboard, new GUIStyle() { fixedWidth = 50f, border = new RectOffset(1, 1, 1, 1) }))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Constant"), useConstant, () => SetProperty(property, true));
                menu.AddItem(new GUIContent("Value"), !useConstant, () => SetProperty(property, false));
                menu.ShowAsContext();
            }

            position.position += Vector2.right * 15f;

            if (useConstant)
            {
                ChangeConstantValue(position, property);
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

        protected abstract void ChangeConstantValue(Rect position, SerializedProperty property);

        protected virtual string GetValue(SerializedProperty property)
        {
            return "Not implemented";
        }

        private void SetProperty(SerializedProperty property, bool v)
        {
            var propRelative = property.FindPropertyRelative("useConstant");
            propRelative.boolValue = v;
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool useConstant = property.FindPropertyRelative("useConstant").boolValue;

            if (useConstant)
            {
                return 18f;
            } else
            {
                return 36f;
            }
        }
    }
#endif
}