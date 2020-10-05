using UnityEngine;
using Laresistance.Data;

namespace Laresistance.Systems
{
    [CreateAssetMenu(menuName = "Laresistance/Systems/Map behaviour pause resume system")]
    public class MapBehaviourPauseResumeSystem : ScriptableObject
    {
        public void PauseAll(RuntimeMapBehaviourSet set)
        {
            set.ForEach((mapBehaviour) => mapBehaviour.PauseMapBehaviour());
        }

        public void ResumeAll(RuntimeMapBehaviourSet set)
        {
            set.ForEach((mapBehaviour) => mapBehaviour.ResumeMapBehaviour());
        }
    }
}