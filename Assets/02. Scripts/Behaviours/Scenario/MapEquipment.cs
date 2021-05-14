using GamedevsToolbox.ScriptableArchitecture.Events;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using Laresistance.Systems;
using System.Collections;
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
        [SerializeField]
        private EquipmentData[] possibleDatas = default;
        [SerializeField]
        private bool randomDataFromDatas = false;

        [Header("Object")]
        [SerializeField]
        private SpriteRenderer spriteRenderer = default;

        private RewardSystem rewardSystem;

        private void Start()
        {
            rewardSystem = new RewardSystem(playerDataRef.Get().player, bloodReference, hardCurrencyReference, rewardUILibrary);
            if (randomDataFromDatas)
            {
                SetData(possibleDatas[Random.Range(0, possibleDatas.Length)]);
            }
        }

        public void SetData(EquipmentData data)
        {
            this.data = data;
            spriteRenderer.sprite = data.SpriteReference;
        }

        public void Interact()
        {
            Equipment e = EquipmentFactory.GetEquipment(data, playerDataRef.Get().StatusManager);
            StartCoroutine(RewardCoroutine(e));
        }

        private IEnumerator RewardCoroutine(Equipment e)
        {
            spriteRenderer.enabled = false;
            gameContextSignal.Raise("UI");
            yield return rewardSystem.GetReward(new RewardData(0, 0, null, null, e, null, null));
            gameContextSignal.Raise("Map");
            Destroy(gameObject);
        }
    }
}