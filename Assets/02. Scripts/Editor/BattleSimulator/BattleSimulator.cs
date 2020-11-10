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
using System.Text;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.Utils;
using System.Linq;

namespace Laresistance.Simulator
{
    public class BattleSimulator : EditorWindow
    {
        private static float BATTLE_SIMULATION_TIME_PER_TICK = 0.33f;

        private BattleSimulatorData simulatorData;
        private TextAsset localizationTextAsset;
        private int enemyPartyDataIndex;
        private EditorCoroutine simulationCoroutine;

        private int minionsAmount;
        private int equipmentsAmount;
        private int consumablesAmount;
        private bool allowCorruptedEquipment;
        private int triesPerEnemyParty = 1;

        private bool minionsOverride;
        private MinionOverrideList minionsOverrideList;
        private bool equipmentsOverride;
        private EquipmentOverrideList equipmentsOverrideList;

        [MenuItem("Battle Simulator", menuItem = "Laresistance/Battle Simulator")]
        public static void OpenWindow()
        {
            BattleSimulator window = (BattleSimulator)EditorWindow.GetWindow(typeof(BattleSimulator));
            window.AutoSearchBaseData();
            window.Show();
        }

        public void AutoSearchBaseData()
        {
            var assets = AssetDatabase.FindAssets("t:BattleSimulatorData");
            if (assets.Length > 0)
            {
                simulatorData = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(BattleSimulatorData)) as BattleSimulatorData;
            }
            var assets2 = AssetDatabase.FindAssets("TextDB");
            if (assets2.Length > 0)
            {
                localizationTextAsset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets2[0]), typeof(TextAsset)) as TextAsset;
                Texts.Init(localizationTextAsset);
            }
        }

        [System.Obsolete]
        private void OnGUI()
        {
            simulatorData = EditorGUILayout.ObjectField(new GUIContent("Data:"), simulatorData, typeof(BattleSimulatorData)) as BattleSimulatorData;
            localizationTextAsset = EditorGUILayout.ObjectField(new GUIContent("Localization:"), localizationTextAsset, typeof(TextAsset)) as TextAsset;
            if (localizationTextAsset != null && !Texts.IsInitialized())
            {
                Texts.Init(localizationTextAsset);
            }
            if (simulatorData != null && Texts.IsInitialized())
            {
                enemyPartyDataIndex = EditorGUILayout.IntField(new GUIContent("Enemy party index:"), enemyPartyDataIndex);
                if (enemyPartyDataIndex >= 0 && simulatorData.PossibleBattles.Length > enemyPartyDataIndex)
                {
                    StringBuilder enemiesText = new StringBuilder();
                    enemiesText.Append(Texts.GetText(simulatorData.PossibleBattles[enemyPartyDataIndex].enemyData.NameRef));
                    enemiesText.Append("(");
                    enemiesText.Append(simulatorData.PossibleBattles[enemyPartyDataIndex].enemyData.name);
                    enemiesText.Append(")");
                    foreach (var enemyData in simulatorData.PossibleBattles[enemyPartyDataIndex].partyEnemiesData)
                    {
                        enemiesText.Append(", ");
                        enemiesText.Append(Texts.GetText(enemyData.NameRef));
                        enemiesText.Append("(");
                        enemiesText.Append(enemyData.name);
                        enemiesText.Append(")");
                    }
                    EditorGUILayout.LabelField(enemiesText.ToString());
                }
                else
                {
                    EditorGUILayout.LabelField("Battle index out of range");
                }
            }
            // Retries
            triesPerEnemyParty = EditorGUILayout.IntField(new GUIContent("Tries per enemy party:"), triesPerEnemyParty);
            // Minions
            minionsOverride = EditorGUILayout.Toggle(new GUIContent("Override minions:"), minionsOverride);
            if (minionsOverride)
            {
                minionsOverrideList = EditorGUILayout.ObjectField(new GUIContent("Minion override list:"), minionsOverrideList, typeof(MinionOverrideList)) as MinionOverrideList;
            } else
            {
                minionsAmount = EditorGUILayout.IntField(new GUIContent("Minions amount:"), minionsAmount);
            }
            // Equipments
            equipmentsOverride = EditorGUILayout.Toggle(new GUIContent("Override equipments:"), equipmentsOverride);
            if (equipmentsOverride)
            {
                equipmentsOverrideList = EditorGUILayout.ObjectField(new GUIContent("Equipment override list:"), equipmentsOverrideList, typeof(EquipmentOverrideList)) as EquipmentOverrideList;
            } else
            {
                equipmentsAmount = EditorGUILayout.IntField(new GUIContent("Equipments amount:"), equipmentsAmount);
                allowCorruptedEquipment = EditorGUILayout.Toggle(new GUIContent("Allow corrupted equipment:"), allowCorruptedEquipment);
            }
            // Consumables
            consumablesAmount = EditorGUILayout.IntField(new GUIContent("Consumables amount:"), consumablesAmount);

            // Buttons
            if (simulatorData != null && localizationTextAsset != null)
            {
                if (simulationCoroutine == null)
                {
                    if (GUILayout.Button("Simulate!"))
                    {
                        simulationCoroutine = EditorCoroutineUtility.StartCoroutineOwnerless(SimualateRandomBattles(simulatorData));
                    }
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
        }

        private IEnumerator SimualateRandomBattles(BattleSimulatorData simulatorData)
        {
            Texts.Init(localizationTextAsset);
            Utils.DeleteFile("simulation.log");
            StartReport();
            StartPartyReport(simulatorData.PossibleBattles[enemyPartyDataIndex]);
            for (int tries = 0; tries < triesPerEnemyParty; tries++)
            {
                AbilityData ability = simulatorData.PlayerAbilities[Random.Range(0, simulatorData.PlayerAbilities.Length)];
                List<MinionData> minions = new List<MinionData>();
                if (minionsOverride)
                {
                    for (int i = 0; i < minionsOverrideList.Minions.Count; i++)
                    {
                        minions.Add(minionsOverrideList.Minions[i]);
                    }
                } else {
                    for (int i = 0; i < minionsAmount; i++) // Minion amount
                    {
                        minions.Add(simulatorData.PossibleMinions[Random.Range(0, simulatorData.PossibleMinions.Length)]);
                    }
                }
                List<EquipmentData> equipments = new List<EquipmentData>();
                if (equipmentsOverride)
                {
                    for (int i = 0; i < equipmentsOverrideList.Equipments.Count; ++i)
                    {
                        equipments.Add(equipmentsOverrideList.Equipments[i]);
                    }
                }
                else
                {
                    while (equipments.Count != equipmentsAmount)
                    {
                        EquipmentData ed = simulatorData.PossibleEquipments[Random.Range(0, simulatorData.PossibleEquipments.Length)];
                        while (equipments.Any((EquipmentData anyEquip) => anyEquip.Slot == ed.Slot) || (allowCorruptedEquipment == false && ed.Corrupted))
                        {
                            ed = simulatorData.PossibleEquipments[Random.Range(0, simulatorData.PossibleEquipments.Length)];
                        }
                        equipments.Add(ed);
                    }
                }
                List<ConsumableData> consumables = new List<ConsumableData>();
                for (int k = 0; k < consumablesAmount; k++)
                {
                    consumables.Add(simulatorData.PossibleConsumables[Random.Range(0, simulatorData.PossibleConsumables.Length)]);
                }

                yield return SimulateBattle(simulatorData.PlayerHealth, ability, minions.ToArray(), equipments.ToArray(), consumables.ToArray(), simulatorData.PossibleBattles[enemyPartyDataIndex]);
            }
            //yield return SimulateBattle(simulatorData.PlayerHealth, simulatorData.PlayerAbilities[0], simulatorData.PossibleMinions, simulatorData.PossibleEquipments, simulatorData.PossibleConsumables, simulatorData.PossibleBattles[0]);
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
                string power = string.Format("Enemy power modifier is {0}", battleSystem.GetEnemies()[2].StatusManager.GetDamageModifier());
                //Utils.AppendText("simulation.log", power);
                //Debug.Log(power);
                //Debug.LogFormat("Simulated {0} seconds. Player health is {1}. Enemy health is {2}", battleTimer.ToString(), battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth(), battleSystem.GetEnemies()[0].StatusManager.health.GetCurrentHealth());
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
            if (!battleSystem.PlayerDead())
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

            IAbilityInputProcessor abilityInput = new PlayerSimulatorAbilityInput(player.GetAbilities());
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

        private void StartReport()
        {
            string reportStart = string.Format("Starting report with {0} minions, {1} equipments, {2} consumables",
                minionsAmount,
                equipmentsAmount,
                consumablesAmount);
            Utils.AppendText("simulation.log", reportStart);
            Debug.Log(reportStart);
        }

        private void StartPartyReport(EnemyPartyData enemyParty)
        {
            StringBuilder enemiesText = new StringBuilder();
            enemiesText.Append(Texts.GetText(enemyParty.enemyData.NameRef));
            enemiesText.Append("(");
            enemiesText.Append(enemyParty.enemyData.name);
            enemiesText.Append(")");
            foreach (var enemyData in enemyParty.partyEnemiesData)
            {
                enemiesText.Append(", ");
                enemiesText.Append(Texts.GetText(enemyData.NameRef));
                enemiesText.Append("(");
                enemiesText.Append(enemyData.name);
                enemiesText.Append(")");
            }
            string partyText = string.Format("Begin of enemy party:{0}", enemiesText.ToString());
            Utils.AppendText("simulation.log", partyText);
            Debug.Log(partyText);
        }

        private void GenerateReport(BattleExecutionData battleExecutionData)
        {
            string wonIndicator = battleExecutionData.playerWon ? ">>>>>" : "<<<<<";
            string whoWon = battleExecutionData.playerWon ? "The player" : "The enemies";
            int remainingHealth = battleExecutionData.playerWon ? battleExecutionData.playerRemainingHealth : battleExecutionData.enemiesRemainingHealth;
            BattleAbility playerAbility = BattleAbilityFactory.GetBattleAbility(battleExecutionData.playerAbility, new EquipmentEvents(), new BattleStatusManager(new CharacterHealth(1)));
            StringBuilder minionsText = new StringBuilder();
            for (int i = 0; i < battleExecutionData.minions.Length; ++i)
            {
                var minionData = battleExecutionData.minions[i];
                minionsText.Append(Texts.GetText(minionData.NameRef));
                minionsText.Append( "(" );
                minionsText.Append( minionData.name );
                minionsText.Append( ")" );
                if (i != battleExecutionData.minions.Length-1)
                    minionsText.Append(Texts.GetText(", "));
            }
            StringBuilder equipmentsText = new StringBuilder();
            for (int i = 0; i < battleExecutionData.equipments.Length; ++i)
            {
                var equipmentData = battleExecutionData.equipments[i];
                equipmentsText.Append(Texts.GetText(equipmentData.EquipmentNameReference));
                equipmentsText.Append("(");
                equipmentsText.Append(equipmentData.name);
                equipmentsText.Append(")");
                if (i != battleExecutionData.equipments.Length-1)
                    equipmentsText.Append(Texts.GetText(", "));
            }
            StringBuilder consumablesText = new StringBuilder();
            for (int i = 0; i < battleExecutionData.consumables.Length; ++i)
            {
                var consumableData = battleExecutionData.consumables[i];
                consumablesText.Append(Texts.GetText(consumableData.NameRef));
                consumablesText.Append("(");
                consumablesText.Append(consumableData.name);
                consumablesText.Append(")");
                if (i != battleExecutionData.consumables.Length-1)
                    consumablesText.Append(Texts.GetText(", "));
            }
            StringBuilder enemiesText = new StringBuilder();
            enemiesText.Append(Texts.GetText(battleExecutionData.enemies.enemyData.NameRef));
            enemiesText.Append("(");
            enemiesText.Append(battleExecutionData.enemies.enemyData.name);
            enemiesText.Append(")");
            foreach (var enemyData in battleExecutionData.enemies.partyEnemiesData)
            {
                enemiesText.Append(", ");
                enemiesText.Append(Texts.GetText(enemyData.NameRef));
                enemiesText.Append("(");
                enemiesText.Append(enemyData.name);
                enemiesText.Append(")");
            }

            string battleText = string.Format("{8}{0} has won in {1} seconds with {2} remaining hp.\nPlayer ability was {3}\nMinions: {4}.\nEquipments {5}.\nConsumables {6}.\nEnemies {7}.\n\n\n",
                whoWon,
                battleExecutionData.battleDuration.ToString(),
                remainingHealth.ToString(),
                playerAbility.GetAbilityText(1),
                minionsText.ToString(),
                equipmentsText.ToString(),
                consumablesText.ToString(),
                enemiesText.ToString(),
                wonIndicator);
            Utils.AppendText("simulation.log", battleText);
            //Debug.LogFormat("Battle finished! {0} has won in {1} seconds with {2} remaining hp.", whoWon, battleExecutionData.battleDuration.ToString(), remainingHealth.ToString());
            Debug.Log(battleText);
        }
    }
}