using UnityEngine;

namespace Laresistance.Behaviours
{
    public class RandomRotationZ : RandomAsset<float, Transform>
    {
        protected override void SubtituteAsset(Transform reference, float newAsset)
        {
            var euler = reference.localEulerAngles;
            euler.z = newAsset;
            reference.localEulerAngles = euler;
        }
    }
}