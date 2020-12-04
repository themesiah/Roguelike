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
        private AbilityData playerAbilityData = default;
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
        private ScriptableFloatReference energyProductionRef = default;

        public Player player { get; private set; }
        public BattleStatusManager StatusManager { get; private set; }

        private void Awake()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(100), energyProductionRef.GetValue());
            player = new Player(StatusManager);
            StatusManager.SetEquipmentEvents(player.GetEquipmentEvents());

            BattleAbility playerAbility = BattleAbilityFactory.GetBattleAbility(playerAbilityData, player.GetEquipmentEvents(), StatusManager);

            player.SetMainAbility(playerAbility);

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