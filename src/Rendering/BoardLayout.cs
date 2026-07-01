using Godot;

namespace SRPGPractice.Rendering;

public sealed class BoardLayout
{
  public const int BoardSize = 8;
  public const int GridCellSize = 64;

  private const int UnitPadding = 10;

  public Vector2 BoardOrigin { get; } = new(64, 64);

  public Rect2 EndTurnButtonRect => new(
    BoardOrigin + new Vector2(BoardSize * GridCellSize + 32, 0),
    new Vector2(128, 48));

  public Rect2 GetGridRect(Vector2I gridPosition)
  {
    return new Rect2(GetGridCellPosition(gridPosition), new Vector2(GridCellSize, GridCellSize));
  }

  public Rect2 GetSelectionRect(Vector2I gridPosition)
  {
    return new Rect2(GetGridCellPosition(gridPosition) + new Vector2(3, 3), new Vector2(GridCellSize - 6, GridCellSize - 6));
  }

  public Rect2 GetUnitRect(Vector2I gridPosition)
  {
    return new Rect2(
        BoardOrigin + new Vector2(gridPosition.X * GridCellSize + UnitPadding, gridPosition.Y * GridCellSize + UnitPadding),
        new Vector2(GridCellSize - UnitPadding * 2, GridCellSize - UnitPadding * 2));
  }

  public Vector2 GetStatusTextPosition()
  {
    return BoardOrigin + new Vector2(0, BoardSize * GridCellSize + 36);
  }

  public bool TryGetGridPosition(Vector2 screenPosition, out Vector2I gridPosition)
  {
    var localPosition = screenPosition - BoardOrigin;
    gridPosition = new Vector2I(
        Mathf.FloorToInt(localPosition.X / GridCellSize),
        Mathf.FloorToInt(localPosition.Y / GridCellSize));

    return gridPosition.X >= 0
        && gridPosition.X < BoardSize
        && gridPosition.Y >= 0
        && gridPosition.Y < BoardSize;
  }

  private Vector2 GetGridCellPosition(Vector2I gridPosition)
  {
    return BoardOrigin + new Vector2(gridPosition.X * GridCellSize, gridPosition.Y * GridCellSize);
  }
}
