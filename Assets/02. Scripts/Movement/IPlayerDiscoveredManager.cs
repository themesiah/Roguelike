using GamedevsToolbox.Utils;

namespace Laresistance.Movement {
    public interface IPlayerDiscoveredManager : IPausable
    {
        void Tick(float delta);
    }
}