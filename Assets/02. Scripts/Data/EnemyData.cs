using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Enemy")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField]
        private string nameRef = default;
        public string NameRef { get { return nameRef; } }

        [SerializeField]
        private int maxHealth = default;
        public int MaxHealth { get { return maxHealth; } }

        [SerializeField]
        private AssetReference prefabReference = default;
        public AssetReference PrefabReference { get { return prefabReference; } }

        [SerializeField]
        private AbilityData[] abilitiesData = default;
        public AbilityData[] AbilitiesData { get { return abilitiesData; } }

        [SerializeField]
        private int baseBloodReward = default;
        public int BaseBloodReward { get { return baseBloodReward; } }
    }
}