using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Systems.Dialog
{
    [System.Serializable]
    public class SingleDialogData
    {
        public bool normalDialog = true;
        public NormalDialog dialog = default;
        public QuestionDialog questionDialog = default;
        public List<DialogVariablesStatus.VariableData> RequiredVariables = default;
        public List<DialogVariablesStatus.VariableData> VariableChanges = default;


        public Vector2 gridPosition;

        public DialogBase Dialog => normalDialog ? (DialogBase)dialog : (DialogBase)questionDialog;
        public bool Continue()
        {
            if (dialog != null) 
            {
                return dialog.continueDialog;
            } else {
                return questionDialog.continueDialog;
            }
        }
    }
}