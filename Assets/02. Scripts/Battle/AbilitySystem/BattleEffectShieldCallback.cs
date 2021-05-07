﻿using UnityEngine.Events;

namespace Laresistance.Battle
{
    public class BattleEffectShieldCallback : BattleEffectCallback
    {
        public override void ConfigureBattleEffectCallback(BattleStatusManager target, UnityAction callback, float delay)
        {
            base.ConfigureBattleEffectCallback(target, callback, delay);
            target.health.OnShieldsChanged += DestroyShield;
            target.OnResetStatus += OnBattleEnd;
            if (!BattleAbilityManager.Instance.Battling)
            {
                OnBattleEnd(target);
            }
        }

        private void DestroyShield(CharacterHealth sender, int delta, int totalShields, bool isDamage)
        {
            sender.OnShieldsChanged -= DestroyShield;
            target.OnResetStatus -= OnBattleEnd;
            Destroy(gameObject);
        }

        private void OnBattleEnd(BattleStatusManager manager)
        {
            manager.health.OnShieldsChanged -= DestroyShield;
            target.OnResetStatus -= OnBattleEnd;
            Destroy(gameObject);
        }
    }
}