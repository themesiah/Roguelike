namespace Laresistance.Battle
{
    public class BattleStatusManager
    {
        public CharacterHealth health { get; private set; }
        // Speed modifiers
        // Damage, heal and shield modifiers
        // Damage over time

        public BattleStatusManager(CharacterHealth health)
        {
            this.health = health;
        }

        public void ProcessStatus()
        {
            // Process damage over time
        }
    }
}