using UnityEngine;

namespace Laresistance.Behaviours
{
    public class RandomSpriteFlipY : RandomAsset<bool, SpriteRenderer>
    {
        protected override void SubtituteAsset(SpriteRenderer reference, bool newAsset)
        {
            reference.flipY = newAsset;
        }
    }
}