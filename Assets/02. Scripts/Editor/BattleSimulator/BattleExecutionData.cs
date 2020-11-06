using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Simulator
{
    public struct BattleExecutionData
    {
        public int playerHealth;
        public AbilityData playerAbility;
        public MinionData[] minions;
        public ConsumableData[] consumables;
        public EquipmentData[] equipments;
        public EnemyPartyData enemies;
        public bool playerWon;
        public int playerRemainingHealth;
        public int enemiesRemainingHealth;
        public float battleDuration;
    }
}