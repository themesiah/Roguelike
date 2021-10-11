using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class BlockParryAnimatorReceiver : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnBlockReceived = default;
        [SerializeField]
        private UnityEvent OnParryReceived = default;

        public void BlockReceiver()
        {
            OnBlockReceived?.Invoke();
        }

        public void ParryReceiver()
        {
            OnParryReceived?.Invoke();
        }
    }
}