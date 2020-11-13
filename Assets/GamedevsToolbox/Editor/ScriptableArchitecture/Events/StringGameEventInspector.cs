using UnityEngine;
using UnityEditor;


namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CustomEditor(typeof(StringGameEvent))]
    public class StringGameEventInspector : Editor
    {
        private string signal;
        public override void OnInspectorGUI()
        {
            StringGameEvent myTarget = (StringGameEvent)target;

            DrawDefaultInspector();

            signal = GUILayout.TextField(signal);
            if (GUILayout.Button("Raise event"))
            {
                if (EditorApplication.isPlaying)
                {
                    myTarget.Raise(signal);
                }
            }
        }
    }
}