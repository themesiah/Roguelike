using UnityEngine;
using UnityEditor;


namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CustomEditor(typeof(FloatGameEvent))]
    public class FloatGameEventInspector : Editor
    {
        private float signal;
        public override void OnInspectorGUI()
        {
            FloatGameEvent myTarget = (FloatGameEvent)target;

            DrawDefaultInspector();

            signal = EditorGUILayout.FloatField(signal);
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