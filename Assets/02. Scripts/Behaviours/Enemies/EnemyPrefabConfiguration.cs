using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class EnemyPrefabConfiguration : MonoBehaviour
    {
        public void ConfigurePrefab(GameObject prefab, UnityAction<AnimatorWrapperBehaviour> animatorSetup)
        {
            GameObject go = Instantiate(prefab, transform);
            AnimatorWrapperBehaviour anim = go.GetComponent<AnimatorWrapperBehaviour>();
            Assert.IsNotNull(anim);
            animatorSetup?.Invoke(anim);
        }
    }
}