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
        protected Character2DController characterController = default;

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