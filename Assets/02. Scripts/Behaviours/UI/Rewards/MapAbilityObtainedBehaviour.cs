using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class MapAbilityObtainedBehaviour : RewardPanelBehaviour
    {
        [SerializeField]
        private LocalizedStringTextBehaviour nameTextRef = default;
        [SerializeField]
        private Text descriptionTextRef = default;
        [SerializeField]
        private Image abilityIcon = default;

        private bool inputDone = false;

        public void AnyInput(InputAction.CallbackContext context)
        {
            inputDone = true;
        }

        protected override IEnumerator StartingTween(RewardData rewardData)
        {
            nameTextRef.ChangeVariable(Texts.GetText(rewardData.mapAbilityData.AbilityName));
            descriptionTextRef.text = Texts.GetText(rewardData.mapAbilityData.AbilityDescriptionId);
            abilityIcon.sprite = rewardData.mapAbilityData.AbilitySpriteRef;
            yield return base.StartingTween(rewardData);
        }

        protected override IEnumerator ExecutePanelProcess(RewardData rewardData)
        {
            inputDone = false;
            while (!inputDone)
            {
                yield return null;
            }
        }

        public override RewardUIType RewardType => RewardUIType.MapAbility;
    }
}