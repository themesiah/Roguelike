using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Laresistance.Management
{
    public class AddressableTagLoader<T> : MonoBehaviour where T : Object
    {
        [SerializeField]
        private string tagName = "";

        [SerializeField]
        private bool loadOnStart = false;

        [SerializeField]
        private bool unloadOnDestroy = false;

        [Header("DEBUG (Only editor)")]
        [SerializeField]
        private InputAction loadAction = default;
        [SerializeField]
        private InputAction unloadAction = default;
        [SerializeField]
        private bool debugLog = false;

        private List<T> loadedAssets;
        private AsyncOperationHandle operation;

        private void Start()
        {
            loadedAssets = new List<T>();
            if (loadOnStart)
            {
                LoadTag();
            }
#if UNITY_EDITOR
            loadAction.Enable();
            unloadAction.Enable();
#endif
        }

        private void OnDestroy()
        {
            if (unloadOnDestroy)
            {
                UnloadTag();
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (loadAction.triggered)
            {
                if (debugLog)
                    Debug.LogFormat("Doing loading action for tag {0}", tagName);
                LoadTag();
            }

            if (unloadAction.triggered)
            {
                if (debugLog)
                    Debug.LogFormat("Doing unloading action for tag {0}", tagName);
                UnloadTag();
            }
        }
#endif

        public void LoadTag()
        {
            StartCoroutine(LoadTagCoroutine());
        }

        private IEnumerator LoadTagCoroutine()
        {
            operation = Addressables.LoadAssetsAsync<T>(tagName, (obj) =>
            {
                if (debugLog)
                    Debug.LogFormat("{0} loaded!", obj.name);
                loadedAssets.Add(obj);
            });

            yield return operation;
        }

        public void UnloadTag()
        {
            Addressables.Release(operation);
        }
    }
}