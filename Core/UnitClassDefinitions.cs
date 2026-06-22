using System.Collections.Generic;

public static class UnitClassDefinitions
{
  private static readonly Dictionary<UnitClass, UnitClassDefinition> Definitions = new()
  {
    [UnitClass.Warrior] = new(
        UnitClass.Warrior,
        DisplayName: "Warrior",
        MaxHp: 24,
        MaxMp: 0,
        AttackPower: 7,
        Defense: 3,
        HitCoefficient: 85,
        Evasion: 10,
        MoveRange: 3,
        CriticalRate: 10,
        NormalAttackRange: new AttackRange(1, 1)),
    [UnitClass.Archer] = new(
        UnitClass.Archer,
        DisplayName: "Archer",
        MaxHp: 18,
        MaxMp: 0,
        AttackPower: 6,
        Defense: 1,
        HitCoefficient: 90,
        Evasion: 12,
        MoveRange: 3,
        CriticalRate: 10,
        NormalAttackRange: new AttackRange(2, 3)),
    [UnitClass.Mage] = new(
        UnitClass.Mage,
        DisplayName: "Mage",
        MaxHp: 16,
        MaxMp: 10,
        AttackPower: 4,
        Defense: 1,
        HitCoefficient: 80,
        Evasion: 8,
        MoveRange: 3,
        CriticalRate: 5,
        NormalAttackRange: new AttackRange(1, 1))
  };

  public static UnitClassDefinition Get(UnitClass unitClass)
  {
    return Definitions[unitClass];
  }
}
