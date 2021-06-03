using UnityEngine;
using Laresistance.Data;

namespace Laresistance.Systems
{
    [CreateAssetMenu(menuName = "Laresistance/Systems/Map behaviour pause resume system")]
    public class MapBehaviourPauseResumeSystem : ScriptableObject
    {
        private int pauseStack = 0;

        public void PauseAll(RuntimeMapBehaviourSet set)
        {
            pauseStack++;
            if (pauseStack > 0)
            {
                set.ForEach((mapBehaviour) => mapBehaviour.PauseMapBehaviour());
            }
        }

        public void ResumeAll(RuntimeMapBehaviourSet set)
        {
            pauseStack--;
            pauseStack = System.Math.Max(pauseStack, 0);
            if (pauseStack == 0)
            {
                set.ForEach((mapBehaviour) => mapBehaviour.ResumeMapBehaviour());
            }
        }
    }
}