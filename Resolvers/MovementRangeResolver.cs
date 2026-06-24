using Godot;
using System.Collections.Generic;
using SRPGPractice.Core;
using SRPGPractice.Rendering;
using SRPGPractice.Rules;

namespace SRPGPractice.Resolvers;

public static class MovementRangeResolver
{
  public static IEnumerable<Vector2I> GetValidMovementTiles(BattleState battleState, Unit unit)
  {
    if (unit is null) yield break;
    if (unit.Team != Team.Player) yield break;
    if (unit.HasAttackedThisTurn) yield break;

    for (var y = 0; y < BoardLayout.BoardSize; y++)
    {
      for (var x = 0; x < BoardLayout.BoardSize; x++)
      {
        var gridPosition = new Vector2I(x, y);
        if (gridPosition == unit.GridPosition) continue;
        if (!MovementRules.IsWithinRemainingMovePoints(unit, gridPosition)) continue;
        if (UnitQuery.TryGetAliveUnitAt(battleState, gridPosition, out _)) continue;

        yield return gridPosition;
      }
    }
  }
}
