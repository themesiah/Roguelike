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

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public static GameSceneManager Instance { get { return instance; } }

        public void ChangeScene(AssetReference targetScene)
        {
            StartCoroutine(SceneLoadAndChange(targetScene));
        }

        public IEnumerator SceneLoadAndChange(AssetReference targetScene)
        {
            if (!currentlyLoadingScene)
            {
                currentlyLoadingScene = true;

                var blankSceneOp = blankScene.LoadSceneAsync(LoadSceneMode.Single);
                yield return blankSceneOp;
                var loadingSceneOp = loadingScene.LoadSceneAsync(LoadSceneMode.Additive);
                yield return loadingSceneOp;
                var newSceneOp = targetScene.LoadSceneAsync(LoadSceneMode.Additive, true);
                yield return newSceneOp;
                yield return Addressables.UnloadSceneAsync(loadingSceneOp);

                currentlyLoadingScene = false;
            }
        }
    }
}