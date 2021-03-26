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
        protected UnityEvent<bool> onTurn = default;
        [SerializeField]
        protected Character2DController characterController = default;

        

        protected virtual void Awake()
        {
            characterController?.OnFlip.AddListener((bool right) => { onTurn?.Invoke(right); });
        }

        public virtual void PauseMapBehaviour()
        {
            characterController?.Pause();
        }

        public virtual void ResumeMapBehaviour()
        {
            characterController?.Resume();
        }

        private void OnDisable()
        {
            mapBehaviours.Remove(this);
        }

        private void OnEnable()
        {
            mapBehaviours.Add(this);
        }

        public Character2DController GetCharacterController()
        {
            return characterController;
        }
    }
}