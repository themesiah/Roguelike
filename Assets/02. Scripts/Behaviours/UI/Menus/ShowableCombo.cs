using Laresistance.Core;
using UnityEngine;
using UnityEngine.UI;
using Laresistance.Battle;
using Laresistance.Data;
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

        public void SetupShowableElement(ShowableElement showableElement)
        {
            Combo combo = (Combo)showableElement;

            if (conditionsHolder != null)
            {
                for (int i = conditionsHolder.childCount - 1; i >= 0; --i)
                {
                    PoolInitializerBehaviour.GetPool("ComboCondition").FreeInstance(conditionsHolder.GetChild(i).gameObject);
                }
                BattleStatusManager dummyStatusManager = new BattleStatusManager(new CharacterHealth(1));
                foreach (AbilityConditionData acd in combo.comboCondition.conditionData.AbilityConditionDatas)
                {
                    GameObject go = PoolInitializerBehaviour.GetPool("ComboCondition").GetInstance(conditionsHolder);
                    go.transform.localScale = Vector3.one;
                    ShowableAbility showableAbility = go.GetComponent<ShowableAbility>();
                    BattleAbility conditionAbility = BattleAbilityFactory.GetBattleAbility(acd.AbilityDatas[0], new EquipmentsContainer(), dummyStatusManager);
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
                effectText.text = Texts.GetText(combo.comboAbility.GetShortAbilityText(combo.comboAbility.AbilityLevel));
            }
        }
    }
}