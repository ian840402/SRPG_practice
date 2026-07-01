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

  public PlayerActionResult ResolveClick(Unit? selectedUnit, PlayerActionMode actionMode, Vector2I clickedGridPosition)
  {
    if (actionMode == PlayerActionMode.UnSelected || selectedUnit is null)
    {
      return ResolveSelectPlayerUnit(clickedGridPosition);
    }

    return ResolveSelectedPlayerUnitClick(selectedUnit, actionMode, clickedGridPosition);
  }

  private PlayerActionResult ResolveSelectPlayerUnit(Vector2I clickedGridPosition)
  {
    if (UnitQuery.TryGetAliveUnitAt(_battleState, clickedGridPosition, Team.Player, out var playerUnit))
      return new PlayerActionResult(playerUnit, PlayerActionMode.Selected, $"{playerUnit.Name} selected at {playerUnit.GridPosition}. Move points: {playerUnit.RemainingMovePoints}.");

    return new PlayerActionResult(null, PlayerActionMode.UnSelected, $"Clicked tile {clickedGridPosition}.");
  }

  private PlayerActionResult ResolveSelectedPlayerUnitClick(Unit selectedUnit, PlayerActionMode actionMode, Vector2I clickedGridPosition)
  {
    if (selectedUnit.HasWaitedThisTurn)
    {
      return new PlayerActionResult(selectedUnit, PlayerActionMode.Selected, "This unit is waiting!");
    }

    return actionMode switch
    {
      PlayerActionMode.UnSelected or PlayerActionMode.Selected => ResolveSelectPlayerUnit(clickedGridPosition),
      PlayerActionMode.Move => ResolveMoveClick(selectedUnit, clickedGridPosition),
      PlayerActionMode.NormalAttack => ResolveAttackClick(selectedUnit, clickedGridPosition),
      _ => new PlayerActionResult(selectedUnit, PlayerActionMode.Selected, "Select an action mode!")
    };
  }

  private PlayerActionResult ResolveMoveClick(Unit unit, Vector2I targetGridPosition)
  {
    if (UnitQuery.TryGetAliveUnitAt(_battleState, targetGridPosition, out _))
      return new PlayerActionResult(unit, PlayerActionMode.Move, "Cannot move onto an occupied tile.");

    if (!unit.ValidMovementTiles.TryGetValue(targetGridPosition, out int distance))
      return new PlayerActionResult(unit, PlayerActionMode.Move, $"Target is too far. Remaining move points: {unit.RemainingMovePoints}.");

    unit.MoveTo(targetGridPosition);
    unit.SpendMovePoints(distance);
    GD.Print($"{unit.Name} moved to: {targetGridPosition}");
    return new PlayerActionResult(unit, PlayerActionMode.Selected, $"{unit.Name} moved to {targetGridPosition}. Remaining move points: {unit.RemainingMovePoints}.");
  }

  private PlayerActionResult ResolveAttackClick(Unit unit, Vector2I targetGridPosition)
  {
    if (!unit.ValidAttackTiles.ContainsKey(targetGridPosition))
      return new PlayerActionResult(unit, PlayerActionMode.NormalAttack, $"Target is outside attack range. Range: {unit.NormalAttackRange.Min}-{unit.NormalAttackRange.Max}.");

    if (!UnitQuery.TryGetAliveUnitAt(_battleState, targetGridPosition, Team.Enemy, out var targetEnemy))
      return new PlayerActionResult(unit, PlayerActionMode.NormalAttack, "No enemy found.");

    var damageResult = CombatResolver.ResolveNormalAttack(unit, targetEnemy);
    var damageInfo = CombatResolver.FormatAttackResult(unit.Name, damageResult.IsHit, damageResult.IsCritical);

    targetEnemy.TakeDamage(damageResult.Damage);
    var resultText = targetEnemy.Hp == 0 ? $"{damageInfo}\n{targetEnemy.Name} took {damageResult.Damage} damage and was defeated." : $"{damageInfo}\n{targetEnemy.Name} took {damageResult.Damage} damage. {targetEnemy.Name} HP: {targetEnemy.Hp}.";
    unit.MarkAttacked();
    return new PlayerActionResult(unit, PlayerActionMode.Selected, resultText);
  }
}

public sealed record PlayerActionResult(Unit? SelectedUnit, PlayerActionMode NextActionMode, string StatusText);
