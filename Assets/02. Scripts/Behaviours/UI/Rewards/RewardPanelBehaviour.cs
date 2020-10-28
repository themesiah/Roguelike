using DigitalRuby.Tween;
using Laresistance.Core;
using Laresistance.Data;
using System.Collections;
using UnityEngine;

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

        private bool finished = false;

        private void Awake()
        {
            panel.SetActive(false);
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
            yield return ExecutePanelProcess(rewardData);
            yield return FinishingTween(rewardData);
        }

        protected abstract IEnumerator ExecutePanelProcess(RewardData rewardData);
        public abstract RewardUIType RewardType { get; }
    }
}