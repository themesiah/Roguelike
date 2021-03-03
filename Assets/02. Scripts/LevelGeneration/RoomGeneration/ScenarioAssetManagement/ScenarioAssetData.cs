using UnityEngine;

namespace Laresistance.LevelGeneration
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Scenario/Scenario Asset")]
    public class ScenarioAssetData : ScriptableObject
    {
        [SerializeField]
        [TextArea]
        private string assetDescription = default;
        public string AssetDescription { get { return assetDescription; } }

        [SerializeField]
        private GameObject[] prefabRightVersions = default;
        [SerializeField]
        private GameObject[] prefabLeftVersions = default;

        public GameObject[] GetScenarioAsset(bool right)
        {
            if (right && prefabRightVersions != null && prefabRightVersions.Length > 0)
            {
                return prefabRightVersions;
            }
            else if (!right && prefabLeftVersions != null && prefabLeftVersions.Length > 0)
            {
                return prefabLeftVersions;
            }
            else if (prefabRightVersions != null && prefabRightVersions.Length > 0)
            {
                return prefabRightVersions;
            }
            else if (prefabLeftVersions != null && prefabLeftVersions.Length > 0)
            {
                return prefabLeftVersions;
            }
            else
            {
                return null;
            }
        }

        public GameObject GetRandomScenarioAsset(bool right)
        {
            if (right && prefabRightVersions != null && prefabRightVersions.Length > 0)
            {
                return prefabRightVersions[Random.Range(0, prefabRightVersions.Length)];
            }
            else if (!right && prefabLeftVersions != null && prefabLeftVersions.Length > 0)
            {
                return prefabLeftVersions[Random.Range(0, prefabLeftVersions.Length)];
            }
            else if (prefabRightVersions != null && prefabRightVersions.Length > 0)
            {
                return prefabRightVersions[Random.Range(0, prefabRightVersions.Length)];
            }
            else if (prefabLeftVersions != null && prefabLeftVersions.Length > 0)
            {
                return prefabLeftVersions[Random.Range(0, prefabLeftVersions.Length)];
            }
            else
            {
                return null;
            }
        }
    }
}