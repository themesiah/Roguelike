using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Laresistance.Management
{
    public class AddressableGroupLoader : MonoBehaviour
    {
        [SerializeField]
        private string groupName = "";

        [SerializeField]
        private bool loadOnStart = false;

        private void Start()
        {
            if (loadOnStart)
            {
                LoadGroup();
            }
        }

        public void LoadGroup()
        {
            StartCoroutine(LoadGroupCoroutine());
        }

        public IEnumerator LoadGroupCoroutine()
        {
            yield return Addressables.LoadAssetsAsync<GameObject>(groupName, (go) => {
                Debug.LogFormat("{0} loaded!", go.name);
            });
        }
    }
}