using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Laresistance.LevelGeneration
{
    [CustomEditor(typeof(SnapPoints))] [CanEditMultipleObjects]
    public class SnapPointsCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //SnapPoints sp = (SnapPoints)target;
            //if (GUILayout.Button("Recenter"))
            //{
            //    sp.Recenter();
            //}

            var pointTargets = targets;
            if (GUILayout.Button("Recenter"))
            {
                foreach (var sp in pointTargets)
                {

                    ((SnapPoints)sp).Recenter();
                }
            }
        }
    }
}