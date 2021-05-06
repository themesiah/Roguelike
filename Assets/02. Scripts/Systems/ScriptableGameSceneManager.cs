using Laresistance.Behaviours;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Laresistance.Systems
{
    [CreateAssetMenu(menuName = "Laresistance/Systems/Game Scene Manager")]
    public class ScriptableGameSceneManager : ScriptableObject
    {
        [SerializeField]
        private AssetReference sceneReference = default;

        public void RestartGame()
        {
            GameSceneManager gsm = GameSceneManager.Instance;
            gsm.ChangeScene(sceneReference);
        }
    }
}