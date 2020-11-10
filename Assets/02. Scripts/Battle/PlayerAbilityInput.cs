using Laresistance.Core;

namespace Laresistance.Battle
{
    public class PlayerAbilityInput : IAbilityInputProcessor
    {
        private Player player;

        private int currentAbilityIndex = -1;

        public PlayerAbilityInput(Player player)
        {
            this.player = player;
        }

        public int GetAbilityToExecute(BattleStatusManager battleStatus, float delta)
        {
            foreach(var ability in player.GetAbilities())
            {
                ability?.Tick(delta);
            }
            int temp = currentAbilityIndex;
            currentAbilityIndex = -1;
            return temp;
        }

        public void TryToExecuteAbility(int index)
        {
            if (index < -1 || index > 6)
                throw new System.Exception("Invalid index trying to execute ability. It must be 0 (character ability), 1,2,3 (minion abilities) or 4,5,6 (consumables)");
            var ability = player.GetAbilities()[index];
            if (ability != null)
            {
                if (ability.CanBeUsed())
                {
                    currentAbilityIndex = index;
                }
            }
        }

        public void ResetAbilities()
        {
            foreach(var ability in player.GetAbilities())
            {
                ability?.ResetTimer();
            }
        }
    }
}