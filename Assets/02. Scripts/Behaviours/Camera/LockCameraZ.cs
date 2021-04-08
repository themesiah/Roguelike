using UnityEngine;
using Cinemachine;

namespace Laresistance.Behaviours
{
    public class LockCameraZ : CinemachineExtension
    {
        [SerializeField] [Tooltip("Lock the camera's Z position to this value")]
        private float zPosition = -10f;
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                pos.z = zPosition;
                state.RawPosition = pos;

            }
        }
    }
}