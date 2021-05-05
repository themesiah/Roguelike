#if UNITY_EDITOR && UNITY_STANDALONE
using UnityEngine;
using UnityEditor;
using Laresistance.Management;
using System.Collections.Generic;

namespace Laresistance
{
    [CustomPropertyDrawer(typeof(ServiceName))]
    public class ServiceNameDrawer : PropertyDrawer
    {
        private int selected = -1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ServiceName serviceName = attribute as ServiceName;

            if (property.propertyType == SerializedPropertyType.String)
            {
                var serviceLocator = FindServiceLocator();
                var names = GetListOfServiceNames(serviceLocator);
                if (selected == -1)
                {
                    selected = FindNameIndex(property.stringValue, names);
                }
                selected = EditorGUI.Popup(position, property.displayName, selected, names);
                property.stringValue = names[selected];
            } else
            {
                EditorGUI.LabelField(position, label.text, "Service Name must be string");
            }
        }

        public ScriptableServiceLocator FindServiceLocator()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableServiceLocator).Name);
            ScriptableServiceLocator[] a = new ScriptableServiceLocator[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<ScriptableServiceLocator>(path);
            }

            return a[0];
        }

        public string[] GetListOfServiceNames(ScriptableServiceLocator serviceLocator)
        {
            List<string> listOfNames = new List<string>();
            foreach(var so in serviceLocator.ScriptableObjectsArray)
            {
                listOfNames.Add(so.name);
            }
            return listOfNames.ToArray();
        }

        private int FindNameIndex(string name, string[] namesArray)
        {
            for(int i = 0; i < namesArray.Length; ++i)
            {
                if (namesArray[i] == name)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
#endif