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
            for (int i = 0; i < player.GetMinions().Length; ++i)
            {
                if (player.GetAbilities()[i] != null)
                    player.GetAbilities()[i].Tick(delta * battleStatus.GetSpeedModifier());
            }
            int temp = currentAbilityIndex;
            currentAbilityIndex = -1;
            return temp;
        }

        public void TryToExecuteAbility(int index)
        {
            if (index < -1 || index > 6)
                throw new System.Exception("Invalid index trying to execute ability. It must be 0 (character ability), 1,2,3 (minion abilities) or 4,5,6 (consumables)");
            if (player.GetMinions().Length < index)
            {
                // Nothing for now
                return;
            }
            if (player.GetAbilities()[index] != null && player.GetAbilities()[index].CanBeUsed())
            {
                currentAbilityIndex = index;
            }
        }
    }
}