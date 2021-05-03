using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Laresistance.LevelGeneration
{
    [ExecuteInEditMode] [SelectionBase]
    public class SnapPoints : MonoBehaviour
    {
        [SerializeField]
        private Transform[] snapPoints = default;

        private Vector3 lastPosition;

        public Transform[] Points => snapPoints;

#if UNITY_EDITOR
        public SnapPoints[] SceneSnapPoints()
        {
            return FindObjectsOfType<SnapPoints>(true);
        }

        private void Awake()
        {
            lastPosition = transform.position;
        }

        private void Update()
        {
            Snap();
        }

        private void Snap()
        {
            if (UnityEngine.InputSystem.Keyboard.current.leftCtrlKey.isPressed)
            {
                if (Selection.gameObjects.Length > 0)
                {
                    if (Selection.gameObjects[0] == gameObject)
                    {
                        if (Vector2.Distance(lastPosition, transform.position) > 0.01f)
                        {
                            lastPosition = transform.position;
                            var sceneSnapPoints = SceneSnapPoints();
                            foreach (SnapPoints sp in sceneSnapPoints)
                            {
                                if (sp != null && sp != this)
                                {
                                    foreach (var thisPoint in Points)
                                    {
                                        foreach (var otherPoint in sp.Points)
                                        {
                                            if (Vector2.Distance(thisPoint.position, otherPoint.position) <= 0.3f)
                                            {
                                                var local = thisPoint.localPosition;
                                                local.x *= Mathf.Sign(thisPoint.lossyScale.x);
                                                transform.position = otherPoint.position - local;
                                                Selection.objects = null;
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Recenter()
        {
            Vector3 pos = SceneView.lastActiveSceneView.camera.transform.position;
            pos.z = 0f;
            gameObject.transform.position = pos;
            Debug.Log("Changing pos");
        }
#endif
    }
}