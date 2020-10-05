using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Movement Data")]
    public class PlayerMovementData : ScriptableObject
    {
        [SerializeField]
        private ScriptableFloatReference horizontalSpeed = default;
        public ScriptableFloatReference HorizontalSpeed { get { return horizontalSpeed; } }

        [SerializeField]
        private ScriptableFloatReference jumpForce = default;
        public ScriptableFloatReference JumpForce { get { return jumpForce; } }

        [SerializeField]
        private ScriptableIntReference jumpsLimit = default;
        public ScriptableIntReference JumpsLimit { get { return jumpsLimit; } }
    }
}