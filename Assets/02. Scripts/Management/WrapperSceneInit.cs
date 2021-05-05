using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WrapperSceneInit : MonoBehaviour
{
    [SerializeField]
    private AssetReference sceneRef = default;

    private IEnumerator Start()
    {
        yield return Addressables.LoadSceneAsync(sceneRef, UnityEngine.SceneManagement.LoadSceneMode.Single, true);
    }
}
