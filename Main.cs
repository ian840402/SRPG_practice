using Godot;
using System;

public partial class Main : Node2D
{
  private const int BoardSize = 8;
  private const int TileSize = 64;
  private static readonly Vector2 BoardOrigin = new(64, 64);

  private readonly Unit _playerUnit = new(new Vector2I(1, 1), hp: 10, attackPower: 3, moveRange: 3, Team.Player);
  private readonly Unit _enemyUnit = new(new Vector2I(6, 6), hp: 10, attackPower: 2, moveRange: 2, Team.Enemy);
  private Unit _selectedUnit;
  private bool _isEnemyDefeated;
  private string _statusText = "Click the player unit to select it.";

  public override void _Ready()
  {
    QueueRedraw();
  }

  public override void _Draw()
  {
    DrawBoard();
    DrawSelection();
    DrawUnit(_playerUnit, new Color(0.2f, 0.45f, 1.0f), "P");
    if (!_isEnemyDefeated)
    {
      DrawUnit(_enemyUnit, new Color(1.0f, 0.25f, 0.25f), "E");
    }
    DrawStatusText();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (inputEvent is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } mouseButton)
    {
      return;
    }

    if (!TryGetGridPosition(mouseButton.Position, out var clickedGridPosition))
    {
      _statusText = "Clicked outside the board.";
      QueueRedraw();
      return;
    }

    GD.Print($"Clicked tile: {clickedGridPosition}");

    if (clickedGridPosition == _playerUnit.GridPosition)
    {
      _selectedUnit = _playerUnit;
      _statusText = $"Player selected at {clickedGridPosition}.";
    }
    else if (_selectedUnit is { Team: Team.Player } selectedUnit)
    {
      var playerActed = TryMoveSelectedUnit(selectedUnit, clickedGridPosition);
      if (!playerActed)
      {
        TryAttackEnemy(selectedUnit, clickedGridPosition);
      }
    }
    else
    {
      _statusText = $"Clicked tile {clickedGridPosition}.";
    }

    QueueRedraw();
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

  private void DrawSelection()
  {
    if (_selectedUnit is not { } selectedUnit)
    {
      return;
    }

    var selectedGridPosition = selectedUnit.GridPosition;
    var tilePosition = BoardOrigin + new Vector2(selectedGridPosition.X * TileSize, selectedGridPosition.Y * TileSize);
    var tileRect = new Rect2(tilePosition + new Vector2(3, 3), new Vector2(TileSize - 6, TileSize - 6));

    DrawRect(tileRect, new Color(1.0f, 0.9f, 0.1f), false, 4.0f);
  }

  private bool TryMoveSelectedUnit(Unit unit, Vector2I targetGridPosition)
  {
    if (!_isEnemyDefeated && targetGridPosition == _enemyUnit.GridPosition)
    {
      _statusText = "Cannot move onto the enemy tile.";
      return false;
    }

    var distance = GetManhattanDistance(unit.GridPosition, targetGridPosition);
    if (distance > unit.MoveRange)
    {
      _statusText = $"Target is too far. Move range is {unit.MoveRange}.";
      return false;
    }

    unit.MoveTo(targetGridPosition);
    EndPlayerAction($"Player moved to {targetGridPosition}.");
    GD.Print($"Player moved to: {targetGridPosition}");
    return true;
  }

  private bool TryAttackEnemy(Unit unit, Vector2I targetGridPosition)
  {
    if (_isEnemyDefeated)
    {
      return false;
    }

    var distance = GetManhattanDistance(_playerUnit.GridPosition, _enemyUnit.GridPosition);
    if (targetGridPosition != _enemyUnit.GridPosition)
    {
      return false;
    }

    if (distance != 1)
    {
      _statusText = "Enemy is too far to attack.";
      return false;
    }

    _enemyUnit.TakeDamage(unit.AttackPower);
    if (_enemyUnit.Hp == 0)
    {
      _isEnemyDefeated = true;
      _selectedUnit = null;
      _statusText = $"Enemy took {unit.AttackPower} damage and was defeated.";
      return true;
    }

    EndPlayerAction($"Enemy took {unit.AttackPower} damage. Enemy HP: {_enemyUnit.Hp}.");
    return true;
  }

  private void EndPlayerAction(string playerActionText)
  {
    _selectedUnit = null;

    if (_isEnemyDefeated)
    {
      _statusText = playerActionText;
      return;
    }

    _statusText = $"{playerActionText} {TakeEnemyTurn()}";
  }

  private string TakeEnemyTurn()
  {
    var distance = GetManhattanDistance(_enemyUnit.GridPosition, _playerUnit.GridPosition);
    if (distance == 1)
    {
      _playerUnit.TakeDamage(_enemyUnit.AttackPower);
      return $"Enemy attacked player for {_enemyUnit.AttackPower} damage. Player HP: {_playerUnit.Hp}.";
    }

    var stepsMoved = MoveEnemyTowardPlayer();
    return $"Enemy moved {stepsMoved} tile(s) to {_enemyUnit.GridPosition}.";
  }

  private int MoveEnemyTowardPlayer()
  {
    var stepsMoved = 0;
    for (var step = 0; step < _enemyUnit.MoveRange; step++)
    {
      if (GetManhattanDistance(_enemyUnit.GridPosition, _playerUnit.GridPosition) == 1)
      {
        break;
      }

      var enemyMoveDirection = GetStepToward(_enemyUnit.GridPosition, _playerUnit.GridPosition);
      _enemyUnit.MoveTo(_enemyUnit.GridPosition + enemyMoveDirection);
      stepsMoved++;
    }

    return stepsMoved;
  }

  private static Vector2I GetStepToward(Vector2I from, Vector2I to)
  {
    var delta = to - from;
    if (delta.X != 0)
    {
      return new Vector2I(Math.Sign(delta.X), 0);
    }

    return new Vector2I(0, Math.Sign(delta.Y));
  }

  private void DrawStatusText()
  {
    var statusPosition = BoardOrigin + new Vector2(0, BoardSize * TileSize + 36);

    DrawString(ThemeDB.FallbackFont, statusPosition, _statusText, fontSize: 20);
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
