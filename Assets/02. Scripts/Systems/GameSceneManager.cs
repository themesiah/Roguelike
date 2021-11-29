using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using UnityEngine.AddressableAssets;

namespace Laresistance.Behaviours
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField]
        private AssetReference loadingScene = default;
        [SerializeField]
        private AssetReference blankScene = default;

        private static GameSceneManager instance = null;
        private bool currentlyLoadingScene = false;
        private bool blankSceneLoaded = false;
        private AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> blankSceneOp;

        private bool sceneFinishedLoading = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public static GameSceneManager Instance { get { return instance; } }

        public void ChangeScene(AssetReference targetScene)
        {
            StartCoroutine(SceneLoadAndChange(targetScene));
        }

        public void SceneFinishedLoading()
        {
            if (currentlyLoadingScene)
                sceneFinishedLoading = true;
        }

        public IEnumerator SceneLoadAndChange(AssetReference targetScene)
        {
            if (!currentlyLoadingScene)
            {
                currentlyLoadingScene = true;
                if (blankSceneLoaded)
                {
                    yield return Addressables.UnloadSceneAsync(blankSceneOp);
                    blankSceneLoaded = false;
                }
                blankSceneOp = blankScene.LoadSceneAsync(LoadSceneMode.Single);
                yield return blankSceneOp;
                blankSceneLoaded = true;
                var loadingSceneOp = loadingScene.LoadSceneAsync(LoadSceneMode.Additive);
                yield return loadingSceneOp;
                var newSceneOp = targetScene.LoadSceneAsync(LoadSceneMode.Additive);
                yield return newSceneOp;
                Debug.LogFormat("Loaded scene with name {0}", newSceneOp.Result.Scene.name);
                while (sceneFinishedLoading == false)
                {
                    yield return null;
                }
                sceneFinishedLoading = false;
                yield return Addressables.UnloadSceneAsync(loadingSceneOp);

                currentlyLoadingScene = false;
            }
        }
    }
}