# Current Progress

## Current Status

- Godot C# project initialization is complete.
- `Main.tscn` exists and is set as the main scene.
- `Main.cs` is attached to the root `Node2D` in `Main.tscn`.
- `Main.cs.uid` exists and matches the script UID referenced by `Main.tscn`.
- `Main.cs` draws an 8x8 board and displays one player unit and one enemy unit.
- Board tile clicks now resolve to grid coordinates.
- The player unit can be selected by clicking its tile.
- Selection feedback is shown with a highlighted tile, status text, and console output.
- Basic unit data now includes grid position, HP, attack power, move range, and team.
- The selected player unit can move to valid empty tiles within move range.
- Movement uses Manhattan distance and does not allow moving onto the enemy tile.
- The selected player unit can attack the enemy when adjacent.
- Enemy HP is reduced by the player's attack power.
- When enemy HP reaches 0, the enemy is marked defeated, hidden, no longer blocks movement, and cannot be attacked again.
- After the player successfully moves or attacks, the current player action ends by clearing the selected unit.
- After the player successfully moves or attacks, the enemy takes a simple turn if it is not defeated.
- If the enemy is adjacent to the player during its turn, it attacks and reduces player HP.
- Otherwise, the enemy moves toward the player within its move range and stops early when it becomes adjacent.
- Win/loss logic is implemented.
- Gameplay flow now uses explicit `GameState` values for player turn, enemy turn, win, and loss.
- If the enemy HP reaches 0, the game shows a win state.
- If the player HP reaches 0, the game shows a loss state.
- After win or loss, further gameplay input is ignored.
- No animation or art pass is planned for the first prototype.

## Next Milestone

Second playable target.

Done when:

- Explicit turn states are implemented.
- A player unit can move, then attack if adjacent.
- The board has two player units and two enemy units.
- Unit HP is visible on the board.
- Valid movement tiles are highlighted when a player unit is selected.

## Progress Log

### 2026-05-26T15:58:42+0800

- Confirmed project documentation rules are in place in `AGENTS.md`.
- Confirmed `docs/GOALS.md` and `docs/PROGRESS.md` exist and are the active planning documents.
- Confirmed `project.godot` points `run/main_scene` to the UID used by `Main.tscn`.
- Confirmed `Main.tscn` root node is named `Main` and has `Main.cs` attached.
- Confirmed `Main.cs` only contains a startup `Hello World!!!` print and no gameplay systems yet.
- No prototype todo item in `docs/GOALS.md` is complete yet.

### 2026-05-26T16:05:13+0800

- Implemented the first visual prototype in `Main.cs`.
- The game now draws an 8x8 board using simple rectangles.
- Added one player unit at grid position `(1, 1)` and one enemy unit at grid position `(6, 6)`.
- Player and enemy units are shown with different colors and simple labels.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
- Updated `docs/GOALS.md` to mark completed board and unit display items.

### 2026-05-27T15:22:03+0800

- Implemented board tile click handling in `Main.cs`.
- Added grid coordinate resolution from mouse position.
- Added player unit selection when clicking the player tile.
- Added selection feedback using a highlighted tile, status text, and console output.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
- Updated `docs/GOALS.md` to mark tile clicking and player selection items complete.

### 2026-05-28T08:52:56+0800

- Renamed the Godot C# project files from `test_trpg` to `SRPG_practice`.
- Updated the Godot .NET assembly name to `SRPG_practice`.
- Renamed the project folder from `test-trpg` to `SRPG_practice`.
- Verified the renamed project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
- Removed the obsolete `test_trpg.sln` file after confirming `SRPG_practice.sln` builds successfully.

### 2026-05-28T14:50:03+0800

- Added a simple `Unit` data structure in `Main.cs` for grid position, HP, attack power, move range, and team.
- Updated player and enemy state to use `Unit` instances instead of separate grid position fields.
- Implemented selected player movement to valid empty tiles within move range.
- Movement now uses Manhattan distance and rejects movement onto the enemy tile.
- Ran `dotnet format SRPG_practice.sln` to align `Main.cs` indentation with `.editorconfig`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Updated `docs/GOALS.md` to mark basic unit data and player movement items complete.

### 2026-05-28T15:00:48+0800

- Updated `.editorconfig` to use 2-space indentation across the project.
- Ran `dotnet format SRPG_practice.sln` to apply the new indentation setting.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-05-28T15:10:12+0800

