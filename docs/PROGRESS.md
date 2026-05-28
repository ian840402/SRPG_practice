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
- No attack, enemy turn, or win/loss logic has been implemented yet.
- No animation or art pass is planned for the first prototype.

## Next Milestone

Implement adjacent player attacks against the enemy.

Done when:

- If the selected player unit is adjacent to the enemy, clicking the enemy attacks it.
- Enemy HP is reduced by the player's attack power.
- The enemy is removed or hidden when HP reaches 0 or below.
- Non-adjacent enemy clicks do not attack.

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
