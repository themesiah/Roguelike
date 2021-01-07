using Laresistance.Behaviours;
using System.Collections;
using UnityEngine;

namespace Laresistance.Battle
{
    /*
     * This class should only be used in the battle simulator.
     */
    public class PlayerSimulatorAbilityInput : IAbilityInputProcessor
    {
        private BattleAbility[] abilities;

        public PlayerSimulatorAbilityInput(BattleAbility[] abilities)
        {
            this.abilities = abilities;
        }

        public BattleAbility[] GetAbilities()
        {
            return abilities;
        }

        public int GetAbilityToExecute(BattleStatusManager battleStatus, float delta)
        {
            for (int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == null)
                    continue;
                abilities[i].Tick(delta);
            }
            for (int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == null)
                    continue;
                if (abilities[i].CanBeUsed())
                {
                    if (abilities[i].IsShieldAbility)
                    {
                        BattleAbility currentAbility = BattleAbilityManager.currentAbility;
                        if (currentAbility == null || (currentAbility.GetStatusManager() != battleStatus && currentAbility.IsOffensiveAbility))
                        {
                            return i;
                        }
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public void BattleStart()
        {

        }

        public void BattleEnd()
        {

        }
    }
}