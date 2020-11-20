using GamedevsToolbox.Utils;
using System.Collections;

namespace Laresistance.Behaviours
{
    public interface IBattleAnimator : IPausable
    {
        IEnumerator PlayAnimation(string trigger);
        void Stop();
    }
}