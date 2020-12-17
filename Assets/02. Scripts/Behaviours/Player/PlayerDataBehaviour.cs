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
            StatusManager.SetEquipmentEvents(player.GetEquipmentEvents());

            BattleAbility[] playerAbilities = new BattleAbility[4];
            playerAbilities[0] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[0], player.GetEquipmentEvents(), StatusManager);
            playerAbilities[1] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[1], player.GetEquipmentEvents(), StatusManager);
            playerAbilities[2] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[2], player.GetEquipmentEvents(), StatusManager);
            playerAbilities[3] = BattleAbilityFactory.GetBattleAbility(playerAbilityData[3], player.GetEquipmentEvents(), StatusManager);
            BattleAbility ultimate = BattleAbilityFactory.GetBattleAbility(playerAbilityData[4], player.GetEquipmentEvents(), StatusManager);

            player.SetMainAbilities(playerAbilities, ultimate);

            if (useStartingData)
            {
                foreach (var md in startingMinions)
                {
                    Minion m = MinionFactory.GetMinion(md, 1, player.GetEquipmentEvents(), StatusManager);
                    player.EquipMinion(m);
                }

                foreach (var cd in startingConsumables)
                {
                    Consumable c = ConsumableFactory.GetConsumable(cd, player.GetEquipmentEvents(), StatusManager);
                    player.AddConsumable(c);
                }

                foreach (var eq in startingEquipments)
                {
                    Equipment e = EquipmentFactory.GetEquipment(eq, player.GetEquipmentEvents(), StatusManager);
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