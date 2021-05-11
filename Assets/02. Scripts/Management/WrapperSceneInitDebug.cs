using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WrapperSceneInitDebug : MonoBehaviour
{
    [SerializeField]
    private AssetReference sceneRef = default;
    [SerializeField]
    private AssetReference sceneRef2 = default;

    private IEnumerator Start()
    {
        if (!string.IsNullOrEmpty(sceneRef.RuntimeKey.ToString()))
            yield return Addressables.LoadSceneAsync(sceneRef, UnityEngine.SceneManagement.LoadSceneMode.Additive, true);
        if (!string.IsNullOrEmpty(sceneRef2.RuntimeKey.ToString()))
            yield return Addressables.LoadSceneAsync(sceneRef2, UnityEngine.SceneManagement.LoadSceneMode.Additive, true);
    }
}
