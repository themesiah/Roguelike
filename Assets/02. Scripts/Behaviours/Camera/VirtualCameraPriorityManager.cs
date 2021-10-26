using UnityEngine;
using Cinemachine;

namespace Laresistance.Behaviours
{
    public class VirtualCameraPriorityManager : MonoBehaviour
    {
        [SerializeField]
        private int basePriority = 100;
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera = default;

        private bool virtualCameraActive = false;

        public void ShowCamera()
        {
            virtualCamera.Priority = basePriority;
            virtualCameraActive = true;
        }

        public void HideCamera()
        {
            virtualCamera.Priority = 0;
            virtualCameraActive = false;
        }

        public void StartBattle()
        {
            if (virtualCameraActive)
            {
                virtualCamera.Priority = 0;
            }
        }

        public void EndBattle()
        {
            if (virtualCameraActive)
            {
                virtualCamera.Priority = basePriority;
            }
        }
    }
}