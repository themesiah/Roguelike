using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Selectors;
using UnityEngine.AddressableAssets;

namespace Laresistance.LevelGeneration
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Scenario/Biome")]
    public class RoomBiome : ScriptableObject
    {
        [SerializeField]
        [TextArea]
        private string biomeDescription = default;
        public string BiomeDescription { get { return biomeDescription; } }

        [SerializeField]
        private ScriptableAssetReferenceSelector normalEnemies = default;
        public ScriptableAssetReferenceSelector NormalEnemies { get { return normalEnemies; } }

        [SerializeField]
        private ScriptableAssetReferenceSelector minibossEnemies = default;
        public ScriptableAssetReferenceSelector MinibossEnemies { get { return minibossEnemies; } }

        [SerializeField]
        private AssetReference bossSceneRef = default;
        public AssetReference BossSceneRef { get { return bossSceneRef; } }
    }
}