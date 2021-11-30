using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Selectors;
using UnityEngine.AddressableAssets;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class AddressableSelectorInstancerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AssetReference[] gameObjectsReferences = default;
        [SerializeField]
        private ScriptableIntReference indexReference = default;
        [SerializeField]
        private bool spawnOnAwake = default;

        private void Awake()
        {
            if (spawnOnAwake)
            {
                Spawn();
            }
        }

        public void Spawn()
        {
            //var asyncOp = selectorReference.Get().InstantiateAsync(transform.parent, true);
            var asyncOp = gameObjectsReferences[indexReference.GetValue()].InstantiateAsync(transform.parent, true);
            asyncOp.Completed += (obj) => {
                Debug.LogFormat("Spawned instance of object {0}", obj.Result.name);
            };
        }
    }
}