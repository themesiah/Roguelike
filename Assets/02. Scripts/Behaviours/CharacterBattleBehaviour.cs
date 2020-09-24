using UnityEngine;
using Laresistance.Battle;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public abstract class CharacterBattleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 100; // This will be a reference to a scriptable object data

        public CharacterHealth CharacterHealth { get; protected set; }
        public BattleStatusManager StatusManager { get; protected set; }
        public IAbilityInputProcessor AbilityInputProcessor { get; protected set; }
        public IAbilityExecutor AbilityExecutor { get; protected set; }


        protected virtual void Start()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(maxHealth));
        }

        protected virtual void Update()
        {
            StatusManager.ProcessStatus();
            int index = AbilityInputProcessor.GetAbilityToExecute(StatusManager);
            if (index != -1)
            {
                AbilityExecutor.ExecuteAbility(index, StatusManager, GetEnemies());
            }
        }

        protected abstract BattleStatusManager[] GetEnemies(); // First is selected enemy.
    }
}