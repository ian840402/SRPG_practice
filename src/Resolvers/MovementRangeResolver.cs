using Godot;
using System.Collections.Generic;
using SRPGPractice.Core;
using SRPGPractice.Rendering;
using SRPGPractice.Rules;
using System.Linq;

namespace SRPGPractice.Resolvers;

public static class MovementRangeResolver
{
  // Friendly units are passable for preview, but occupied tiles are rejected when moving.
  public static Dictionary<Vector2I, int> GetValidMovementTiles(BattleState battleState, Unit unit)
  {
    var startPoint = unit.GridPosition;
    var queue = new Queue<Vector2I>();
    var distances = new Dictionary<Vector2I, int>();

    queue.Enqueue(startPoint);
    distances[startPoint] = 0;

    while (queue.Count > 0)
    {
      var current = queue.Dequeue();
      var currentDistance = distances[current];
      if (currentDistance == unit.RemainingMovePoints) continue;

      var neighbors = GetNeighbors(current);

      foreach (var item in neighbors)
      {
        if (distances.ContainsKey(item)) continue;
        if (UnitQuery.TryGetAliveUnitAt(battleState, item, Team.Enemy, out _)) continue;
        if (item.X is < 0 or >= BoardLayout.BoardSize) continue;
        if (item.Y is < 0 or >= BoardLayout.BoardSize) continue;

        distances[item] = currentDistance + 1;
        queue.Enqueue(item);
      }
    }

    distances.Remove(startPoint);

    return distances;
  }

  public static Dictionary<Vector2I, int> GetValidNormalAttackTiles(Unit unit)
  {
    var startPoint = unit.GridPosition;
    var queue = new Queue<Vector2I>();
    var distances = new Dictionary<Vector2I, int>();

    queue.Enqueue(startPoint);
    distances[startPoint] = 0;

    while (queue.Count > 0)
    {
      var current = queue.Dequeue();
      var currentDistance = distances[current];
      if (currentDistance == unit.NormalAttackRange.Max) continue;

      var neighbors = GetNeighbors(current);

      foreach (var item in neighbors)
      {
        if (distances.ContainsKey(item)) continue;
        if (item.X is < 0 or >= BoardLayout.BoardSize) continue;
        if (item.Y is < 0 or >= BoardLayout.BoardSize) continue;

        distances[item] = currentDistance + 1;
        queue.Enqueue(item);
      }
    }

    distances.Remove(startPoint);
    var result = distances.Where(i => i.Value >= unit.NormalAttackRange.Min).ToDictionary(i => i.Key, i => i.Value);

    return result;
  }

  private static Vector2I[] GetNeighbors(Vector2I startPoint)
  {
    return [
      startPoint + new Vector2I(0, 1),
      startPoint + new Vector2I(0, -1),
      startPoint + new Vector2I(1, 0),
      startPoint + new Vector2I(-1, 0),
    ];
  }
}
