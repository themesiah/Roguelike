using DigitalRuby.Tween;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public abstract class RewardPanelBehaviour : MonoBehaviour, IPanelBehaviour
    {
        [SerializeField]
        protected GameObject panel = default;
        [SerializeField]
        protected RuntimePlayerDataBehaviourSingle playerDataReference = default;
        [SerializeField]
        private RewardUILibrary rewardUILibrary = default;
        [SerializeField]
        private Button startingButton = default;
        [SerializeField]
        protected Button[] selectableButtons = default;
        [SerializeField]
        protected ScriptableIntReference currentLevelRef = default;

        private bool finished = false;
        protected int selectedOptionIndex = -2;

        private void Awake()
        {
            panel.SetActive(false);
            SetButtonCallbacks();
        }

        protected virtual void SetButtonCallbacks()
        {
            for (int i = 0; i < selectableButtons.Length; ++i)
            {
                if (i == 0)
                {
                    selectableButtons[i].onClick.AddListener(() => { selectedOptionIndex = -1; });
                }
                else
                {
                    int currentIndex = i;
                    selectableButtons[i].onClick.AddListener(() => { selectedOptionIndex = currentIndex - 1; });
                }
            }
        }

        private void OnEnable()
        {
            rewardUILibrary.RegisterBehaviour(this);
        }

        private void OnDisable()
        {
            rewardUILibrary.UnregisterBehaviour(this);
        }

        private void UpdatePanelSize(ITween<Vector2> t)
        {
            panel.transform.localScale = t.CurrentValue;
        }

        private void ResizeCompleted(ITween<Vector2> t)
        {
            finished = true;
        }

        protected virtual IEnumerator StartingTween(RewardData rewardData)
        {
            panel.SetActive(true);
            finished = false;
            TweenFactory.Tween("PanelSize", Vector3.zero, Vector3.one, 0.5f, TweenScaleFunctions.CubicEaseIn, UpdatePanelSize, ResizeCompleted);
            while (!finished)
            {
                yield return null;
            }
        }

        protected virtual IEnumerator FinishingTween(RewardData rewardData)
        {
            panel.Tween("PanelSize", Vector3.one, Vector2.zero, 0.5f, TweenScaleFunctions.CubicEaseIn, UpdatePanelSize, ResizeCompleted);
            finished = false;
            while (!finished)
            {
                yield return null;
            }
            panel.SetActive(false);
        }

        public IEnumerator StartPanel(RewardData rewardData)
        {
            yield return StartingTween(rewardData);
            startingButton?.Select();
            yield return ExecutePanelProcess(rewardData);
            yield return FinishingTween(rewardData);
        }

        protected abstract IEnumerator ExecutePanelProcess(RewardData rewardData);
        public abstract RewardUIType RewardType { get; }
    }
}