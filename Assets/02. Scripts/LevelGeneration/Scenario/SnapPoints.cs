#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Laresistance.LevelGeneration
{
    [ExecuteInEditMode]
    public class SnapPoints : MonoBehaviour
    {
        [SerializeField]
        private Transform[] snapPoints = default;

        private Vector3 lastPosition;

        public Transform[] Points => snapPoints;

        public SnapPoints[] SceneSnapPoints { get
            {
                return FindObjectsOfType<SnapPoints>();
            } }

        private void Awake()
        {
            lastPosition = transform.position;
        }

        private void Update()
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
                            foreach (SnapPoints sp in SceneSnapPoints)
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
    }
}
#endif