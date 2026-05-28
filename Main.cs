using Godot;
using System;

public partial class Main : Node2D
{
	private const int BoardSize = 8;
	private const int TileSize = 64;
	private static readonly Vector2 BoardOrigin = new(64, 64);

	private readonly Vector2I _playerGridPosition = new(1, 1);
	private readonly Vector2I _enemyGridPosition = new(6, 6);
	private Vector2I? _selectedPlayerGridPosition;
	private string _statusText = "Click the player unit to select it.";

	public override void _Ready()
	{
		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawBoard();
		DrawSelection();
		DrawUnit(_playerGridPosition, new Color(0.2f, 0.45f, 1.0f), "P");
		DrawUnit(_enemyGridPosition, new Color(1.0f, 0.25f, 0.25f), "E");
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

		if (clickedGridPosition == _playerGridPosition)
		{
			_selectedPlayerGridPosition = _playerGridPosition;
			_statusText = $"Player selected at {clickedGridPosition}.";
		}
		else
		{
			_selectedPlayerGridPosition = null;
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

	private void DrawUnit(Vector2I gridPosition, Color color, string label)
	{
		var padding = 10;
		var unitPosition = BoardOrigin + new Vector2(gridPosition.X * TileSize + padding, gridPosition.Y * TileSize + padding);
		var unitSize = new Vector2(TileSize - padding * 2, TileSize - padding * 2);
		var unitRect = new Rect2(unitPosition, unitSize);

		DrawRect(unitRect, color);
		DrawString(ThemeDB.FallbackFont, unitPosition + new Vector2(16, 32), label, fontSize: 24);
	}

	private void DrawSelection()
	{
		if (_selectedPlayerGridPosition is not { } selectedGridPosition)
		{
			return;
		}

		var tilePosition = BoardOrigin + new Vector2(selectedGridPosition.X * TileSize, selectedGridPosition.Y * TileSize);
		var tileRect = new Rect2(tilePosition + new Vector2(3, 3), new Vector2(TileSize - 6, TileSize - 6));

		DrawRect(tileRect, new Color(1.0f, 0.9f, 0.1f), false, 4.0f);
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
}
