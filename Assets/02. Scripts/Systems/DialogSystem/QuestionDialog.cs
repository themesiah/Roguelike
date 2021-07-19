using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Systems.Dialog
{
    [System.Serializable]
    public class QuestionDialog : DialogBase
    {
        public string dialogQuestionRef;
        public string answer1TextRef = default;
        public string answer2TextRef = default;
        public List<DialogVariablesStatus.VariableData> variablesChange = default;

        public override void NextDialog()
        {
            // Don't set continueSignal to true. It is controlled by the option buttons.
        }

        public override IEnumerator SetDialog(DialogVariablesStatus dialogVariablesStatus, TextMeshProUGUI dialogRef, TextMeshProUGUI questionRef, 
            TextMeshProUGUI answer1Ref, TextMeshProUGUI answer2Ref, Button answer1Button, Button answer2Button)
        {
            continueSignal = false;
            questionRef.text = Texts.GetText(dialogQuestionRef);
            answer1Ref.text = Texts.GetText(answer1TextRef);
            answer2Ref.text = Texts.GetText(answer2TextRef);
            dialogRef.gameObject.SetActive(false);
            questionRef.gameObject.SetActive(true);
            answer1Button.gameObject.SetActive(true);
            answer2Button.gameObject.SetActive(true);

            answer1Button.onClick.RemoveAllListeners();
            answer2Button.onClick.RemoveAllListeners();

            answer1Button.onClick.AddListener(() => {
                dialogVariablesStatus.SetVariable(variablesChange[0].name, variablesChange[0].value);
                continueSignal = true;
            });

            answer2Button.onClick.AddListener(() => {
                dialogVariablesStatus.SetVariable(variablesChange[1].name, variablesChange[1].value);
                continueSignal = true;
            });

            while (continueSignal == false)
            {
                yield return null;
            }
        }
    }
}