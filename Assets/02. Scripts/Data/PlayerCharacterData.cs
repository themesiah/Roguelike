using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Player Character")]
    public class PlayerCharacterData : ScriptableObject
    {
        [SerializeField]
        private PlayerCharacterList playerCharacterType = default;
        public PlayerCharacterList PlayerCharacterType { get { return playerCharacterType; } }

        [SerializeField]
        private int characterMaxHealth = default;
        public int CharacterMaxHealth { get { return characterMaxHealth; } }

        [SerializeField]
        private AbilityData[] playerAbilityData = default;
        public AbilityData[] PlayerAbilityData { get { return playerAbilityData; } }

        [SerializeField]
        private AbilityData playerUltimateAbility = default;
        public AbilityData PlayerUltimateAbility { get { return playerUltimateAbility; } }

        [SerializeField]
        private AbilityData playerSupportAbility = default;
        public AbilityData PlayerSupportAbility { get { return playerSupportAbility; } }

    }
}