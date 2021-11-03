using UnityEngine;
using UnityEditor;

namespace Laresistance.Behaviours
{
    [CustomEditor(typeof(BreakableUrn))]
    public class BreakableUrnCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Break Urn"))
                {
                    BreakableUrn bu = (BreakableUrn)target;
                    bu.Break();
                }
            }
        }
    }
}