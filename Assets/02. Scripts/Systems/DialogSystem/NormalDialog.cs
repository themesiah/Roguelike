using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Systems.Dialog
{
    [System.Serializable]
    public class NormalDialog : DialogBase
    {
        public List<string> dialogTextRef;

        private int currentDialog = 0;
        private TextMeshProUGUI dialogTextMesh;

        public override void NextDialog()
        {
            continueSignal = true;
        }

        public override IEnumerator SetDialog(DialogVariablesStatus dialogVariablesStatus, TextMeshProUGUI dialogRef, TextMeshProUGUI questionRef, 
            TextMeshProUGUI answer1Ref, TextMeshProUGUI answer2Ref, Button answer1Button, Button answer2Button)
        {
            dialogTextMesh = dialogRef;
            dialogRef.gameObject.SetActive(true);
            questionRef.gameObject.SetActive(false);
            answer1Button.gameObject.SetActive(false);
            answer2Button.gameObject.SetActive(false);

            currentDialog = 0;
            yield return ShowDialog(currentDialog);
        }

        private IEnumerator ShowDialog(int current)
        {
            dialogTextMesh.text = Texts.GetText(dialogTextRef[current]);
            continueSignal = false;
            while (continueSignal == false)
            {
                yield return null;
            }
            currentDialog++;
            if (dialogTextRef.Count > currentDialog)
            {
                continueSignal = false;
                yield return ShowDialog(currentDialog);
            }
        }
    }
}