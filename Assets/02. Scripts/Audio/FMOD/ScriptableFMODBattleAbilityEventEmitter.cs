using UnityEngine;

namespace Laresistance.Audio
{
    [CreateAssetMenu(menuName = "Laresistance/Audio/FMOD Battle Ability Event Emitter")]
    public class ScriptableFMODBattleAbilityEventEmitter : ScriptableFMODEventEmitter
    {
        private int currentPotency;

        public void Play(int potency)
        {
            currentPotency = potency;
            Play();
        }

        protected override void SetParameters()
        {
            instance.setParameterByID(paramDescription[0].id, (float)currentPotency);
        }
    }
}