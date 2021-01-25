using Laresistance.Core;

namespace Laresistance.Battle {
    public interface ITargetDependant
    {
        void SetSelectedTarget(CharacterBattleManager cbm);
        CharacterBattleManager GetSelectedTarget();
    }
}