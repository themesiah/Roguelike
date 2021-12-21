using UnityEngine;
using Laresistance.Data;
using Laresistance.Systems;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections;
using Laresistance.Systems.Dialog;
using GamedevsToolbox.ScriptableArchitecture.Events;

namespace Laresistance.Behaviours
{
    public class Altar : MonoBehaviour
    {
        [Header("Dialog")]
        [SerializeField]
        private CharacterDialogEvent characterDialogEvent = default;
        [SerializeField]
        private CharacterDialog characterDialog = default;
        [SerializeField]
        private CharacterDialog statsDialog = default;
        [Header("References")]
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        [SerializeField]
        private RewardUILibrary uiLibrary = default;
        [SerializeField]
        private StringGameEvent gameContextSignal = default;

        private StatsType[] statsTypeList;
        private RewardSystem rewardSystem;
        private bool alreadyActivated = false;

        private void Start()
        {
            rewardSystem = new RewardSystem(playerDataRef.Get().player, bloodReference, hardCurrencyReference, uiLibrary);
        }

        public void SetStats(StatsType[] statsList)
        {
            Debug.LogFormat("Altar {0} has {1} stats", gameObject.name, statsList.Length);
            statsTypeList = statsList;
        }

        public void Interact()
        {
            if (!alreadyActivated)
            {
                alreadyActivated = true;
                StartCoroutine(InteractCoroutine());
            }
        }

        private IEnumerator InteractCoroutine()
        {
            yield return characterDialogEvent?.Raise(characterDialog);
            gameContextSignal.Raise("UI");
            yield return rewardSystem.GetReward(new RewardData(0, 0, null, null, null, null, statsTypeList, playerDataRef.Get().player, true));
            gameContextSignal.Raise("Map");
            yield return characterDialogEvent?.Raise(statsDialog);
        }
    }
}