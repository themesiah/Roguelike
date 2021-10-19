namespace Laresistance.Data
{
    [System.Serializable]
    public enum EffectType
    {
        Damage,
        Heal,
        Shield,
        DamageOverTime,
        Speed,
        Stun,
        ImproveDamage,
        Cure,
        AdvanceCooldowns, // Unused, now obtain energy
        DamageModification,
        ObtainEnergy,
        BasicAttack,
        RemoveBuffs,
        LifeSiphon,
        Blind,
        Retaliation,
        Barrier,
        Exchange,
        Regeneration,
        PercentDamage,
        PrepareParry,
        PrepareBlock,
        Shuffle,
        BodyBlock,
        Rush
    }
}