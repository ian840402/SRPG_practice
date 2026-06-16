using Godot;
using System.Collections.Generic;
using System.Linq;

public static class UnitQuery
{
  public static IEnumerable<Unit> GetAliveUnits(IEnumerable<Unit> units)
  {
    return units.Where(unit => unit.Hp > 0);
  }

  public static Unit GetNearestAliveUnit(Unit fromUnit, IEnumerable<Unit> units)
  {
    return GetAliveUnits(units)
        .OrderBy(unit => MovementRules.GetManhattanDistance(fromUnit.GridPosition, unit.GridPosition))
        .FirstOrDefault();
  }

  public static bool TryGetAdjacentAliveUnit(BattleState battleState, Unit fromUnit, Team team, out Unit adjacentUnit)
  {
    adjacentUnit = GetAliveUnits(battleState.GetUnitsByTeam(team))
        .FirstOrDefault(unit => MovementRules.GetManhattanDistance(fromUnit.GridPosition, unit.GridPosition) == 1);
    return adjacentUnit is not null;
  }

  public static bool TryGetAliveUnitInNormalAttackRange(BattleState battleState, Unit attacker, Team targetTeam, out Unit targetUnit)
  {
    targetUnit = GetAliveUnits(battleState.GetUnitsByTeam(targetTeam))
        .FirstOrDefault(unit => attacker.NormalAttackRange.Contains(MovementRules.GetManhattanDistance(attacker.GridPosition, unit.GridPosition)));
    return targetUnit is not null;
  }

  public static bool TryGetAliveUnitAt(BattleState battleState, Vector2I gridPosition, out Unit unit)
  {
    unit = GetAliveUnits(battleState.AllUnits)
        .FirstOrDefault(unit => unit.GridPosition == gridPosition);
    return unit is not null;
  }

  public static bool TryGetAliveUnitAt(BattleState battleState, Vector2I gridPosition, Team team, out Unit unit)
  {
    unit = GetAliveUnits(battleState.GetUnitsByTeam(team))
        .FirstOrDefault(unit => unit.GridPosition == gridPosition);
    return unit is not null;
  }
}
