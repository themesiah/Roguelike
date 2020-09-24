using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace GamedevsToolbox.Utils
{
    public class LookToCamera : MonoBehaviour
    {
        [SerializeField]
        private RuntimeSingleCamera cameraRef = default;

        private void Update()
        {
            transform.LookAt(cameraRef.Get().transform);
        }
    }
}