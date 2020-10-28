using UnityEngine;
using Laresistance.Movement;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class EnemyMapBehaviour : MapBehaviour
    {
        [SerializeField]
        private ScriptableFloatReference speedReference = default;
        [SerializeField]
        private LayerMask raycastLayerMask = default;
        [SerializeField]
        private Transform raycastPivot = default;

        protected override IMovementManager CreateMovementManager()
        {
            return new EnemySimpleMovementManager(GetComponent<Rigidbody2D>(), speedReference, raycastLayerMask.value, raycastPivot);
        }
    }
}