using System.Collections.Generic;

public sealed class EnemyTurnResolver
{
  private readonly BattleState _battleState;

  public EnemyTurnResolver(BattleState battleState)
  {
    _battleState = battleState;
  }

  public string ResolveTurn()
  {
    var enemyActionTexts = new List<string>();
    foreach (var enemyUnit in UnitQuery.GetAliveUnits(_battleState.EnemyUnits))
    {
      enemyActionTexts.Add(ResolveEnemyUnitAction(enemyUnit));
    }

    return string.Join(" ", enemyActionTexts);
  }

  private string ResolveEnemyUnitAction(Unit enemyUnit)
  {
    if (UnitQuery.TryGetAdjacentAliveUnit(_battleState, enemyUnit, Team.Player, out var playerTarget))
    {
      playerTarget.TakeDamage(enemyUnit.AttackPower);
      return $"Enemy at {enemyUnit.GridPosition} attacked player at {playerTarget.GridPosition} for {enemyUnit.AttackPower} damage. Player HP: {playerTarget.Hp}.";
    }

    var stepsMoved = MoveEnemyTowardPlayer(enemyUnit);
    return $"Enemy at {enemyUnit.GridPosition} moved {stepsMoved} tile(s).";
  }

  private int MoveEnemyTowardPlayer(Unit enemyUnit)
  {
    var stepsMoved = 0;
    for (var step = 0; step < enemyUnit.MoveRange; step++)
    {
      if (UnitQuery.TryGetAdjacentAliveUnit(_battleState, enemyUnit, Team.Player, out _)) break;

      var nearestPlayerUnit = UnitQuery.GetNearestAliveUnit(enemyUnit, _battleState.PlayerUnits);
      if (nearestPlayerUnit is null) break;

      var enemyMoveDirection = MovementRules.GetStepToward(enemyUnit.GridPosition, nearestPlayerUnit.GridPosition);
      var targetGridPosition = enemyUnit.GridPosition + enemyMoveDirection;
      if (UnitQuery.TryGetAliveUnitAt(_battleState, targetGridPosition, out _)) break;

      enemyUnit.MoveTo(targetGridPosition);
      stepsMoved++;
    }

    return stepsMoved;
  }
}
