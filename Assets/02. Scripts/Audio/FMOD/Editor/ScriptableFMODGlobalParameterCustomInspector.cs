#if UNITY_EDITOR && UNITY_STANDALONE
using UnityEngine;
using UnityEditor;

namespace Laresistance.Audio
{
    [CustomEditor(typeof(ScriptableFMODGlobalParameter))]
    public class ScriptableFMODGlobalParameterCustomInspector : Editor
    {
        private float parameterValue = 0f;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            parameterValue = EditorGUILayout.FloatField("Value", parameterValue);
            if (GUILayout.Button("Raise event"))
            {
                ScriptableFMODGlobalParameter parameter = (ScriptableFMODGlobalParameter)target;
                parameter.TriggerParameter(parameterValue);
            }
        }
    }
}
#endif