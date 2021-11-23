using Laresistance.Behaviours;
using System.Collections;
using System.Collections.Generic;
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

        public AbilityExecutionData GetAbilitiesToExecute(BattleStatusManager battleStatus, float delta, float unmodifiedDelta)
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
                        BattleAbility currentAbility = BattleAbilityManager.Instance.currentAbility;
                        if (currentAbility == null || (currentAbility.GetStatusManager() != battleStatus && currentAbility.IsOffensiveAbility))
                        {
                            return new AbilityExecutionData() { index = i, selectedTarget = null }; ;
                        }
                    }
                    else
                    {
                        return new AbilityExecutionData() { index = i, selectedTarget = null }; ;
                    }
                }
            }
            return new AbilityExecutionData() { index = -1, selectedTarget = null }; ;
        }

        public void BattleStart()
        {

        }

        public void BattleEnd()
        {

        }
        public void PerformTimeStop(bool activate)
        {
        }

    }
}