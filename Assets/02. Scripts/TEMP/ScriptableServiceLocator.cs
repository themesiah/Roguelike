using UnityEngine;
using System.Collections.Generic;

namespace Laresistance.Management
{
    [CreateAssetMenu(menuName = "Laresistance/Service Locator")]
    public class ScriptableServiceLocator : ScriptableObject
    {
        [System.Serializable]
        public struct KeyObjectPair
        {
            public string key;
            public ScriptableObject scriptableObject;
        }

        [SerializeField]
        private List<ScriptableObject> scriptableObjectsList = default;

        private Dictionary<string, ScriptableObject> scriptableObjectsMap;
        private bool initialized = false;

        public ScriptableObject[] ScriptableObjectsArray => scriptableObjectsList.ToArray();

        public void Init()
        {
            if (initialized)
                return;
            scriptableObjectsMap = new Dictionary<string, ScriptableObject>();
            foreach(var so in scriptableObjectsList)
            {
                if (scriptableObjectsMap.ContainsKey(so.name))
                {
                    Debug.LogErrorFormat("[ScriptableServiceLocator] Duplicated key {0}", so.name);
                } else {
                    scriptableObjectsMap.Add(so.name, so);
                }
            }
        }

        public T GetService<T>(string key) where T : ScriptableObject
        {
            Init();
            if (scriptableObjectsMap.ContainsKey(key))
            {
                return scriptableObjectsMap[key] as T;
            } else
            {
                Debug.LogErrorFormat("[ScriptableServiceLocator] Non existant key {0}", key);
                return null;
            }
        }
    }
}