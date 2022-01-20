using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Enemy Map Data")]
    public class EnemyMapData : ScriptableObject
    {
        public enum PlayerDiscoveredBehaviour
        {
            None = 0,
            Chase,
            DistanceAttack,
            Trap
        }

        [Header("General")]
        [SerializeField]
        private PlayerDiscoveredBehaviour discoverBehaviour = default;
        public PlayerDiscoveredBehaviour DiscoverBehaviour { get { return discoverBehaviour; } }

        [Header("Move state")]
        [SerializeField]
        private float speed = default;
        public float Speed { get { return speed; } }

        [Header("Chase state")]
        [SerializeField]
        private float chaseSpeed = default;
        public float ChaseSpeed { get { return chaseSpeed; } }

        [Header("Player discovery")]
        [SerializeField]
        private float viewDistance = default;
        public float ViewDistance { get { return viewDistance; } }

        [SerializeField]
        private float viewAngle = default;
        public float ViewAngle { get { return viewAngle; } }

        [Header("Ranged attack state")]
        [SerializeField]
        private float attackDelay = default;
        public float AttackDelay { get { return attackDelay; } }
    }
}