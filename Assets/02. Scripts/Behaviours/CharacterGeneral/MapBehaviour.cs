using Laresistance.Movement;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public abstract class MapBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RuntimeMapBehaviourSet mapBehaviours = default;

        protected IMovementManager movementManager;

        protected virtual void Awake()
        {
            movementManager = CreateMovementManager();
        }

        protected abstract IMovementManager CreateMovementManager();

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
    }
}