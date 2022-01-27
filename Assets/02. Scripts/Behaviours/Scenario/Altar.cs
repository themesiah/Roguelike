using UnityEngine;
using UnityEngine.Events;
using Laresistance.Data;
using Laresistance.Systems;
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
        private RewardEvent rewardEvent = default;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private StringGameEvent gameContextSignal = default;
        [SerializeField]
        private UnityEvent OnStatRetrieved = default;

        private StatsType[] statsTypeList;
        private bool alreadyActivated = false;

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
            yield return rewardEvent?.Raise(new RewardData(0, 0, null, null, null, null, statsTypeList, playerDataRef.Get().player, true));
            OnStatRetrieved?.Invoke();
            gameContextSignal.Raise("Map");
            yield return characterDialogEvent?.Raise(statsDialog);
        }
    }
}