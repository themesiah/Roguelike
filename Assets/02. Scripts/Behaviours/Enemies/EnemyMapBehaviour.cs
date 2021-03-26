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

        private IMovementManager movementManager;

        protected override void Awake()
        {
            base.Awake();
            movementManager = new EnemySimpleMovementManager(characterController, speedReference, raycastLayerMask.value, raycastPivot);
        }

        private void Start()
        {
            if (!partyMember)
            {
                try
                {
                    bool enoughSpace = BattlePosition.CheckSpace(transform.position, raycastLayerMask.value);
                    if (!enoughSpace)
                    {
                        Debug.LogErrorFormat(gameObject, "Enemy {0} have no space to battle", name);
                    }
                } catch(System.Exception e)
                {
                    Debug.LogError(e);
                    Debug.LogErrorFormat("Enemy {0} have no space to battle in room {1}", name, transform.parent.parent.parent.name);
                }
            }
        }

        private void Update()
        {
            if (!partyMember)
            {
                movementManager.Tick(Time.deltaTime);
            }
        }

        public override void PauseMapBehaviour()
        {
            base.PauseMapBehaviour();
            movementManager?.Pause();
        }

        public override void ResumeMapBehaviour()
        {
            base.ResumeMapBehaviour();
            movementManager?.Resume();
        }
    }
}