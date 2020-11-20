using UnityEngine;
using UnityEngine.Events;
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

        protected override IMovementManager CreateMovementManager(UnityAction<bool> onTurnAction)
        {
            return new EnemySimpleMovementManager(GetComponent<Rigidbody2D>(), speedReference, raycastLayerMask.value, raycastPivot, onTurnAction);
        }

        public void Turn()
        {
            ((EnemySimpleMovementManager)movementManager).Turn();
        }

        public void Turn(bool right)
        {
            ((EnemySimpleMovementManager)movementManager).Turn(right);
        }
    }
}