using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Pools;
using Laresistance.Battle;

namespace Laresistance.Behaviours
{
    public class StatusIconManager : MonoBehaviour
    {
        [SerializeField]
        private Image frameImage = default;
        [SerializeField]
        private Image statusIcon = default;

        private StatusIconType statusType;
        private float timer;
        private float duration;
        private ScriptablePool pool;
        private BattleStatusManager statusManager;

        public delegate void OnStatusTerminatedHandler(StatusIconManager sender, StatusIconType statusType);
        public event OnStatusTerminatedHandler OnStatusTerminated;
        public StatusIconType GetStatusType => statusType;

        public void InitStatusIcon(StatusIconType statusType, Sprite frameSprite, Sprite iconSprite, Color frameColor, float duration, BattleStatusManager statusManager, ScriptablePool pool)
        {
            frameImage.sprite = frameSprite;
            frameImage.color = frameColor;
            statusIcon.sprite = iconSprite;

            timer = duration;
            this.statusType = statusType;
            this.duration = duration;
            this.pool = pool;
            this.statusManager = statusManager;
            frameImage.fillAmount = 1f;

            SuscribeToStatusManager(statusManager);
        }

        private void SuscribeToStatusManager(BattleStatusManager statusManager)
        {
            if (statusType == StatusIconType.Shield)
            {
                statusManager.health.OnShieldsChanged += OnShieldsChanged;
            } else
            {
                statusManager.OnTick += OnTick;
            }
            
            statusManager.OnResetStatus += OnResetStatus;
        }

        private void UnsuscribeFromStatusManager(BattleStatusManager statusManager)
        {
            if (statusType == StatusIconType.Shield)
            {
                statusManager.health.OnShieldsChanged -= OnShieldsChanged;
            } else
            {
                statusManager.OnTick -= OnTick;
            }
            
            statusManager.OnResetStatus -= OnResetStatus;
        }

        private void OnTick(BattleStatusManager statusManager, float delta)
        {
            if (duration < 0f)
            {
                // Infinite duration
            }
            else
            {
                timer -= delta;
                frameImage.fillAmount = timer / duration;
                if (timer <= 0f)
                {
                    DeactivateIndicator();
                }
            }
        }

        private void OnResetStatus(BattleStatusManager statusManager)
        {
            DeactivateIndicator();
        }

        private void OnShieldsChanged(CharacterHealth sender, int delta, int total, bool isDamage, float shieldPercent)
        {
            if (total <= 0)
            {
                DeactivateIndicator();
            }
        }

        private void DeactivateIndicator()
        {
            OnStatusTerminated?.Invoke(this, statusType);
            UnsuscribeFromStatusManager(statusManager);
            pool.FreeInstance(gameObject);
        }

        public void ManualTermination()
        {
            DeactivateIndicator();
        }
    }
}