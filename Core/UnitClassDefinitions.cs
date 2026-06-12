using System.Collections.Generic;

public static class UnitClassDefinitions
{
  private static readonly Dictionary<UnitClass, UnitClassDefinition> Definitions = new()
  {
    [UnitClass.Warrior] = new(
        UnitClass.Warrior,
        displayName: "Warrior",
        maxHp: 24,
        maxMp: 0,
        attackPower: 7,
        defense: 3,
        hitCoefficient: 85,
        evasion: 10,
        moveRange: 3,
        criticalRate: 10,
        normalAttackRange: new AttackRange(min: 1, max: 1)),
    [UnitClass.Archer] = new(
        UnitClass.Archer,
        displayName: "Archer",
        maxHp: 18,
        maxMp: 0,
        attackPower: 6,
        defense: 1,
        hitCoefficient: 90,
        evasion: 12,
        moveRange: 3,
        criticalRate: 10,
        normalAttackRange: new AttackRange(min: 2, max: 3)),
    [UnitClass.Mage] = new(
        UnitClass.Mage,
        displayName: "Mage",
        maxHp: 16,
        maxMp: 10,
        attackPower: 4,
        defense: 1,
        hitCoefficient: 80,
        evasion: 8,
        moveRange: 3,
        criticalRate: 5,
        normalAttackRange: new AttackRange(min: 1, max: 1))
  };

  public static UnitClassDefinition Get(UnitClass unitClass)
  {
    return Definitions[unitClass];
  }
}
