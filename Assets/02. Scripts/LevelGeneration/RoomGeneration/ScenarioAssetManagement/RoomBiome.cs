using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private GameObject[] normalEnemies = default;
        public GameObject[] NormalEnemies { get { return normalEnemies; } }

        [SerializeField]
        private GameObject[] minibossEnemies = default;
        public GameObject[] MinibossEnemies { get { return minibossEnemies; } }
    }
}