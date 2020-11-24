namespace Laresistance.Battle
{
    public interface IAbilityInputProcessor
    {
        int GetAbilityToExecute(BattleStatusManager battleStatus, float delta);
        void ResetAbilities();
        BattleAbility[] GetAbilities();
    }
}