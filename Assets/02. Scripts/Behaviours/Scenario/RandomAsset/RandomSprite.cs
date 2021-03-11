using UnityEngine;

namespace Laresistance.Behaviours
{
    public class RandomSprite : RandomAsset<Sprite, SpriteRenderer>
    {
        protected override void SubtituteAsset(SpriteRenderer reference, Sprite newAsset)
        {
            reference.sprite = newAsset;
        }
    }
}