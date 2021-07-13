using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Events;

namespace Laresistance.Systems.Dialog
{
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField]
        private CharacterDialogEvent characterDialogEvent = default;
        [SerializeField]
        private GameObject dialogPanel = default;
        [SerializeField]
        private StringGameEvent gameContextEventSignal = default;
        [SerializeField]
        private DialogVariablesStatus dialogVariablesStatus = default;
        [SerializeField]
        private TextMeshProUGUI dialogNameRef = default;
        [SerializeField]
        private TextMeshProUGUI dialogRef = default;
        [SerializeField]
        private TextMeshProUGUI questionRef = default;
        [SerializeField]
        private TextMeshProUGUI answer1Ref = default;
        [SerializeField]
        private TextMeshProUGUI answer2Ref = default;
        [SerializeField]
        private Button answer1Button = default;
        [SerializeField]
        private Button answer2Button = default;
        [SerializeField]
        private Image portraitRef = default;

        private SingleDialogData currentDialogData = null;

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        public void Register()
        {
            characterDialogEvent?.RegisterListener(this);
        }

        public void Unregister()
        {
            characterDialogEvent?.UnregisterListener(this);
        }

        public virtual IEnumerator OnEventRaised(CharacterDialog data)
        {
            yield return StartDialogCoroutine(data);
        }

        //public void StartDialog(CharacterDialog characterDialog)
        //{
        //    StartCoroutine(StartDialogCoroutine(characterDialog));
        //}

        public IEnumerator StartDialogCoroutine(CharacterDialog characterDialog) {
            // Activate UI
            dialogPanel.SetActive(true);
            gameContextEventSignal?.Raise("UI");

            // Search the single dialog data depending on variables (all must be equal)
            SingleDialogData singleDialogData = null;
            foreach(var singleDialog in characterDialog.ListOfDialogs)
            {
                bool found = true;
                foreach(var requiredVariable in singleDialog.RequiredVariables)
                {
                    if (dialogVariablesStatus.GetVariable(requiredVariable.name) != requiredVariable.value)
                    {
                        found = false;
                        break;
                    }
                }
                if (found == true)
                {
                    singleDialogData = singleDialog;
                    break;
                }
            }
            UnityEngine.Assertions.Assert.IsNotNull(singleDialogData);
            currentDialogData = singleDialogData;

            // Change base dialog data (name and portrait)
            dialogNameRef.text = Texts.GetText(characterDialog.CharacterNameRef);
            portraitRef.sprite = characterDialog.CharacterPortrait;
            // Show the dialog and wait until signal
            yield return singleDialogData.Dialog.SetDialog(dialogVariablesStatus, dialogRef, questionRef, answer1Ref, answer2Ref, answer1Button, answer2Button);
            foreach(var variableData in singleDialogData.VariableChanges)
            {
                dialogVariablesStatus.SetVariable(variableData.name, variableData.value);
            }
            // Deactivate UI
            dialogPanel.SetActive(false);
            gameContextEventSignal?.Raise("Map");
            yield return null;
            yield return null;
            yield return null;
        }

        // Next is called from outside. It's the signal to advance in the dialog, being it showing the next text, finishing or advancing to the next one.
        public void Next()
        {
            currentDialogData.Dialog.NextDialog();
        }
    }
}