using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Laresistance.Management
{
    [DefaultExecutionOrder(-10000)]
    public class MonoServiceLocator : MonoBehaviour
    {
        private static MonoServiceLocator Instance;

        [SerializeField]
        private AssetReference serviceLocatorReference = default;

        private ScriptableServiceLocator serviceLocator;
        private AsyncOperationHandle operationHandle;


        private void Awake()
        {
            Instance = this;
            operationHandle = serviceLocatorReference.LoadAssetAsync<ScriptableServiceLocator>();
            operationHandle.Completed += (handle) =>
            {
                serviceLocator = handle.Result as ScriptableServiceLocator;
            };
        }

        public static void GetServiceAsync<T>(string key, UnityAction<T> action) where T : ScriptableObject
        {
            if (Instance == null)
                return;
            Instance.GetServiceAsyncInstanced(key, action);
        }

        public void GetServiceAsyncInstanced<T>(string key, UnityAction<T> action) where T : ScriptableObject
        {
            if (operationHandle.IsDone)
            {
                T service = serviceLocator.GetService<T>(key);
                action?.Invoke(service);
            } else
            {
                operationHandle.Completed += (handle) =>
                {
                    T service = serviceLocator.GetService<T>(key);
                    action?.Invoke(service);
                };
            }
        }

        public static T GetServiceSync<T>(string key) where T : ScriptableObject
        {
            if (Instance == null)
                return null;
            return Instance.GetServiceSyncInstanced<T>(key);
        }

        public T GetServiceSyncInstanced<T>(string key) where T : ScriptableObject
        {
            if (serviceLocator == null)
                return null;
            return serviceLocator.GetService<T>(key);
        }
    }
}