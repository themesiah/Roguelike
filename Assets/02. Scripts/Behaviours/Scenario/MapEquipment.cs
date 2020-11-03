using GamedevsToolbox.ScriptableArchitecture.Events;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using Laresistance.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class MapEquipment : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private RewardUILibrary rewardUILibrary = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        [SerializeField]
        private EquipmentData data = default;
        [SerializeField]
        private StringGameEvent gameContextSignal = default;

        [Header("Object")]
        [SerializeField]
        private SpriteRenderer spriteRenderer = default;

        private RewardSystem rewardSystem;

        private void Start()
        {
            rewardSystem = new RewardSystem(playerDataRef.Get().player, bloodReference, hardCurrencyReference, rewardUILibrary);
        }

        public void SetData(EquipmentData data)
        {
            this.data = data;
            spriteRenderer.sprite = data.SpriteReference;
        }

        public void Interact()
        {
            Equipment e = EquipmentFactory.GetEquipment(data, playerDataRef.Get().player.GetEquipmentEvents(), playerDataRef.Get().StatusManager);
            StartCoroutine(RewardCoroutine(e));
        }

        private IEnumerator RewardCoroutine(Equipment e)
        {
            spriteRenderer.enabled = false;
            gameContextSignal.Raise("UI");
            yield return rewardSystem.GetReward(new RewardData(0, 0, null, null, e, null));
            gameContextSignal.Raise("Map");
            Destroy(gameObject);
        }
    }
}