namespace Laresistance.Core
{
    public class EquipmentEvents
    {
        public delegate void OnGetPowerHandler(ref int currentPower);
        public event OnGetPowerHandler OnGetPower;
        public void InvokeOnGetPower(ref int power){OnGetPower?.Invoke(ref power);}
        public delegate void OnGetCooldownHandler(ref float currentCooldown);
        public event OnGetCooldownHandler OnGetCooldown;
        public void InvokeOnGetCooldown(ref float cooldown){ OnGetCooldown?.Invoke(ref cooldown);}
    }
}