using Godot;

public sealed class PlayerActionResolver
{
  private readonly BattleState _battleState;

  public PlayerActionResolver(BattleState battleState)
  {
    _battleState = battleState;
  }

  public PlayerActionResult ResolveClick(Unit selectedUnit, Vector2I clickedGridPosition)
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

  private PlayerActionResult SelectPlayerUnit(Unit unit)
  {
    return new PlayerActionResult(unit, $"Player selected at {unit.GridPosition}. Move points: {unit.RemainingMovePoints}.");
  }

  private PlayerActionResult ResolveSelectedPlayerUnitClick(Unit selectedUnit, Vector2I clickedGridPosition)
  {
    if (TryMoveSelectedUnit(selectedUnit, clickedGridPosition, out var moveStatusText))
    {
      return new PlayerActionResult(selectedUnit, moveStatusText);
    }

    var statusText = TryResolveAttackStatusText(selectedUnit, clickedGridPosition, out var attackStatusText)
        ? attackStatusText
        : moveStatusText;

    return new PlayerActionResult(selectedUnit, statusText);
  }

  private bool TryMoveSelectedUnit(Unit unit, Vector2I targetGridPosition, out string statusText)
  {
    if (unit.HasAttackedThisTurn)
    {
      statusText = "Cannot move after attacking.";
      return false;
    }

    if (UnitQuery.TryGetAliveUnitAt(_battleState, targetGridPosition, out _))
    {
      statusText = "Cannot move onto an occupied tile.";
      return false;
    }

    var distance = MovementRules.GetMoveCost(unit.GridPosition, targetGridPosition);
    if (!MovementRules.IsWithinRemainingMovePoints(unit, targetGridPosition))
    {
      statusText = $"Target is too far. Remaining move points: {unit.RemainingMovePoints}.";
      return false;
    }

    unit.MoveTo(targetGridPosition);
    unit.SpendMovePoints(distance);
    statusText = $"Player moved to {targetGridPosition}. Remaining move points: {unit.RemainingMovePoints}.";
    GD.Print($"Player moved to: {targetGridPosition}");
    return true;
  }

  private bool TryResolveAttackStatusText(Unit unit, Vector2I targetGridPosition, out string statusText)
  {
    if (!UnitQuery.TryGetAliveUnitAt(_battleState, targetGridPosition, Team.Enemy, out var targetEnemy))
    {
      statusText = default;
      return false;
    }

    var distance = MovementRules.GetManhattanDistance(unit.GridPosition, targetEnemy.GridPosition);
    if (!unit.NormalAttackRange.Contains(distance))
    {
      statusText = $"Enemy is outside attack range. Range: {unit.NormalAttackRange.Min}-{unit.NormalAttackRange.Max}. Distance: {distance}.";
      return true;
    }

    if (unit.HasAttackedThisTurn)
    {
      statusText = "Player already attacked this turn.";
      return true;
    }

    targetEnemy.TakeDamage(unit.AttackPower);
    unit.MarkAttacked();
    if (targetEnemy.Hp == 0)
    {
      statusText = $"Enemy took {unit.AttackPower} damage and was defeated.";
      return true;
    }

    statusText = $"Enemy took {unit.AttackPower} damage. Enemy HP: {targetEnemy.Hp}.";
    return true;
  }
}
