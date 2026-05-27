# Prototype Goals

## Main Goal

Build a very small 2D tactical RPG prototype focused only on core gameplay logic.

No animations, sprites, sound effects, or polish are planned for the first version.

## First Playable Target

- [x] An 8x8 board exists.
- [x] One player unit and one enemy unit are placed on the board.
- The player can select their unit.
- The player can move the selected unit to a valid empty tile.
- The player can attack the enemy when adjacent.
- The enemy can take a very simple turn.
- The game can detect win or loss.

## Todo List

### 1. Create the Board

- [x] Create an 8x8 grid.
- [ ] Each tile should be clickable.
- [x] Use simple colored rectangles or default Godot nodes.
- [x] No tile art is needed yet.

### 2. Create Basic Unit Data

- [x] Create one player unit.
- [x] Create one enemy unit.
- [ ] Each unit needs:
  - Grid position
  - HP
  - Attack power
  - Move range
  - Team

### 3. Display Unit Positions

- [x] Show the player unit on the board.
- [x] Show the enemy unit on the board.
- [x] Use different simple colors to distinguish teams.

### 4. Select the Player Unit

- Clicking the player unit tile selects the unit.
- Store the selected unit in game state.
- A console log or simple label is enough for feedback.

### 5. Move the Player Unit

- After selecting the player unit, clicking an empty tile attempts movement.
- Allow movement only within move range.
- Use Manhattan distance only.
- Do not allow diagonal movement.
- Do not implement terrain costs yet.

### 6. Attack the Enemy

- If the player unit is adjacent to the enemy, clicking the enemy attacks it.
- Reduce enemy HP by the player's attack power.
- Remove the enemy when HP is 0 or below.

### 7. End the Player Action

- After the player moves or attacks, end the player's action.
- Keep the first version to one player unit only.
- Do not implement action points yet.

### 8. Add a Simple Enemy Turn

- If the enemy is adjacent to the player, it attacks.
- Otherwise, the enemy moves one tile toward the player.
- Do not implement full pathfinding yet.

### 9. Add Win and Loss Checks

- If the enemy HP reaches 0, show a win state.
- If the player HP reaches 0, show a loss state.

### 10. Keep the First Version Simple

- Keep the first gameplay implementation in `Main.cs`.
- Split into files like `Board.cs`, `Unit.cs`, or `TurnManager.cs` only after the logic becomes harder to read.
