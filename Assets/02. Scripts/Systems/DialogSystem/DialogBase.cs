using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace Laresistance.Systems.Dialog
{
    [System.Serializable]
    public abstract class DialogBase
    {
        protected bool continueSignal = false;

        public abstract IEnumerator SetDialog(DialogVariablesStatus dialogVariablesStatus, TextMeshProUGUI dialogRef, TextMeshProUGUI questionRef, 
            TextMeshProUGUI answer1Ref, TextMeshProUGUI answer2Ref, Button answer1Button, Button answer2Button);

        public abstract void NextDialog();
    }
}