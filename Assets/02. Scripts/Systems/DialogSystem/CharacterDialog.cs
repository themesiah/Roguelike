using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Systems.Dialog
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Character Dialog")]
    public class CharacterDialog : ScriptableObject
    {
        [SerializeField]
        private List<SingleDialogData> listOfDialogs = default;
        [SerializeField]
        private string characterNameRef = default;
        [SerializeField]
        private Sprite characterPortrait = default;

        public SingleDialogData[] ListOfDialogs => listOfDialogs.ToArray();
        public string CharacterNameRef => characterNameRef;
        public Sprite CharacterPortrait => characterPortrait;
    }
}