using UnityEngine;
using Laresistance.Battle;
using Laresistance.Data;

namespace Laresistance.Simulator
{
    [CreateAssetMenu(menuName = "Laresistance/Editor/Battle Simulator Data")]
    public class BattleSimulatorData : ScriptableObject
    {
        [Header("Player")]
        [SerializeField]
        private int playerHealth = default;
        public int PlayerHealth { get { return playerHealth; } }

        [SerializeField]
        private AbilityData[] playerAbilities = default;
        public AbilityData[] PlayerAbilities { get { return playerAbilities; } }

        [SerializeField]
        private MinionData[] possibleMinions = default;
        public MinionData[] PossibleMinions { get { return possibleMinions; } }

        [SerializeField]
        private EquipmentData[] possibleEquipments = default;
        public EquipmentData[] PossibleEquipments { get { return possibleEquipments; } }

        [SerializeField]
        private ConsumableData[] possibleConsumables = default;
        public ConsumableData[] PossibleConsumables { get { return possibleConsumables; } }

        [Header("Enemies")]
        [SerializeField]
        private EnemyPartyData[] possibleBattles = default;
        public EnemyPartyData[] PossibleBattles { get { return possibleBattles; } }
    }
}