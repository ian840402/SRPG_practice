using Godot;
using SRPGPractice.Core;
using SRPGPractice.Rendering;
using SRPGPractice.Resolvers;
using SRPGPractice.Rules;

namespace SRPGPractice;

public partial class Main : Node2D
{
  private readonly BoardLayout _boardLayout = new();
  private readonly BattleState _battleState = new();
  private readonly BattleRenderer _battleRenderer;
  private readonly SelectedUnitPanel _selectedUnitPanel;
  private readonly EnemyTurnResolver _enemyTurnResolver;
  private readonly PlayerActionResolver _playerActionResolver;
  private Unit? _selectedUnit;
  private GameState _gameState = GameState.PlayerTurn;
  private PlayerActionMode _actionMode = PlayerActionMode.UnSelected;
  private string _statusText = "Click the player unit to select it.";

  public Main()
  {
    _battleRenderer = new BattleRenderer(_boardLayout);
    _selectedUnitPanel = new SelectedUnitPanel(new Vector2(_boardLayout.EndTurnButtonRect.Position.X, _boardLayout.EndTurnButtonRect.End.Y + 32));
    _enemyTurnResolver = new EnemyTurnResolver(_battleState);
    _playerActionResolver = new PlayerActionResolver(_battleState);

    AddChild(_selectedUnitPanel);
  }

  public override void _Ready()
  {
    InitTurn();
    QueueRedraw();
  }

  public override void _Draw()
  {
    _battleRenderer.Draw(this, _battleState, _selectedUnit, _gameState, _actionMode, _statusText);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (inputEvent is InputEventKey { Pressed: true, Keycode: Key.A or Key.M or Key.W or Key.Escape } keyButton && _gameState == GameState.PlayerTurn && _selectedUnit is not null)
    {
      if (_selectedUnit.HasWaitedThisTurn)
      {
        _statusText = "This unit is waiting!";
        QueueRedraw();
        return;
      }

      switch (keyButton.Keycode)
      {
        case Key.A:
          if (TrySetActionMode(PlayerActionMode.NormalAttack, _selectedUnit))
          {
            _selectedUnit.SetValidAttackTiles(MovementRangeResolver.GetValidNormalAttackTiles(_selectedUnit));
            _statusText = "Attack Mode";
          }
          else
          {
            _statusText = "This unit has attacked!";
          }
          break;
        case Key.M:
          if (TrySetActionMode(PlayerActionMode.Move, _selectedUnit))
          {
            _selectedUnit.SetValidMovementTiles(MovementRangeResolver.GetValidMovementTiles(_battleState, _selectedUnit));
            _statusText = "Move Mode";
          }
          else
          {
            _statusText = "This unit has moved!";
          }
          break;
        case Key.W:
          _selectedUnit.MarkWaited();
          _statusText = "This unit is waiting!";
          UnSelectUnit();
          break;
        case Key.Escape:
          if (_actionMode is PlayerActionMode.Move or PlayerActionMode.NormalAttack)
            TrySetActionMode(PlayerActionMode.Selected);
          _statusText = "This unit is selected!";
          break;
      }

      QueueRedraw();
      return;
    }

    if (!TryGetPlayerTurnMouseClick(inputEvent, out var clickPosition)) return;

    if (TryHandleEndTurnButtonClick(clickPosition))
    {
      UpdateGameStateAfterInput();
      QueueRedraw();
      return;
    }

    if (!_boardLayout.TryGetGridPosition(clickPosition, out var clickedGridPosition))
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
    if (!_boardLayout.EndTurnButtonRect.HasPoint(clickPosition)) return false;

    EndPlayerTurn("Player ended the turn.");
    return true;
  }

  private void HandlePlayerTurnClick(Vector2I clickedGridPosition)
  {
    var actionResult = _playerActionResolver.ResolveClick(_selectedUnit, _actionMode, clickedGridPosition);

    if (actionResult.SelectedUnit is null)
    {
      UnSelectUnit();
      TrySetActionMode(actionResult.NextActionMode);
      _statusText = actionResult.StatusText;
      return;
    }

    SelectUnit(actionResult.SelectedUnit);
    TrySetActionMode(actionResult.NextActionMode, actionResult.SelectedUnit);
    _statusText = actionResult.StatusText;
  }

  private void EndPlayerTurn(string playerActionText)
  {
    if (_gameState != GameState.PlayerTurn) return;

    UnSelectUnit();
    StartEnemyTurn(playerActionText);
  }

  private void StartEnemyTurn(string playerActionText)
  {
    if (_gameState != GameState.PlayerTurn) return;

    _gameState = GameState.EnemyTurn;
    var enemyActionText = _enemyTurnResolver.ResolveTurn();
    EndEnemyTurn($"{playerActionText} {enemyActionText}");
  }

  private void EndEnemyTurn(string turnSummaryText)
  {
    if (_gameState != GameState.EnemyTurn) return;

    StartPlayerTurn(turnSummaryText);
  }

  private void UpdateGameStateAfterInput()
  {
    if (!_battleState.IsEnemyAlive())
    {
      SetGameState(GameState.Win, $"{_statusText} You win.");
      return;
    }

    if (!_battleState.IsPlayerAlive())
    {
      SetGameState(GameState.Loss, $"{_statusText} You lose.");
    }
  }

  private void SetGameState(GameState gameState, string statusText)
  {
    _gameState = gameState;
    _statusText = statusText;
    UnSelectUnit();
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
    foreach (var unit in UnitQuery.GetAliveUnits(_battleState.PlayerUnits))
    {
      unit.StartTurn();
    }
    _statusText = statusText;
  }

  private void SelectUnit(Unit unit)
  {
    _selectedUnit = unit;
    _selectedUnitPanel.SetUnitInfo(_selectedUnit);
    _selectedUnitPanel.ShowInfo(true);
  }

  private void UnSelectUnit()
  {
    _selectedUnit = null;
    _selectedUnitPanel.ShowInfo(false);
  }

  private bool TrySetActionMode(PlayerActionMode mode)
  {
    if (mode is PlayerActionMode.Move or PlayerActionMode.NormalAttack) return false;
    _actionMode = mode;
    return true;
  }

  private bool TrySetActionMode(PlayerActionMode mode, Unit unit)
  {
    if (mode == PlayerActionMode.Move && !unit.CanMoveThisTurn) return false;
    if (mode == PlayerActionMode.NormalAttack && !unit.CanAttackThisTurn) return false;

    _actionMode = mode;

    return true;
  }

}
