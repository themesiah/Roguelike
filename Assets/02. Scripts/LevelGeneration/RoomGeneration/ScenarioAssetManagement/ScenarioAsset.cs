namespace Laresistance.LevelGeneration
{
    public struct ScenarioAsset
    {
        public ScenarioAssetData scenarioAssetData;
        public bool right;

        public ScenarioAsset(ScenarioAssetData scenarioAssetData, bool right)
        {
            this.scenarioAssetData = scenarioAssetData;
            this.right = right;
        }
    }
}