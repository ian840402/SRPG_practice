using Godot;
using System.Collections.Generic;

public sealed class MovementRangeResolver
{
  private readonly BattleState _battleState;

  public MovementRangeResolver(BattleState battleState)
  {
    _battleState = battleState;
  }

  public IEnumerable<Vector2I> GetValidMovementTiles(Unit unit)
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
        if (UnitQuery.TryGetAliveUnitAt(_battleState, gridPosition, out _)) continue;

        yield return gridPosition;
      }
    }
  }
}
