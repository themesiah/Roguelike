using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Minion")]
    public class MinionData : EnemyData
    {
        [SerializeField]
        private int baseBloodPrice = default;
        public int BaseBloodPrice {  get { return baseBloodPrice; } }

        [SerializeField]
        private FactionType faction = default;
        public FactionType Faction { get { return faction; } }
    }
}