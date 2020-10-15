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

        private bool finished = false;

        private void Awake()
        {
            panel.SetActive(false);
        }

        private void UpdatePanelSize(ITween<Vector2> t)
        {
            panel.transform.localScale = t.CurrentValue;
        }

        private void ResizeCompleted(ITween<Vector2> t)
        {
            finished = true;
        }

        protected virtual IEnumerator StartingTween(RewardData rewardData, Player player)
        {
            panel.SetActive(true);
            finished = false;
            TweenFactory.Tween("PanelSize", Vector3.zero, Vector3.one, 1f, TweenScaleFunctions.CubicEaseIn, UpdatePanelSize, ResizeCompleted);
            while (!finished)
            {
                yield return null;
            }
        }

        protected virtual IEnumerator FinishingTween(RewardData rewardData, Player player)
        {
            panel.Tween("PanelSize", Vector3.one, Vector2.zero, 1f, TweenScaleFunctions.CubicEaseIn, UpdatePanelSize, ResizeCompleted);
            finished = false;
            while (!finished)
            {
                yield return null;
            }
            panel.SetActive(false);
        }

        public IEnumerator StartPanel(RewardData rewardData, Player player)
        {
            yield return StartingTween(rewardData, player);
            yield return ExecutePanelProcess(rewardData, player);
            yield return FinishingTween(rewardData, player);
        }

        protected abstract IEnumerator ExecutePanelProcess(RewardData rewardData, Player player);
    }
}