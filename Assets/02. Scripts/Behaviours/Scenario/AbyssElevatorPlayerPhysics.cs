using GamedevsToolbox.ScriptableArchitecture.Sets;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class AbyssElevatorPlayerPhysics : MonoBehaviour
    {
        [SerializeField]
        private RuntimeSingleTransform playerTransformReference = default;

        private Character2DController playerController;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(playerTransformReference);
        }

        private void SetPlayerController()
        {
            if (playerController == null)
            {
                Transform t = playerTransformReference.Get();
                if (t != null)
                {
                    playerController = t.GetComponent<Character2DController>();
                }
            }
        }

        public void OnEnterElevator()
        {
            SetPlayerController();
            playerController.SetCanJump(false);
        }

        public void OnExitElevator()
        {
            SetPlayerController();
            playerController.SetCanJump(true);
        }
    }
}