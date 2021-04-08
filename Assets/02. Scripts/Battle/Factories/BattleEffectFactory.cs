using Laresistance.Data;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Laresistance.Battle
{
    public class BattleEffectFactory
    {
        private static Dictionary<EffectType, Type> effectByType;
        private static bool IsInitialized => effectByType != null;

        private static void InitializeFactory()
        {
            if (IsInitialized)
                return;

            var effectTypes = Assembly.GetAssembly(typeof(BattleEffect)).GetTypes().
                Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BattleEffect)));

            effectByType = new Dictionary<EffectType, Type>();

            foreach (var type in effectTypes)
            {
                var tempEffect = Activator.CreateInstance(type, args:new object[] { 1, EffectTargetType.Self, new BattleStatusManager(new CharacterHealth(1)), null}) as BattleEffect;
                effectByType.Add(tempEffect.EffectType, type);
            }
        }

        public static BattleEffect GetBattleEffect(EffectData effectData, BattleStatusManager battleStatus)
        {
            InitializeFactory();

            if (effectByType.ContainsKey(effectData.EffectType))
            {
                Type effectType = effectByType[effectData.EffectType];
                var effect = Activator.CreateInstance(effectType, args:new object[] { effectData.Power, effectData.TargetType, battleStatus, effectData }) as BattleEffect;
                return effect;
            }

            throw new Exception("Battle effect with type " + effectData.EffectType.ToString() + " is not implemented");
        }
    }
}