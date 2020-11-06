using Laresistance.Battle;
using Laresistance.Data;
using Laresistance.Systems;
using Laresistance.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Laresistance.Behaviours;
using Unity.EditorCoroutines.Editor;

namespace Laresistance.Simulator
{
    public class BattleSimulator : EditorWindow
    {
        private static float BATTLE_SIMULATION_TIME_PER_TICK = 0.5f;

        private BattleSimulatorData simulatorData;
        private EditorCoroutine simulationCoroutine;

        [MenuItem("Battle Simulator", menuItem = "Laresistance/Battle Simulator")]
        public static void OpenWindow()
        {
            BattleSimulator window = (BattleSimulator)EditorWindow.GetWindow(typeof(BattleSimulator));
            window.Show();
        }

        [System.Obsolete]
        private void OnGUI()
        {
            simulatorData = EditorGUILayout.ObjectField(new GUIContent("Data:"), simulatorData, typeof(BattleSimulatorData)) as BattleSimulatorData;

            if (GUILayout.Button("Simulate!"))
            {
                simulationCoroutine = EditorCoroutineUtility.StartCoroutineOwnerless(SimualateRandomBattles(simulatorData));
            }

            if (simulationCoroutine != null)
            {
                if (GUILayout.Button("Stop simulation"))
                {
                    EditorCoroutineUtility.StopCoroutine(simulationCoroutine);
                    simulationCoroutine = null;
                }
            }
        }

        private IEnumerator SimualateRandomBattles(BattleSimulatorData simulatorData)
        {
            yield return SimulateBattle(simulatorData.PlayerHealth, simulatorData.PlayerAbilities[0], simulatorData.PossibleMinions, simulatorData.PossibleEquipments, simulatorData.PossibleConsumables, simulatorData.PossibleBattles[0]);
            simulationCoroutine = null;
        }

        private IEnumerator SimulateBattle(int playerHealth, AbilityData playerAbilityData, MinionData[] playerMinions, EquipmentData[] playerEquipments, ConsumableData[] playerConsumables, EnemyPartyData enemyParty)
        {
            BattleSystem battleSystem = new BattleSystem();
            CharacterBattleManager playerBattleManager = GetPlayerBattleManager(playerHealth, playerAbilityData, playerMinions, playerEquipments, playerConsumables);
            CharacterBattleManager[] enemyBattleManagers = new CharacterBattleManager[enemyParty.partyEnemiesData.Length+1];
            enemyBattleManagers[0] = GetEnemyBattleManager(enemyParty.enemyData);
            for(int i = 0; i < enemyParty.partyEnemiesData.Length; ++i)
            {
                enemyBattleManagers[i + 1] = GetEnemyBattleManager(enemyParty.partyEnemiesData[i]);
            }

            battleSystem.InitBattle(playerBattleManager, enemyBattleManagers);

            float battleTimer = 0f;
            Debug.Log("Battle simulation start");
            while (!battleSystem.AllEnemiesDead() && !battleSystem.PlayerDead())
            {
                yield return battleSystem.Tick(BATTLE_SIMULATION_TIME_PER_TICK);
                battleTimer += BATTLE_SIMULATION_TIME_PER_TICK;

                Debug.LogFormat("Simulated {0} seconds. Player health is {1}. Enemy health is {2}", battleTimer.ToString(), battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth(), battleSystem.GetEnemies()[0].StatusManager.health.GetCurrentHealth());
            }
            Debug.Log("Battle simulation end");
            BattleExecutionData battleExecutionData = new BattleExecutionData();
            battleExecutionData.playerHealth = playerHealth;
            battleExecutionData.playerAbility = playerAbilityData;
            battleExecutionData.minions = playerMinions;
            battleExecutionData.equipments = playerEquipments;
            battleExecutionData.consumables = playerConsumables;
            battleExecutionData.enemies = enemyParty;
            battleExecutionData.battleDuration = battleTimer;
            if (battleSystem.AllEnemiesDead())
            {
                battleExecutionData.playerWon = true;
                battleExecutionData.enemiesRemainingHealth = 0;
                battleExecutionData.playerRemainingHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            } else
            {
                battleExecutionData.playerWon = false;
                battleExecutionData.playerRemainingHealth = 0;
                battleExecutionData.enemiesRemainingHealth = 0;
                foreach(var enemy in battleSystem.GetEnemies())
                {
                    battleExecutionData.enemiesRemainingHealth += enemy.StatusManager.health.GetCurrentHealth();
                }
            }

            GenerateReport(battleExecutionData);
        }

        private CharacterBattleManager GetPlayerBattleManager(int playerHealth, AbilityData playerAbilityData, MinionData[] playerMinions, EquipmentData[] playerEquipments, ConsumableData[] playerConsumables)
        {
            BattleStatusManager playerStatus = new BattleStatusManager(new CharacterHealth(playerHealth));
            Player player = new Player(playerStatus);
            playerStatus.SetEquipmentEvents(player.GetEquipmentEvents());

            BattleAbility playerAbility = BattleAbilityFactory.GetBattleAbility(playerAbilityData, player.GetEquipmentEvents(), playerStatus);
            player.SetMainAbility(playerAbility);

            foreach (var md in playerMinions)
            {
                Minion m = MinionFactory.GetMinion(md, 1, player.GetEquipmentEvents(), playerStatus);
                player.EquipMinion(m);
            }

            foreach (var equipmentData in playerEquipments)
            {
                Equipment equip = EquipmentFactory.GetEquipment(equipmentData, player.GetEquipmentEvents(), playerStatus);
                if (!player.EquipEquipment(equip))
                {
                    Assert.IsTrue(false, "The simulation contains more than one equipment of the same slot");
                }
            }

            IAbilityInputProcessor abilityInput = new EnemyAbilityManager(player.GetAbilities(), 1, new DummyBattleAnimator());
            IAbilityExecutor abilityExecutor = new PlayerAbilityExecutor(player, new DummyBattleAnimator(), null);

            CharacterBattleManager playerBM = new CharacterBattleManager(playerStatus, abilityInput, abilityExecutor, new DummyTargetSelection(), new DummyBattleAnimator());
            return playerBM;
        }

        private CharacterBattleManager GetEnemyBattleManager(EnemyData enemyData)
        {
            BattleStatusManager enemyStatus = new BattleStatusManager(new CharacterHealth(enemyData.MaxHealth));
            BattleAbility[] abilities = new BattleAbility[enemyData.AbilitiesData.Length];
            for (int i = 0; i < enemyData.AbilitiesData.Length; ++i)
            {
                abilities[i] = BattleAbilityFactory.GetBattleAbility(enemyData.AbilitiesData[i], null, enemyStatus);
            }

            IAbilityInputProcessor abilityInput = new EnemyAbilityManager(abilities, 1, new DummyBattleAnimator());

            CharacterBattleManager enemy = new CharacterBattleManager(enemyStatus, abilityInput, (IAbilityExecutor)abilityInput, new DummyTargetSelection(), new DummyBattleAnimator());
            return enemy;
        }

        private void GenerateReport(BattleExecutionData battleExecutionData)
        {
            string whoWon = battleExecutionData.playerWon ? "The player" : "The enemies";
            int remainingHealth = battleExecutionData.playerWon ? battleExecutionData.playerRemainingHealth : battleExecutionData.enemiesRemainingHealth;
            Debug.LogFormat("Battle finished! {0} has won in {1} seconds with {2} remaining hp.", whoWon, battleExecutionData.battleDuration.ToString(), remainingHealth.ToString());
        }
    }
}