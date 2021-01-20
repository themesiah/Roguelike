using UnityEngine;
using Laresistance.Data;

namespace Laresistance.Behaviours
{
    public class InfoUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private ShowablePlayer[] showablePlayers = default;
        private void OnEnable()
        {
            foreach (ShowablePlayer showablePlayer in showablePlayers)
            {
                showablePlayer.SetupShowableElement(playerDataRef.Get().player);
            }
        }
    }
}