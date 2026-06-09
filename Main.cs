using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Node2D
{
  private const int BoardSize = 8;
  private const int TileSize = 64;
  private static readonly Vector2 BoardOrigin = new(64, 64);
  private static readonly Rect2 EndTurnButtonRect = new(BoardOrigin + new Vector2(BoardSize * TileSize + 32, 0), new Vector2(128, 48));

  private readonly List<Unit> _playerUnits = [
      new(new Vector2I(1, 1), hp: 10, attackPower: 3, moveRange: 3, Team.Player),
      new(new Vector2I(1, 3), hp: 10, attackPower: 3, moveRange: 3, Team.Player)
  ];
  private readonly List<Unit> _enemyUnits = [
      new(new Vector2I(6, 6), hp: 10, attackPower: 2, moveRange: 2, Team.Enemy),
      new(new Vector2I(6, 4), hp: 10, attackPower: 2, moveRange: 2, Team.Enemy)
  ];
  private Unit _selectedUnit;
  private GameState _gameState = GameState.PlayerTurn;
  private string _statusText = "Click the player unit to select it.";

  public override void _Ready()
  {
    InitTurn();
    QueueRedraw();
  }

  public override void _Draw()
  {
    DrawBoard();
    DrawSelection();
    DrawUnits(_playerUnits, new Color(0.2f, 0.45f, 1.0f), "P");
    DrawUnits(_enemyUnits, new Color(1.0f, 0.25f, 0.25f), "E");
    DrawEndTurnButton();
    DrawStatusText();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!TryGetPlayerTurnMouseClick(inputEvent, out var clickPosition)) return;

    if (TryHandleEndTurnButtonClick(clickPosition))
    {
      UpdateGameStateAfterInput();
      QueueRedraw();
      return;
    }

    if (!TryGetGridPosition(clickPosition, out var clickedGridPosition))
    {
      _statusText = "Clicked outside the board.";
      QueueRedraw();
      return;
    }

    HandlePlayerTurnClick(clickedGridPosition);
    UpdateGameStateAfterInput();
    QueueRedraw();
  }

  private bool TryGetPlayerTurnMouseClick(InputEvent inputEvent, out Vector2 clickPosition)
  {
    clickPosition = default;
    if (_gameState != GameState.PlayerTurn) return false;

    if (inputEvent is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } mouseButton) return false;

    clickPosition = mouseButton.Position;
    return true;
  }

  private bool TryHandleEndTurnButtonClick(Vector2 clickPosition)
  {
    if (!EndTurnButtonRect.HasPoint(clickPosition)) return false;

    EndPlayerTurn("Player ended the turn.");
    return true;
  }

  private void HandlePlayerTurnClick(Vector2I clickedGridPosition)
  {
    GD.Print($"Clicked tile: {clickedGridPosition}");

    if (TryGetAliveUnitAt(clickedGridPosition, Team.Player, out var playerUnit))
    {
      SelectPlayerUnit(playerUnit);
      return;
    }

    if (_selectedUnit is { Team: Team.Player } selectedUnit)
    {
      HandleSelectedPlayerUnitClick(selectedUnit, clickedGridPosition);
      return;
    }

    _statusText = $"Clicked tile {clickedGridPosition}.";
  }

  private void SelectPlayerUnit(Unit unit)
  {
    _selectedUnit = unit;
    _statusText = $"Player selected at {unit.GridPosition}. Move points: {unit.RemainingMovePoints}.";
  }

  private void HandleSelectedPlayerUnitClick(Unit selectedUnit, Vector2I clickedGridPosition)
  {
    var playerActed = TryMoveSelectedUnit(selectedUnit, clickedGridPosition);
    if (!playerActed)
    {
      TryAttackEnemy(selectedUnit, clickedGridPosition);
    }
  }

  private void DrawBoard()
  {
    for (var y = 0; y < BoardSize; y++)
    {
      for (var x = 0; x < BoardSize; x++)
      {
        var tilePosition = BoardOrigin + new Vector2(x * TileSize, y * TileSize);
        var tileRect = new Rect2(tilePosition, new Vector2(TileSize, TileSize));
        var tileColor = (x + y) % 2 == 0
            ? new Color(0.78f, 0.78f, 0.78f)
            : new Color(0.64f, 0.64f, 0.64f);

        DrawRect(tileRect, tileColor);
        DrawRect(tileRect, Colors.Black, false, 1.0f);
      }
    }
  }

  private void DrawUnit(Unit unit, Color color, string label)
  {
    var padding = 10;
    var gridPosition = unit.GridPosition;
    var unitPosition = BoardOrigin + new Vector2(gridPosition.X * TileSize + padding, gridPosition.Y * TileSize + padding);
    var unitSize = new Vector2(TileSize - padding * 2, TileSize - padding * 2);
    var unitRect = new Rect2(unitPosition, unitSize);

    DrawRect(unitRect, color);
    DrawString(ThemeDB.FallbackFont, unitPosition + new Vector2(16, 32), label, fontSize: 24);
  }

  private void DrawUnitIfAlive(Unit unit, Color color, string label)
  {
    if (unit.Hp == 0) return;

    DrawUnit(unit, color, label);
  }

  private void DrawUnits(IEnumerable<Unit> units, Color color, string label)
  {
    foreach (var unit in units)
    {
      DrawUnitIfAlive(unit, color, label);
    }
  }

  private void DrawSelection()
  {
    if (_selectedUnit is not { } selectedUnit) return;

    var selectedGridPosition = selectedUnit.GridPosition;
    var tilePosition = BoardOrigin + new Vector2(selectedGridPosition.X * TileSize, selectedGridPosition.Y * TileSize);
    var tileRect = new Rect2(tilePosition + new Vector2(3, 3), new Vector2(TileSize - 6, TileSize - 6));

    DrawRect(tileRect, new Color(1.0f, 0.9f, 0.1f), false, 4.0f);
  }

  private bool TryMoveSelectedUnit(Unit unit, Vector2I targetGridPosition)
  {
    if (unit.HasAttackedThisTurn)
    {
      _statusText = "Cannot move after attacking.";
      return false;
    }

    if (TryGetAliveUnitAt(targetGridPosition, out _))
    {
      _statusText = "Cannot move onto an occupied tile.";
      return false;
    }

    var distance = GetManhattanDistance(unit.GridPosition, targetGridPosition);
    if (distance > unit.RemainingMovePoints)
    {
      _statusText = $"Target is too far. Remaining move points: {unit.RemainingMovePoints}.";
      return false;
    }

    unit.MoveTo(targetGridPosition);
    unit.SpendMovePoints(distance);
    _statusText = $"Player moved to {targetGridPosition}. Remaining move points: {unit.RemainingMovePoints}.";
    GD.Print($"Player moved to: {targetGridPosition}");
    return true;
  }

  private bool TryAttackEnemy(Unit unit, Vector2I targetGridPosition)
  {
    if (!TryGetAliveUnitAt(targetGridPosition, Team.Enemy, out var targetEnemy)) return false;

    var distance = GetManhattanDistance(unit.GridPosition, targetEnemy.GridPosition);
    if (distance != 1)
    {
      _statusText = "Enemy is too far to attack.";
      return false;
    }

    if (unit.HasAttackedThisTurn)
    {
      _statusText = "Player already attacked this turn.";
      return false;
    }

    targetEnemy.TakeDamage(unit.AttackPower);
    unit.MarkAttacked();
    if (targetEnemy.Hp == 0)
    {
      _statusText = $"Enemy took {unit.AttackPower} damage and was defeated.";
      return true;
    }

    _statusText = $"Enemy took {unit.AttackPower} damage. Enemy HP: {targetEnemy.Hp}.";
    return true;
  }

  private void EndPlayerTurn(string playerActionText)
  {
    if (_gameState != GameState.PlayerTurn) return;

    _selectedUnit = null;

    StartEnemyTurn(playerActionText);
  }

  private void StartEnemyTurn(string playerActionText)
  {
    if (_gameState != GameState.PlayerTurn) return;

    _gameState = GameState.EnemyTurn;
    var enemyActionText = ResolveEnemyTurnAction();
    EndEnemyTurn($"{playerActionText} {enemyActionText}");
  }

  private void EndEnemyTurn(string turnSummaryText)
  {
    if (_gameState != GameState.EnemyTurn) return;

    StartPlayerTurn(turnSummaryText);
  }

  private string ResolveEnemyTurnAction()
  {
    var enemyActionTexts = new List<string>();
    foreach (var enemyUnit in GetAliveUnits(_enemyUnits))
    {
      enemyActionTexts.Add(ResolveEnemyUnitAction(enemyUnit));
    }

    return string.Join(" ", enemyActionTexts);
  }

  private string ResolveEnemyUnitAction(Unit enemyUnit)
  {
    if (TryGetAdjacentAliveUnit(enemyUnit, Team.Player, out var playerTarget))
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
      if (TryGetAdjacentAliveUnit(enemyUnit, Team.Player, out _)) break;

      var nearestPlayerUnit = GetNearestAliveUnit(enemyUnit, _playerUnits);
      if (nearestPlayerUnit is null) break;

      var enemyMoveDirection = GetStepToward(enemyUnit.GridPosition, nearestPlayerUnit.GridPosition);
      var targetGridPosition = enemyUnit.GridPosition + enemyMoveDirection;
      if (TryGetAliveUnitAt(targetGridPosition, out _)) break;

      enemyUnit.MoveTo(targetGridPosition);
      stepsMoved++;
    }

    return stepsMoved;
  }

  private void UpdateGameStateAfterInput()
  {
    if (!IsEnemyAlive())
    {
      SetGameState(GameState.Win, $"{_statusText} You win.");
      return;
    }

    if (!IsPlayerAlive())
    {
      SetGameState(GameState.Loss, $"{_statusText} You lose.");
    }
  }

  private void SetGameState(GameState gameState, string statusText)
  {
    _gameState = gameState;
    _selectedUnit = null;
    _statusText = statusText;
  }

  private void InitTurn()
  {
    _gameState = GameState.EnemyTurn;
    StartPlayerTurn("Click the player unit to select it.");
  }

  private void StartPlayerTurn(string statusText)
  {
    if (_gameState != GameState.EnemyTurn) return;

    _gameState = GameState.PlayerTurn;
    foreach (var unit in GetAliveUnits(_playerUnits))
    {
      unit.StartTurn();
    }
    _statusText = statusText;
  }

  private bool IsEnemyAlive()
  {
    return _enemyUnits.Any(unit => unit.Hp > 0);
  }

  private bool IsPlayerAlive()
  {
    return _playerUnits.Any(unit => unit.Hp > 0);
  }

  private IEnumerable<Unit> GetAliveUnits(IEnumerable<Unit> units)
  {
    return units.Where(unit => unit.Hp > 0);
  }

  private Unit GetNearestAliveUnit(Unit fromUnit, IEnumerable<Unit> units)
  {
    return GetAliveUnits(units)
        .OrderBy(unit => GetManhattanDistance(fromUnit.GridPosition, unit.GridPosition))
        .FirstOrDefault();
  }

  private bool TryGetAdjacentAliveUnit(Unit fromUnit, Team team, out Unit adjacentUnit)
  {
    adjacentUnit = GetAliveUnits(GetUnitsByTeam(team))
        .FirstOrDefault(unit => GetManhattanDistance(fromUnit.GridPosition, unit.GridPosition) == 1);
    return adjacentUnit is not null;
  }

  private bool TryGetAliveUnitAt(Vector2I gridPosition, out Unit unit)
  {
    unit = GetAliveUnits(_playerUnits.Concat(_enemyUnits))
        .FirstOrDefault(unit => unit.GridPosition == gridPosition);
    return unit is not null;
  }

  private bool TryGetAliveUnitAt(Vector2I gridPosition, Team team, out Unit unit)
  {
    unit = GetAliveUnits(GetUnitsByTeam(team))
        .FirstOrDefault(unit => unit.GridPosition == gridPosition);
    return unit is not null;
  }

  private IEnumerable<Unit> GetUnitsByTeam(Team team)
  {
    return team == Team.Player ? _playerUnits : _enemyUnits;
  }

  private static Vector2I GetStepToward(Vector2I from, Vector2I to)
  {
    var delta = to - from;
    if (delta.X != 0) return new Vector2I(Math.Sign(delta.X), 0);

    return new Vector2I(0, Math.Sign(delta.Y));
  }

  private void DrawStatusText()
  {
    var statusPosition = BoardOrigin + new Vector2(0, BoardSize * TileSize + 36);

    DrawString(ThemeDB.FallbackFont, statusPosition, _statusText, fontSize: 20);
  }

  private void DrawEndTurnButton()
  {
    var isEnabled = _gameState == GameState.PlayerTurn;
    var buttonColor = isEnabled
        ? new Color(0.2f, 0.45f, 0.85f)
        : new Color(0.45f, 0.45f, 0.45f);

    DrawRect(EndTurnButtonRect, buttonColor);
    DrawRect(EndTurnButtonRect, Colors.Black, false, 2.0f);
    DrawString(ThemeDB.FallbackFont, EndTurnButtonRect.Position + new Vector2(16, 31), "End Turn", fontSize: 20);
  }

  private static bool TryGetGridPosition(Vector2 screenPosition, out Vector2I gridPosition)
  {
    var localPosition = screenPosition - BoardOrigin;
    gridPosition = new Vector2I(
        Mathf.FloorToInt(localPosition.X / TileSize),
        Mathf.FloorToInt(localPosition.Y / TileSize));

    return gridPosition.X >= 0
        && gridPosition.X < BoardSize
        && gridPosition.Y >= 0
        && gridPosition.Y < BoardSize;
  }

  private static int GetManhattanDistance(Vector2I from, Vector2I to)
  {
    return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
  }

}
