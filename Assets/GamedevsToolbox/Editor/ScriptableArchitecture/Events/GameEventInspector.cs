using UnityEngine;
using UnityEditor;


namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            GameEvent myTarget = (GameEvent)target;

            DrawDefaultInspector();
            if (GUILayout.Button("Raise event"))
            {
                if (EditorApplication.isPlaying)
                {
                    myTarget.Raise();
                }
            }
        }
    }
}