using UnityEngine;

namespace Laresistance.Behaviours
{
    public class RandomSpriteFlip : RandomAsset<bool, SpriteRenderer>
    {
        protected override void SubtituteAsset(SpriteRenderer reference, bool newAsset)
        {
            reference.flipX = newAsset;
        }
    }
}