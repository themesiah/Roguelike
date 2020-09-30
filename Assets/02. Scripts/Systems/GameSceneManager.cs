using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Laresistance.Behaviours
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField]
        private string loadingScene = default;

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

        public void ChangeScene(string targetScene)
        {
            StartCoroutine(SceneLoadAndChange(targetScene));
        }

        public IEnumerator SceneLoadAndChange(string targetScene)
        {
            if (!currentlyLoadingScene)
            {
                currentlyLoadingScene = true;
                SceneManager.LoadScene(loadingScene, LoadSceneMode.Single);
                var newSceneOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
                while (!newSceneOperation.isDone)
                {
                    yield return null;
                }
                var unloadLoadingSceneOperation = SceneManager.UnloadSceneAsync(loadingScene);
                while (!unloadLoadingSceneOperation.isDone)
                {
                    yield return null;
                }
                currentlyLoadingScene = false;
            }
        }
    }
}