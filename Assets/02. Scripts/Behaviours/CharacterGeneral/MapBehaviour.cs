using Laresistance.Movement;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public abstract class MapBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RuntimeMapBehaviourSet mapBehaviours = default;
        [SerializeField]
        private UnityEvent<bool> onTurn = default;

        protected IMovementManager movementManager;

        protected virtual void Awake()
        {
            movementManager = CreateMovementManager((bool right)=> { onTurn?.Invoke(right); });
        }

        protected abstract IMovementManager CreateMovementManager(UnityAction<bool> onTurnAction);

        private void Update()
        {
            movementManager.Tick(Time.deltaTime);
        }

        public void PauseMapBehaviour()
        {
            movementManager?.Pause();
        }

        public void ResumeMapBehaviour()
        {
            movementManager?.Resume();
        }

        private void OnDisable()
        {
            mapBehaviours.Remove(this);
        }

        private void OnEnable()
        {
            mapBehaviours.Add(this);
        }

        public IMovementManager GetMovementManager()
        {
            return movementManager;
        }
    }
}