using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Laresistance.Behaviours
{
    public class RandomGameObject : RandomAsset<AssetReference, Transform>
    {
        protected override void SubtituteAsset(Transform reference, AssetReference newAsset)
        {
            var op = newAsset.InstantiateAsync(reference, true);
            op.Completed += (obj) => {
                GameObject go = obj.Result;
                go.transform.localPosition = Vector3.zero;
            };
        }
    }
}