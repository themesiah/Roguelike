using UnityEngine;

namespace Laresistance.Behaviours
{
    public class RandomRotationY : RandomAsset<float, Transform>
    {
        protected override void SubtituteAsset(Transform reference, float newAsset)
        {
            var euler = reference.localEulerAngles;
            euler.y = newAsset;
            reference.localEulerAngles = euler;
        }
    }
}