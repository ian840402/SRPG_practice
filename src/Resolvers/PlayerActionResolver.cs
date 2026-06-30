using Godot;
using SRPGPractice.Core;
using SRPGPractice.Rules;

namespace SRPGPractice.Resolvers;

public sealed class PlayerActionResolver
{
  private readonly BattleState _battleState;

  public PlayerActionResolver(BattleState battleState)
  {
    _battleState = battleState;
  }

  public PlayerActionResult ResolveClick(Unit? selectedUnit, Vector2I clickedGridPosition)
  {
    if (UnitQuery.TryGetAliveUnitAt(_battleState, clickedGridPosition, Team.Player, out var playerUnit))
    {
      return SelectPlayerUnit(playerUnit);
    }

    if (selectedUnit is { Team: Team.Player })
    {
      return ResolveSelectedPlayerUnitClick(selectedUnit, clickedGridPosition);
    }

    return new PlayerActionResult(selectedUnit, $"Clicked tile {clickedGridPosition}.");
  }

  private static PlayerActionResult SelectPlayerUnit(Unit unit)
  {
    return new PlayerActionResult(unit, $"{unit.Name} selected at {unit.GridPosition}. Move points: {unit.RemainingMovePoints}.");
  }

  private PlayerActionResult ResolveSelectedPlayerUnitClick(Unit selectedUnit, Vector2I clickedGridPosition)
  {
    if (selectedUnit.HasWaitedThisTurn)
    {
      return new PlayerActionResult(selectedUnit, "This unit is waiting!");
    }

    return selectedUnit.ActionMode switch
    {
      UnitActionMode.Move => ResolveMoveClick(selectedUnit, clickedGridPosition),
      UnitActionMode.NormalAttack => ResolveAttackClick(selectedUnit, clickedGridPosition),
      _ => new PlayerActionResult(selectedUnit, "Select an action mode!")
    };
  }

  private PlayerActionResult ResolveMoveClick(Unit unit, Vector2I targetGridPosition)
  {
    if (UnitQuery.TryGetAliveUnitAt(_battleState, targetGridPosition, out _))
      return new PlayerActionResult(unit, "Cannot move onto an occupied tile.");

    if (!unit.ValidMovementTiles.TryGetValue(targetGridPosition, out int distance))
      return new PlayerActionResult(unit, $"Target is too far. Remaining move points: {unit.RemainingMovePoints}.");

    unit.MoveTo(targetGridPosition);
    unit.SpendMovePoints(distance);
    GD.Print($"{unit.Name} moved to: {targetGridPosition}");
    return new PlayerActionResult(unit, $"{unit.Name} moved to {targetGridPosition}. Remaining move points: {unit.RemainingMovePoints}.");
  }

  private PlayerActionResult ResolveAttackClick(Unit unit, Vector2I targetGridPosition)
  {
    if (!UnitQuery.TryGetAliveUnitAt(_battleState, targetGridPosition, Team.Enemy, out var targetEnemy))
      return new PlayerActionResult(unit, "No enemy found.");

    var distance = MovementRules.GetManhattanDistance(unit.GridPosition, targetEnemy.GridPosition);
    if (!unit.NormalAttackRange.Contains(distance))
      return new PlayerActionResult(unit, $"{targetEnemy.Name} is outside attack range. Range: {unit.NormalAttackRange.Min}-{unit.NormalAttackRange.Max}. Distance: {distance}.");

    if (unit.HasAttackedThisTurn)
      return new PlayerActionResult(unit, $"{unit.Name} already attacked this turn.");

    var damageResult = CombatResolver.ResolveNormalAttack(unit, targetEnemy);
    var damageInfo = CombatResolver.FormatAttackResult(unit.Name, damageResult.IsHit, damageResult.IsCritical);

    targetEnemy.TakeDamage(damageResult.Damage);
    unit.MarkAttacked();
    if (targetEnemy.Hp == 0)
      return new PlayerActionResult(unit, $"{damageInfo}\n{targetEnemy.Name} took {damageResult.Damage} damage and was defeated.");

    return new PlayerActionResult(unit, $"{damageInfo}\n{targetEnemy.Name} took {damageResult.Damage} damage. {targetEnemy.Name} HP: {targetEnemy.Hp}.");
  }
}

public sealed record PlayerActionResult(Unit? SelectedUnit, string StatusText);
