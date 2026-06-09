using Godot;

public sealed class BoardLayout
{
  public const int BoardSize = 8;
  public const int TileSize = 64;

  private const int UnitPadding = 10;

  public Vector2 BoardOrigin { get; } = new(64, 64);

  public Rect2 EndTurnButtonRect => new(
      BoardOrigin + new Vector2(BoardSize * TileSize + 32, 0),
      new Vector2(128, 48));

  public Rect2 GetTileRect(Vector2I gridPosition)
  {
    return new Rect2(GetTilePosition(gridPosition), new Vector2(TileSize, TileSize));
  }

  public Rect2 GetSelectionRect(Vector2I gridPosition)
  {
    return new Rect2(GetTilePosition(gridPosition) + new Vector2(3, 3), new Vector2(TileSize - 6, TileSize - 6));
  }

  public Rect2 GetUnitRect(Vector2I gridPosition)
  {
    return new Rect2(
        BoardOrigin + new Vector2(gridPosition.X * TileSize + UnitPadding, gridPosition.Y * TileSize + UnitPadding),
        new Vector2(TileSize - UnitPadding * 2, TileSize - UnitPadding * 2));
  }

  public Vector2 GetStatusTextPosition()
  {
    return BoardOrigin + new Vector2(0, BoardSize * TileSize + 36);
  }

  public bool TryGetGridPosition(Vector2 screenPosition, out Vector2I gridPosition)
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

  private Vector2 GetTilePosition(Vector2I gridPosition)
  {
    return BoardOrigin + new Vector2(gridPosition.X * TileSize, gridPosition.Y * TileSize);
  }
}
