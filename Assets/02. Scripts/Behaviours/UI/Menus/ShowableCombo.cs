﻿using Laresistance.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laresistance.Battle;
using Laresistance.Data;
using GamedevsToolbox.ScriptableArchitecture.Pools;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;

namespace Laresistance.Behaviours
{
    public class ShowableCombo : MonoBehaviour, IShowableGameElement
    {
        [SerializeField]
        private Transform conditionsHolder = default;
        [SerializeField]
        private ShowableAbility showableComboAbility = default;
        [SerializeField]
        private Text effectText = default;
        [SerializeField]
        private Sprite genericFrameGraphic = default;
        [SerializeField]
        private PoolInitializerBehaviour poolBehaviour = default;

        public void SetupShowableElement(ShowableElement showableElement)
        {
            Combo combo = (Combo)showableElement;

            if (conditionsHolder != null)
            {
                for (int i = conditionsHolder.childCount - 1; i >= 0; --i)
                {
                    Destroy(conditionsHolder.GetChild(i).gameObject);
                }
                BattleStatusManager dummyStatusManager = new BattleStatusManager(new CharacterHealth(1));
                foreach (AbilityConditionData acd in combo.comboCondition.conditionData.AbilityConditionDatas)
                {
                    GameObject go = PoolInitializerBehaviour.GetPool("ComboCondition").GetInstance(conditionsHolder);
                    go.transform.localScale = Vector3.one;
                    ShowableAbility showableAbility = go.GetComponent<ShowableAbility>();
                    BattleAbility conditionAbility = BattleAbilityFactory.GetBattleAbility(acd.AbilityDatas[0], null, dummyStatusManager);
                    showableAbility.SetupShowableElement(conditionAbility);
                    showableAbility.OverrideFrame(genericFrameGraphic);
                }
            }
            if (showableComboAbility != null)
            {
                showableComboAbility.SetupShowableElement(combo.comboAbility);
            }
            if (effectText != null)
            {
                effectText.text = Texts.GetText(combo.comboAbility.GetShortAbilityText(1));
            }
        }
    }
}