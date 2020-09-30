namespace Laresistance.Battle
{
    public interface IAbilityInputProcessor
    {
        int GetAbilityToExecute(BattleStatusManager battleStatus, float delta);
    }
}