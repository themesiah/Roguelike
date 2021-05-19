using UnityEngine;
using UnityEditor;

namespace Laresistance.LevelGeneration
{
    [ExecuteInEditMode] [SelectionBase]
    public class SnapPoints : MonoBehaviour
    {
        public Transform[] snapPoints = default;

        private Vector3 lastPosition;

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
                                    if (snapPoints != null)
                                    {
                                        foreach (var thisPoint in snapPoints)
                                        {
                                            if (sp.snapPoints != null)
                                            {
                                                foreach (var otherPoint in sp.snapPoints)
                                                {
                                                    if (thisPoint != null)
                                                    {
                                                        if (otherPoint != null)
                                                        {
                                                            if (Vector2.Distance(thisPoint.position, otherPoint.position) <= 0.3f)
                                                            {
                                                                var local = thisPoint.localPosition;
                                                                local.x *= Mathf.Sign(thisPoint.lossyScale.x);
                                                                transform.position = otherPoint.position - local;
                                                                Selection.objects = null;
                                                                return;
                                                            }
                                                        } else
                                                        {
                                                            Debug.LogWarningFormat("A point of the candidate object {0} to snap is missing or null.", sp.gameObject.name);
                                                        }
                                                    } else
                                                    {
                                                        Debug.LogError("A point of the object you are trying to move is missing or null.");
                                                    }
                                                }
                                            } else
                                            {
                                                Debug.LogWarningFormat("The snap points of the candidate object {0} to snap are missing.", sp.gameObject.name);
                                            }
                                        }
                                    } else
                                    {
                                        Debug.LogError("The snap points of the object you are tryng to move are missing");
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