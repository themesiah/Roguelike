using Laresistance.Data;
using Laresistance.Core;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Laresistance.Battle;

namespace Laresistance.Behaviours
{
    public class PlayerDataBehaviour : MonoBehaviour
    {
        [SerializeField]
        private List<MinionData> startingMinions = default;
        [SerializeField]
        private List<ConsumableData> startingConsumables = default;
        [SerializeField]
        private List<EquipmentData> startingEquipments = default;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataBehaviourReference = default;

        public Player player { get; private set; }
        public BattleStatusManager StatusManager { get; private set; }

        private void Awake()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(100));
            player = new Player(StatusManager);
            StatusManager.SetEquipmentEvents(player.GetEquipmentEvents());

            List<BattleEffect> testEffects = new List<BattleEffect>();
            testEffects.Add(new BattleEffectDamage(15, EffectTargetType.Enemy, StatusManager));
            BattleAbility testAbility = new BattleAbility(testEffects, 3f, StatusManager, player.GetEquipmentEvents());
            player.SetMainAbility(testAbility);

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