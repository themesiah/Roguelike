using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Systems.Dialog
{
    [System.Serializable]
    public class SingleDialogData
    {
        [SerializeField]
        private bool normalDialog = true;
        [SerializeField]
        private NormalDialog dialog = default;
        [SerializeField]
        private QuestionDialog questionDialog = default;
        [SerializeField]
        private List<DialogVariablesStatus.VariableData> requiredVariables = default;
        [SerializeField]
        private List<DialogVariablesStatus.VariableData> variableChanges = default;

        public DialogBase Dialog => normalDialog ? (DialogBase)dialog : (DialogBase)questionDialog;
        public DialogVariablesStatus.VariableData[] RequiredVariables => requiredVariables.ToArray();
        public DialogVariablesStatus.VariableData[] VariableChanges => variableChanges.ToArray();
    }
}