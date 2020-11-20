using GamedevsToolbox.Utils;

namespace Laresistance.Movement
{
    public interface IMovementManager : IPausable
    {
        void Tick(float delta);
        void Turn();
        void Turn(bool right);
    }
}