using Godot;
using System.Collections.Generic;
using System.Linq;

public sealed class BattleState
{
  public List<Unit> PlayerUnits { get; } = [
      new(new Vector2I(1, 1), hp: 10, attackPower: 3, moveRange: 3, Team.Player),
      new(new Vector2I(1, 3), hp: 10, attackPower: 3, moveRange: 3, Team.Player)
  ];

  public List<Unit> EnemyUnits { get; } = [
      new(new Vector2I(6, 6), hp: 10, attackPower: 2, moveRange: 2, Team.Enemy),
      new(new Vector2I(6, 4), hp: 10, attackPower: 2, moveRange: 2, Team.Enemy)
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
