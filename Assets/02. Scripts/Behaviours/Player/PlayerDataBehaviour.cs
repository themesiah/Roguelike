using Laresistance.Data;
using Laresistance.Core;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Laresistance.Battle;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class PlayerDataBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AbilityData[] playerAbilityData = default;
        [SerializeField]
        private ComboData[] combosData = default;
        [SerializeField]
        private bool useStartingData = false;
        [SerializeField]
        private List<MinionData> startingMinions = default;
        [SerializeField]
        private List<ConsumableData> startingConsumables = default;
        [SerializeField]
        private List<EquipmentData> startingEquipments = default;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataBehaviourReference = default;
        [SerializeField]
        protected Transform effectTargetPivot = default;
        [SerializeField]
        private ScriptableFloatReference energyProductionRef = default;

        public Player player { get; private set; }
        public BattleStatusManager StatusManager { get; private set; }

        private void Awake()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(150), effectTargetPivot, energyProductionRef.GetValue());
            player = new Player(StatusManager);
            StatusManager.health.RegisterEquipmentEvents(player.GetEquipmentContainer());
            StatusManager.SetEquipmentsContainer(player.GetEquipmentContainer());

            // Abilities
            BattleAbility[] playerAbilities = new BattleAbility[4];
            playerAbilities[0] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[0], player.GetEquipmentContainer(), StatusManager);
            playerAbilities[1] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[1], player.GetEquipmentContainer(), StatusManager);
            playerAbilities[2] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[2], player.GetEquipmentContainer(), StatusManager);
            playerAbilities[3] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[3], player.GetEquipmentContainer(), StatusManager);
            BattleAbility ultimate = BattleAbilityFactory.GetBattleAbility(playerAbilityData[4], player.GetEquipmentContainer(), StatusManager);

            player.SetMainAbilities(playerAbilities, ultimate);

            // Combos
            Combo[] combos = new Combo[combosData.Length];
            for (int i = 0; i < combos.Length; ++i)
            {
                combos[i] = new Combo(combosData[i], player.GetEquipmentContainer(), StatusManager);
            }
            player.SetCombos(combos);

            // Starting minions, equipments... for testing purposes
            if (useStartingData)
            {
                foreach (var md in startingMinions)
                {
                    Minion m = MinionFactory.GetMinion(md, 1, player.GetEquipmentContainer(), StatusManager);
                    player.EquipMinion(m);
                }

                foreach (var cd in startingConsumables)
                {
                    Consumable c = ConsumableFactory.GetConsumable(cd, player.GetEquipmentContainer(), StatusManager);
                    player.AddConsumable(c);
                }

                foreach (var eq in startingEquipments)
                {
                    Equipment e = EquipmentFactory.GetEquipment(eq, StatusManager);
                    player.EquipEquipment(e);
                }
            }
        }

        private void OnEnable()
        {
            playerDataBehaviourReference.Set(this);
        }

        private void OnDisable()
        {
            playerDataBehaviourReference.Set(null);
        }
    }
}