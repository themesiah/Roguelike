using Laresistance.Core;

namespace Laresistance.Battle
{
    public class PlayerAbilityInput : IAbilityInputProcessor
    {
        private Player player;

        public PlayerAbilityInput(Player player)
        {
            this.player = player;
        }

        public int GetAbilityToExecute(BattleStatusManager battleStatus, float delta)
        {
            return -1;
        }
    }
}