- Split `Unit` out of `Main.cs` into `Unit.cs`.
- Split `Team` out of `Main.cs` into `Team.cs`.
- Kept unit rendering, input handling, and movement rules in `Main.cs` for now.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-01T10:47:16+0800

- Implemented adjacent player attacks against the enemy in `Main.cs`.
- Attacks now require clicking the enemy while the player unit is adjacent by Manhattan distance.
- Enemy HP is reduced by the player's attack power.
- When enemy HP reaches 0, the enemy is marked defeated, hidden, no longer blocks movement, and cannot be attacked again.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
- Updated `docs/GOALS.md` to mark the attack milestone complete.

### 2026-06-02T15:58:41+0800

- Implemented the end-player-action milestone in `Main.cs`.
- Successful movement now clears the selected unit so the player cannot keep moving in the same action.
- Successful attacks now clear the selected unit so the player cannot keep attacking in the same action.
- Kept the implementation simple and did not add action points.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
- Updated `docs/GOALS.md` to mark the end-player-action milestone complete.

### 2026-06-02T16:23:48+0800

- Implemented a simple enemy turn in `Main.cs`.
- Enemy turns now run after successful player movement or attack, unless the enemy was defeated.
- If the enemy is adjacent to the player, it attacks and reduces player HP.
- Otherwise, the enemy moves one tile toward the player without full pathfinding.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
- Updated `docs/GOALS.md` to mark the simple enemy turn milestone complete.

### 2026-06-02T17:03:52+0800

- Refined enemy movement in `Main.cs` to use the enemy unit's move range.
- Enemy movement now advances up to `MoveRange` tiles toward the player instead of always moving one tile.
- Enemy movement stops early once the enemy becomes adjacent to the player, and it does not attack again in the same turn after moving.
- Updated `docs/GOALS.md` and `docs/PROGRESS.md` to describe enemy movement as range-based rather than one-tile movement.

### 2026-06-03T09:07:03+0800

- Implemented win/loss checks in `Main.cs`.
- Enemy defeat now shows a win state.
- Player defeat during the enemy turn now shows a loss state.
- After win or loss, gameplay input is ignored.
- Updated `docs/GOALS.md` to mark the first playable win/loss milestone complete.

### 2026-06-03T11:45:12+0800

- Defined the second playable target in `docs/GOALS.md`.
- The next phase will focus on explicit turn states, move-then-attack flow, two units per side, visible HP, and valid movement tile highlights.
- Updated `docs/PROGRESS.md` so the next milestone points to the second playable target.

### 2026-06-04T09:06:31+0800

- Added `GameState.cs` with explicit player turn, enemy turn, win, and loss states.
- Updated `Main.cs` to use `GameState` instead of separate game-over and enemy-defeated flags.
- Enemy visibility now follows enemy HP, while gameplay input is accepted only during the player turn.
- Updated `docs/GOALS.md` to mark explicit turn states complete for the second playable target.

### 2026-06-04T09:55:12+0800

- Refactored `Main.cs` input handling without changing gameplay behavior.
- `_Input()` now delegates clicked-grid extraction, player-turn click handling, and selected-unit action handling to separate methods.
- Kept win/loss checks centralized after player input handling.

### 2026-06-04T11:07:10+0800

- Implemented a simple drawn `End Turn` button in `Main.cs`.
- Player movement now consumes remaining move points instead of ending the turn immediately.
- A selected player unit can keep moving while it has remaining move points.
- A player unit can attack once when adjacent, then the player must click `End Turn` to trigger the enemy turn.
- The `End Turn` button is ignored outside the player turn through the existing `GameState` input guard.
- Updated `docs/GOALS.md` to mark the move-then-attack milestone complete for the second playable target.

### 2026-06-04T11:25:25+0800

- Refined turn flow naming in `Main.cs`.
- Player turns now end by calling `StartEnemyTurn()`.
- Enemy turn handling is split into `StartEnemyTurn()`, `ResolveEnemyTurnAction()`, and `EndEnemyTurn()` before returning to `StartPlayerTurn()`.

### 2026-06-04T11:28:21+0800

- Consolidated enemy alive checks in `Main.cs`.
- `IsEnemyAlive()` is now only used by the centralized win/loss check after input handling.
- Movement and turn transition code now rely on `GameState` flow instead of repeating enemy alive guards.

### 2026-06-04T14:15:00+0800

- Added `InitTurn()` in `Main.cs` for initial turn setup.
- Added lightweight `GameState` guards to player/enemy start and end turn methods.
- Kept initialization separate from normal player turn transitions so future first-turn ownership can change more easily.
