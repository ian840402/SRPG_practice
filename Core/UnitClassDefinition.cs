public sealed class UnitClassDefinition
{
  public UnitClassDefinition(
      UnitClass unitClass,
      string displayName,
      int maxHp,
      int maxMp,
      int attackPower,
      int defense,
      int hitCoefficient,
      int evasion,
      int moveRange,
      int criticalRate,
      AttackRange normalAttackRange)
  {
    UnitClass = unitClass;
    DisplayName = displayName;
    MaxHp = maxHp;
    MaxMp = maxMp;
    AttackPower = attackPower;
    Defense = defense;
    HitCoefficient = hitCoefficient;
    Evasion = evasion;
    MoveRange = moveRange;
    CriticalRate = criticalRate;
    NormalAttackRange = normalAttackRange;
  }

  public UnitClass UnitClass { get; }
  public string DisplayName { get; }
  public int MaxHp { get; }
  public int MaxMp { get; }
  public int AttackPower { get; }
  public int Defense { get; }
  public int HitCoefficient { get; }
  public int Evasion { get; }
  public int MoveRange { get; }
  public int CriticalRate { get; }
  public AttackRange NormalAttackRange { get; }
}
