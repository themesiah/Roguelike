using UnityEngine;
using UnityEngine.Events;
using Laresistance.Movement;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Battle;

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
        [SerializeField]
        private bool partyMember = false;

        protected override IMovementManager CreateMovementManager(UnityAction<bool> onTurnAction)
        {
            return new EnemySimpleMovementManager(GetComponent<Rigidbody2D>(), speedReference, raycastLayerMask.value, raycastPivot, onTurnAction);
        }

        private void Start()
        {
            if (!partyMember)
            {
                bool enoughSpace = BattlePosition.CheckSpace(transform.position, raycastLayerMask.value);
                if (!enoughSpace)
                {
                    Debug.LogErrorFormat(gameObject, "Enemy {0} have no space to battle", name);
                }
            }
        }

        public void Turn()
        {
            ((EnemySimpleMovementManager)movementManager).Turn();
        }

        public void Turn(bool right)
        {
            ((EnemySimpleMovementManager)movementManager).Turn(right);
        }

        protected override void Update()
        {
            if (!partyMember)
            {
                base.Update();
            }
        }
    }
}