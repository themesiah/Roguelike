namespace Laresistance.Data
{
    public enum AbilityDataAISpecification
    {
        Always = 0,
        WhenAttacked,
        WhenNotFullLife,
        WhenSelfHaveDebuff,
        WhenEnemyHaveBuff,
        WhenAllyAttacked,
        PrepareBlock,
        PrepareParry
    }
}