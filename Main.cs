using Godot;

public partial class Main : Node2D
{
  private readonly BoardLayout _boardLayout = new();
  private readonly BattleState _battleState = new();
  private readonly BattleRenderer _battleRenderer;
  private readonly EnemyTurnResolver _enemyTurnResolver;
  private readonly MovementRangeResolver _movementRangeResolver;
  private readonly PlayerActionResolver _playerActionResolver;
  private Unit _selectedUnit;
  private GameState _gameState = GameState.PlayerTurn;
  private string _statusText = "Click the player unit to select it.";

  public Main()
  {
    _movementRangeResolver = new MovementRangeResolver(_battleState);
    _battleRenderer = new BattleRenderer(_boardLayout, _movementRangeResolver);
    _enemyTurnResolver = new EnemyTurnResolver(_battleState);
    _playerActionResolver = new PlayerActionResolver(_battleState);
  }

  public override void _Ready()
  {
    InitTurn();
    QueueRedraw();
  }

  public override void _Draw()
  {
    _battleRenderer.Draw(this, _battleState, _selectedUnit, _gameState, _statusText);
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
    GD.Print($"Clicked tile: {clickedGridPosition}");

    var actionResult = _playerActionResolver.ResolveClick(_selectedUnit, clickedGridPosition);
    _selectedUnit = actionResult.SelectedUnit;
    _statusText = actionResult.StatusText;
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
    foreach (var unit in UnitQuery.GetAliveUnits(_battleState.PlayerUnits))
    {
      unit.StartTurn();
    }
    _statusText = statusText;
  }

}
