using Godot;
using System;

public static class MovementRules
{
  public static int GetMoveCost(Vector2I from, Vector2I to)
  {
    return GetManhattanDistance(from, to);
  }

  public static bool IsWithinRemainingMovePoints(Unit unit, Vector2I targetGridPosition)
  {
    return GetMoveCost(unit.GridPosition, targetGridPosition) <= unit.RemainingMovePoints;
  }

  public static Vector2I GetStepToward(Vector2I from, Vector2I to)
  {
    var delta = to - from;
    if (delta.X != 0) return new Vector2I(Math.Sign(delta.X), 0);

    return new Vector2I(0, Math.Sign(delta.Y));
  }

  public static int GetManhattanDistance(Vector2I from, Vector2I to)
  {
    return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
  }
}
