using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Laresistance.Battle
{
    public class StatusEffectFactory
    {
        private static Dictionary<StatusType, Type> statusByType;

        private static bool IsInitialized => statusByType != null;

        private static void InitializeFactory()
        {
            if (IsInitialized)
                return;

            var statusTypes = Assembly.GetAssembly(typeof(StatusEffect)).GetTypes().
                Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(StatusEffect)));

            statusByType = new Dictionary<StatusType, Type>();

            foreach (var type in statusTypes)
            {
                var tempEffect = Activator.CreateInstance(type, args: new object[] { null }) as StatusEffect;
                statusByType.Add(tempEffect.StatusType, type);
            }
        }

        public static StatusEffect GetStatusEffect(StatusType statusType, BattleStatusManager statusManager)
        {
            InitializeFactory();

            if (statusByType.ContainsKey(statusType))
            {
                Type type = statusByType[statusType];
                var effect = Activator.CreateInstance(type, args: new object[] { statusManager }) as StatusEffect;
                return effect;
            }

            throw new Exception("Battle effect with type " + statusType.ToString() + " is not implemented");
        }
    }
}