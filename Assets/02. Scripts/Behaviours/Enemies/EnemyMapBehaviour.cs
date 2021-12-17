using UnityEngine;
using System.Collections;
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

        protected void Awake()
        {
            movementManager = new EnemySimpleMovementManager(characterController, speedReference, raycastLayerMask.value, raycastPivot);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            if (!partyMember)
            {
                try
                {
                    bool enoughSpace = BattlePosition.CheckSpace(transform.position, raycastLayerMask.value);
                    if (!enoughSpace)
                    {
                        GamedevsToolbox.Utils.Logger.Logger.LogError(string.Format("Enemy {0} have no space to battle", name), gameObject);
                    }
                } catch(System.Exception e)
                {
                    GamedevsToolbox.Utils.Logger.Logger.LogError(e.ToString());
                    GamedevsToolbox.Utils.Logger.Logger.LogError(string.Format("Enemy {0} have no space to battle in room {1}", name, transform.parent.parent.parent.name));
                }
            }
        }

        private void FixedUpdate()
        {
            if (!partyMember)
            {
                movementManager.Tick(Time.deltaTime);
            }
        }

        public override void PauseMapBehaviour()
        {
            //base.PauseMapBehaviour();
            movementManager?.Pause();
        }

        public override void ResumeMapBehaviour()
        {
            //base.ResumeMapBehaviour();
            movementManager?.Resume();
        }
    }
}