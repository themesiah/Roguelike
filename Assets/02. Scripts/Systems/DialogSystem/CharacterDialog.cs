using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Systems.Dialog
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Character Dialog")]
    public class CharacterDialog : ScriptableObject
    {
        public List<SingleDialogData> ListOfDialogs = default;
        [SerializeField]
        private string characterNameRef = default;
        [SerializeField]
        private Sprite characterPortrait = default;
        public string CharacterNameRef => characterNameRef;
        public Sprite CharacterPortrait => characterPortrait;
    }
}