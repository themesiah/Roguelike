using GamedevsToolbox.ScriptableArchitecture.Sets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class CanvasCameraAssigner : MonoBehaviour
    {
        [SerializeField]
        private RuntimeSingleCamera cameraRef = default;
        [SerializeField]
        private Canvas canvasRef = default;

        private void Start()
        {
            canvasRef.worldCamera = cameraRef.Get();
        }
    }
}