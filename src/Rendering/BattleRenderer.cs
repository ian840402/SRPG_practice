using Godot;
using System.Collections.Generic;
using SRPGPractice.Core;
using SRPGPractice.Resolvers;

namespace SRPGPractice.Rendering;

public sealed class BattleRenderer
{
  private readonly BoardLayout _layout;

  public BattleRenderer(BoardLayout layout)
  {
    _layout = layout;
  }

  public void Draw(Node2D canvas, BattleState battleState, Unit? selectedUnit, GameState gameState, string statusText)
  {
    DrawBoard(canvas);
    DrawEndTurnButton(canvas, gameState);
    DrawUnits(canvas, battleState.PlayerUnits, new Color(0.2f, 0.45f, 1.0f), "P");
    DrawUnits(canvas, battleState.EnemyUnits, new Color(1.0f, 0.25f, 0.25f), "E");
    if (selectedUnit is not null && gameState is GameState.PlayerTurn)
    {
      DrawSelection(canvas, selectedUnit);

      if (selectedUnit.ActionMode == UnitActionMode.Move && selectedUnit.CanMoveThisTurn) DrawValidMovementTiles(canvas, selectedUnit);
    }
    DrawStatusText(canvas, statusText);
  }

  private void DrawBoard(Node2D canvas)
  {
    for (var y = 0; y < BoardLayout.BoardSize; y++)
    {
      for (var x = 0; x < BoardLayout.BoardSize; x++)
      {
        var gridPosition = new Vector2I(x, y);
        var tileRect = _layout.GetTileRect(gridPosition);
        var tileColor = (x + y) % 2 == 0
            ? new Color(0.78f, 0.78f, 0.78f)
            : new Color(0.64f, 0.64f, 0.64f);

        canvas.DrawRect(tileRect, tileColor);
        canvas.DrawRect(tileRect, Colors.Black, false, 1.0f);
      }
    }
  }

  private void DrawUnit(Node2D canvas, Unit unit, Color color, string label)
  {
    var unitRect = _layout.GetUnitRect(unit.GridPosition);

    canvas.DrawRect(unitRect, color);
    canvas.DrawString(ThemeDB.FallbackFont, unitRect.Position + new Vector2(16, 32), label, fontSize: 24);
    canvas.DrawString(ThemeDB.FallbackFont, unitRect.Position + new Vector2(8, 48), $"HP: {unit.Hp}", fontSize: 14);
  }

  private void DrawValidMovementTiles(Node2D canvas, Unit selectedUnit)
  {
    foreach (var gridPosition in selectedUnit.ValidMovementTiles.Keys)
    {
      var tileRect = _layout.GetTileRect(gridPosition);
      canvas.DrawRect(tileRect, new Color(0.25f, 0.95f, 0.45f, 0.35f));
      canvas.DrawRect(tileRect, new Color(0.1f, 0.6f, 0.25f), false, 2.0f);
    }
  }

  private void DrawUnits(Node2D canvas, IEnumerable<Unit> units, Color color, string label)
  {
    foreach (var unit in units)
    {
      if (unit.Hp == 0) continue;

      DrawUnit(canvas, unit, color, label);
    }
  }

  private void DrawSelection(Node2D canvas, Unit selectedUnit)
  {
    var tileRect = _layout.GetSelectionRect(selectedUnit.GridPosition);
    canvas.DrawRect(tileRect, new Color(1.0f, 0.9f, 0.1f), false, 4.0f);
  }

  private void DrawStatusText(Node2D canvas, string statusText)
  {
    canvas.DrawMultilineString(ThemeDB.FallbackFont, _layout.GetStatusTextPosition(), statusText, fontSize: 20);
  }

  private void DrawEndTurnButton(Node2D canvas, GameState gameState)
  {
    var isEnabled = gameState == GameState.PlayerTurn;
    var buttonColor = isEnabled
        ? new Color(0.2f, 0.45f, 0.85f)
        : new Color(0.45f, 0.45f, 0.45f);

    canvas.DrawRect(_layout.EndTurnButtonRect, buttonColor);
    canvas.DrawRect(_layout.EndTurnButtonRect, Colors.Black, false, 2.0f);
    canvas.DrawString(ThemeDB.FallbackFont, _layout.EndTurnButtonRect.Position + new Vector2(16, 31), "End Turn", fontSize: 20);
  }
}
