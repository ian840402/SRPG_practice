namespace SRPGPractice.Core;

public sealed record UnitClassDefinition(
    UnitClass UnitClass,
    string DisplayName,
    int MaxHp,
    int MaxMp,
    int AttackPower,
    int Defense,
    int HitCoefficient,
    int Evasion,
    int MoveRange,
    int CriticalRate,
    AttackRange NormalAttackRange);
