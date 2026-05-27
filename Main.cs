using Godot;
using System;

public partial class Main : Node2D
{
	private const int BoardSize = 8;
	private const int TileSize = 64;
	private static readonly Vector2 BoardOrigin = new(64, 64);

	private readonly Vector2I _playerGridPosition = new(1, 1);
	private readonly Vector2I _enemyGridPosition = new(6, 6);

	public override void _Ready()
	{
		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawBoard();
		DrawUnit(_playerGridPosition, new Color(0.2f, 0.45f, 1.0f), "P");
		DrawUnit(_enemyGridPosition, new Color(1.0f, 0.25f, 0.25f), "E");
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

	private void DrawUnit(Vector2I gridPosition, Color color, string label)
	{
		var padding = 10;
		var unitPosition = BoardOrigin + new Vector2(gridPosition.X * TileSize + padding, gridPosition.Y * TileSize + padding);
		var unitSize = new Vector2(TileSize - padding * 2, TileSize - padding * 2);
		var unitRect = new Rect2(unitPosition, unitSize);

		DrawRect(unitRect, color);
		DrawString(ThemeDB.FallbackFont, unitPosition + new Vector2(16, 32), label, fontSize: 24);
	}
}
