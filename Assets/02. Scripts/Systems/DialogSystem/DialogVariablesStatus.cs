using UnityEngine;
using System.Collections.Generic;

namespace Laresistance.Systems.Dialog
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Dialog variables data")]
    [System.Serializable]
    public class DialogVariablesStatus : ScriptableObject
    {
        [System.Serializable]
        public class VariableData
        {
            public string name;
            public int value;
        }
        [SerializeField]
        private List<VariableData> variablesData = default;

        private int GetIndexOf(string name)
        {
            for(int i = 0; i < variablesData.Count; ++i)
            {
                var variableData = variablesData[i];
                if (variableData.name == name)
                    return i;
            }
            return -1;
        }

        public void SetVariable(string name, int value)
        {
            int index = GetIndexOf(name);
            if (index == -1)
            {
                variablesData.Add(new VariableData() { name = name, value = value});
            } else
            {
                variablesData[index].value = value;
            }
        }

        public int GetVariable(string name)
        {
            int index = GetIndexOf(name);
            if (index != -1)
            {
                return variablesData[index].value;
            }
            else
            {
                return 0;
            }
        }

        public void ResetVariable(string name)
        {
            int index = GetIndexOf(name);
            if (index != -1)
            {
                variablesData.RemoveAt(index);
            }
        }

        public void ResetAll()
        {
            variablesData.Clear();
        }

        public VariableData[] GetAll()
        {
            return variablesData.ToArray();
        }

        public void SetAll(VariableData[] data)
        {
            variablesData = new List<VariableData>(data);
        }
    }
}