using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using DigitalRuby.Tween;

namespace Laresistance.Behaviours
{
    public class CameraBattleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Camera gameCamera = default;
        [SerializeField]
        private Camera[] additionalCameras = default;
        [SerializeField]
        private ScriptableFloatReference targetSizeReference = default;
        [SerializeField]
        private ScriptableVector3Reference positionOffsetReference = default;
        [SerializeField]
        private CameraMovementBehaviour cameraMovementBehaviour = default;
        [SerializeField]
        private ScriptableFloatReference movementDurationReference = default;
        [SerializeField]
        private ScriptableFloatReference resizeDurationReference = default;

        private float originalCamSize;

        private void Awake()
        {
            originalCamSize = gameCamera.orthographicSize;
        }

        public void CenterOnBattle(Vector3 battleCenter)
        {
            cameraMovementBehaviour.enabled = false;
            Vector3 pos = battleCenter + positionOffsetReference.GetValue();
            pos.z = -12.5f;

            TweenFactory.Tween("BattleCameraMovement", transform.position, pos, movementDurationReference.GetValue(), TweenScaleFunctions.Linear, UpdateCameraPosition);
            TweenFactory.Tween("BattleCameraSize", gameCamera.orthographicSize, targetSizeReference.GetValue(), resizeDurationReference.GetValue(), TweenScaleFunctions.Linear, UpdateCameraSize);
        }

        public void EndBattleCentering()
        {
            cameraMovementBehaviour.enabled = true;
            TweenFactory.Tween("BattleCameraSize", gameCamera.orthographicSize, originalCamSize, resizeDurationReference.GetValue(), TweenScaleFunctions.Linear, UpdateCameraSize);
        }

        private void UpdateCameraPosition(ITween<Vector3> t)
        {
            transform.localPosition = t.CurrentValue;
        }

        private void UpdateCameraSize(ITween<float> t)
        {
            gameCamera.orthographicSize = t.CurrentValue;
            foreach(Camera c in additionalCameras)
            {
                c.orthographicSize = t.CurrentValue;
            }
        }
    }
}