using UnityEngine;

namespace Laresistance.Behaviours
{
    public abstract class RandomAsset<T, T2> : MonoBehaviour
    {
        [SerializeField]
        private T[] assetList = default;

        [SerializeField]
        private T2 targetReference = default;

        private void Start()
        {
            T randomAsset = SelectRandomAsset();
            SubtituteAsset(targetReference, randomAsset);
        }

        private T SelectRandomAsset()
        {
            return assetList[Random.Range(0, assetList.Length)];
        }

        protected abstract void SubtituteAsset(T2 reference, T newAsset);
    }
}