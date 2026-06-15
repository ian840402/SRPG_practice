using Godot;
using System.Collections.Generic;
using System.Linq;

public sealed class BattleState
{
  public List<Unit> PlayerUnits { get; } = [
      new(new Vector2I(1, 1), UnitClass.Warrior, Team.Player),
      new(new Vector2I(1, 3), UnitClass.Archer, Team.Player),
      new(new Vector2I(1, 5), UnitClass.Mage, Team.Player)
  ];

  public List<Unit> EnemyUnits { get; } = [
      new(new Vector2I(6, 6), UnitClass.Warrior, Team.Enemy),
      new(new Vector2I(6, 4), UnitClass.Archer, Team.Enemy),
      new(new Vector2I(6, 2), UnitClass.Mage, Team.Enemy)
  ];

  public IEnumerable<Unit> AllUnits => PlayerUnits.Concat(EnemyUnits);

  public IEnumerable<Unit> GetUnitsByTeam(Team team)
  {
    return team == Team.Player ? PlayerUnits : EnemyUnits;
  }

  public bool IsEnemyAlive()
  {
    return EnemyUnits.Any(unit => unit.Hp > 0);
  }

  public bool IsPlayerAlive()
  {
    return PlayerUnits.Any(unit => unit.Hp > 0);
  }
}
