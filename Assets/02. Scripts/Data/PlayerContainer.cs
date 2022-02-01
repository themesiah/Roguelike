using UnityEngine;
using Laresistance.Core;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Player Container")]
    public class PlayerContainer : ScriptableObject
    {
        public Player player;

        public void Reset()
        {
            player = null;
        }
    }
}