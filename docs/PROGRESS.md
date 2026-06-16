# Current Progress

## Current Status

- Godot C# project initialization is complete.
- `Main.tscn` exists and is set as the main scene.
- `Main.cs` is attached to the root `Node2D` in `Main.tscn`.
- `Main.cs.uid` exists and matches the script UID referenced by `Main.tscn`.
- `Main.cs` draws an 8x8 board and displays three player units and three enemy units.
- Board tile clicks now resolve to grid coordinates.
- The player unit can be selected by clicking its tile.
- Selection feedback is shown with a highlighted tile, status text, and console output.
- Basic unit data now includes grid position, current HP, current MP, class identity, attack power, defense, hit coefficient, evasion, move range, critical rate, normal attack range, and team.
- The selected player unit can move to valid empty tiles within move range.
- Movement uses Manhattan distance and does not allow moving onto occupied tiles.
- The selected player unit can attack the enemy when adjacent.
- Enemy HP is reduced by the player's attack power.
- Unit HP is shown on the board.
- Valid movement tiles are highlighted when a player unit is selected.
- When enemy HP reaches 0, the enemy is marked defeated, hidden, no longer blocks movement, and cannot be attacked again.
- After the player successfully moves or attacks, the current player action ends by clearing the selected unit.
- After the player successfully moves or attacks, the enemy takes a simple turn if it is not defeated.
- If the enemy is adjacent to the player during its turn, it attacks and reduces player HP.
- Otherwise, the enemy moves toward the player within its move range and stops early when it becomes adjacent.
- Win/loss logic is implemented.
- Gameplay flow now uses explicit `GameState` values for player turn, enemy turn, win, and loss.
- Battle state, unit queries, movement rules, and enemy turn resolution are split into dedicated C# classes.
- Board layout and battle rendering are split into dedicated C# classes.
- Player action resolution is split into a dedicated C# class.
- C# gameplay files are organized into `Core`, `Rules`, `Resolvers`, and `Rendering` folders.
- Warrior, archer, and mage class data is implemented and battle setup now creates units from class definitions.
- Normal attack range checks support minimum and maximum range.
- Design tradeoffs are documented in `docs/DESIGN_DECISIONS.md`.
- Product vision is being documented in `docs/VISION.md`.
- If the enemy HP reaches 0, the game shows a win state.
- If the player HP reaches 0, the game shows a loss state.
- After win or loss, further gameplay input is ignored.
- No animation or art pass is planned for the first prototype.

## Next Milestone

Third playable target.

Done when:

- Warrior, archer, and mage units can be created and shown in battle.
- The three classes use the prototype stat table from `docs/VISION.md`.
- Normal attacks use the prototype damage, hit, critical, and minimum-damage rules.
- Mage MP and prototype skills are playable.
- UI shows enough HP, MP, class, and action feedback for playtesting.

Next implementation step: normal attack formula.

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

### 2026-06-04T14:28:40+0800

- Added a second player unit and a second enemy unit.
- Updated `Main.cs` to use player and enemy unit lists instead of single unit fields.
- Player turn state is now stored per unit, including remaining move points and whether that unit attacked this turn.
- Selection, movement, attacks, enemy turns, and win/loss checks now work against alive units in each team.
- Enemy turns now process each alive enemy once.
- Updated `docs/GOALS.md` to mark the multi-unit milestone complete for the second playable target.

### 2026-06-09T11:14:44+0800

- Recorded a known enemy AI limitation for later work.
- Current enemy movement stops immediately if the direct step toward the nearest player is occupied, even when other adjacent tiles are open.
- Updated `docs/GOALS.md` with a follow-up item to try alternate valid movement options during a later enemy AI pass.

### 2026-06-09T14:22:28+0800

- Refactored gameplay architecture without changing intended gameplay behavior.
- Added `BattleState.cs` to own player and enemy unit collections plus alive-state checks.
- Added `UnitQuery.cs` for reusable alive-unit, occupied-tile, adjacent-unit, and nearest-unit lookups.
- Added `MovementRules.cs` for Manhattan movement cost and direct-step movement helpers.
- Added `EnemyTurnResolver.cs` to isolate enemy turn and simple enemy AI behavior from `Main.cs`.
- Kept `Main.cs` focused on Godot input, drawing, player actions, and turn transitions.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-09T16:49:20+0800

- Refactored rendering and board layout responsibilities without changing intended gameplay behavior.
- Added `BoardLayout.cs` to own board dimensions, tile/unit rectangles, status text position, end-turn button bounds, and screen-to-grid conversion.
- Added `BattleRenderer.cs` to draw the board, units, selection outline, status text, and end-turn button.
- Kept `Main.cs` focused on input handling, player actions, and turn transitions instead of draw details.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-09T16:59:24+0800

- Refactored player action responsibilities without changing intended gameplay behavior.
- Added `PlayerActionResolver.cs` to handle player unit selection, movement attempts, attack attempts, and action status text.
- Added `PlayerActionResult.cs` to return the selected unit and status text from player action resolution.
- Kept `Main.cs` focused on Godot input, end-turn handling, turn transitions, win/loss checks, and redraw requests.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-09T17:16:15+0800

- Organized C# files into responsibility-based folders without changing intended gameplay behavior.
- Moved core state and data files into `Core/`.
- Moved query and movement rule files into `Rules/`.
- Moved player and enemy action resolver files into `Resolvers/`.
- Moved board layout and rendering files into `Rendering/`.
- Kept `Main.cs` in the project root so `Main.tscn` continues to reference `res://Main.cs`.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-09T17:19:36+0800

- Implemented simple on-board HP display for units.
- Updated `Rendering/BattleRenderer.cs` so each alive unit draws an `HP: n` label inside its unit rectangle.
- Updated `docs/GOALS.md` to mark the second playable HP display item complete.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-09T17:24:32+0800

- Implemented valid movement tile highlights for selected player units.
- Added `Rules/MovementRangeResolver.cs` to calculate empty tiles within the selected unit's remaining Manhattan movement range.
- Updated `Rendering/BattleRenderer.cs` to draw highlighted movement tiles before drawing the selection outline and units.
- Updated `docs/GOALS.md` to mark the second playable movement highlight item complete.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-11T09:42:29+0800

- Added `docs/DESIGN_DECISIONS.md` as a Traditional Chinese record for design tradeoffs.
- Documented why movement range highlight currently scans the whole board instead of using BFS / flood fill from the selected unit.
- Documented why enemy movement currently stops when the direct step toward the target is occupied.
- Recorded when each simplified approach should be revisited.

### 2026-06-11T11:08:00+0800

- Added `docs/VISION.md` to record the intended final product vision before defining the next prototype scope.
- Recorded the current core experience: fixed-character party growth, chapter-based battles, story-driven objectives, and decision-making under known information and limited resources.
- Recorded current prototype purposes: core tactical rules, architecture practice, and a technical foundation for a future full game.
- Listed follow-up discussion topics for gradually refining the product vision.

### 2026-06-11T11:12:30+0800

- Updated `docs/VISION.md` with the current direction for party and class identity.
- Recorded that party members should join through story chapters.
- Recorded that characters are fixed, have clear class-like tactical roles, and start bound to their class because class identity is tied to story and character background.

### 2026-06-11T11:20:45+0800

- Updated `docs/VISION.md` with the current direction for character growth and class progression.
- Recorded level-based growth, active skills learned by current class and level, passive skills bound to classes, and tree-based class promotion without cross-class switching.
- Recorded three open stat growth options: fixed character growth, Fire Emblem-like random growth, and Pokemon-like effort value growth.

### 2026-06-11T11:23:35+0800

- Updated `docs/VISION.md` with the current direction for combat information transparency.
- Recorded that tactical information should be mostly transparent, while uncertainty should mainly come from hit rate and evasion rate.

### 2026-06-11T11:26:31+0800

- Updated `docs/VISION.md` with the current direction for chapter objectives and stage variety.
- Recorded that objective design should be split into main clear conditions, optional conditions, and stage characteristics.
- Recorded interest in varied story-driven objectives, optional challenge conditions, and reinforcement pressure as a stage characteristic.

### 2026-06-11T11:28:42+0800

- Updated `docs/VISION.md` with the current enemy AI framing.
- Recorded that enemy AI should be considered from both basic combat logic and objective-driven behavior based on chapter goals.

### 2026-06-11T11:37:15+0800

- Updated `docs/VISION.md` with planned enemy basic target-selection logic.
- Recorded class matchup considerations, enemy focus-fire behavior, and the current attack target priority order: killable target, advantaged matchup, high tactical value, low HP, then nearest target.

### 2026-06-11T13:42:15+0800

- Updated `docs/VISION.md` with the current resource management direction.
- Recorded MP as the planned skill resource, permanent weapons without durability, planned consumable item types, chapter-between money/equipment/shop resources, and movement plus attack or wait as the primary action model.
- Recorded that resource management should remain a supporting system, with the main gameplay focus staying on class identity.

### 2026-06-11T13:55:50+0800

- Updated `docs/VISION.md` with the current direction for death, retreat, injury, and restart rules.
- Recorded that HP reaching 0 should cause retreat instead of permanent death, with injury carrying into later chapters until treated between chapters.
- Recorded that chapter restart and returning to pre-battle preparation should be available.
- Recorded in-chapter saving as an open decision because it trades modern convenience against save/load abuse and tactical risk.
- Recorded that difficulty modes are not currently planned to avoid adding another layer of numerical balancing complexity.

### 2026-06-11T14:04:38+0800

- Updated `docs/VISION.md` with the current direction for story presentation and between-chapter base flow.
- Recorded linear story progression, story scenes before and after battles, in-battle story events, and a between-chapter base for preparation and character interaction.
- Recorded that the base should include equipment preparation, shop, class promotion, tavern-style next-map information, and character interaction, with no world map system planned.

### 2026-06-11T14:14:25+0800

- Updated `docs/VISION.md` with initial class design principles.
- Recorded that class differences may come from stats, movement, range, active skills, passives, attack properties, and objective contribution, with base stats, movement, and passive abilities currently considered the most important.

### 2026-06-11T14:18:31+0800

- Updated `docs/VISION.md` to clarify that class identity is the central design axis.
- Recorded that stats, movement, passives, active skills, matchup rules, enemy AI target selection, objective contribution, story identity, and promotion routes should be evaluated through how they reinforce class differences and tactical decisions.

### 2026-06-11T14:29:15+0800

- Updated `docs/VISION.md` with the initial combat stat model.
- Recorded HP, MP, attack, defense, hit, evasion, luck, and movement as the current base stats.
- Recorded the current plan to avoid separate magic attack and magic defense stats, using mage skill rules such as ignoring physical defense to create matchup value against high-defense units.
- Recorded hit rate and critical rate as separate combat probabilities, with luck and critical rate interaction still open for design.

### 2026-06-11T14:38:36+0800

- Updated `docs/VISION.md` with the current in-chapter save direction.
- Recorded that in-chapter saving should be allowed, but save data should preserve RNG state so identical actions after loading produce identical results and cannot be used only to reroll hit, evasion, or critical outcomes.

### 2026-06-11T14:56:23+0800

- Updated `docs/VISION.md` with initial active skill design principles.
- Recorded sparse active skills for most non-mage classes, class promotion timing for active and passive skill acquisition, mage classes gaining more active skills and passives per promotion, MP-only skill costs, no displacement or terrain-effect skills, and mage skills not using hit or critical checks.

### 2026-06-11T15:10:32+0800

- Updated `docs/VISION.md` with the revised class promotion pacing.
- Recorded that characters may start around level 2-3, major party members should join through the early-to-mid game, the first true branch promotion should happen around level 15, the later strengthening promotion around level 25, and the level cap around 30.
- Recorded six initial class groups and the current intent for three true functional branches per group, treating pure strengthening promotions as non-functional class count increases.

### 2026-06-11T15:14:37+0800

- Updated `docs/VISION.md` with the revised skill acquisition schedule.
- Recorded normal classes gaining an initial passive, a level 10 active skill, a level 15 branch passive, and a level 25 active plus passive.
- Recorded mage classes gaining an initial active plus passive, a level 10 active skill, a level 15 active plus passive, and two active skills plus one passive at level 25.

### 2026-06-11T15:51:10+0800

- Updated `docs/VISION.md` with the current active skill effect scope.
- Recorded damage, healing, buff/debuff, defensive skills, and control skills as planned skill categories.
- Recorded that conditional class skills are currently out of scope to keep early skill rules and balance simpler.

### 2026-06-11T15:56:22+0800

- Updated `docs/VISION.md` to narrow defensive skill scope.
- Recorded damage reduction and taunt as the main planned defensive skill types.
- Recorded that guard and counter-stance style defensive interactions should stay out of the early scope to avoid adding too much frontline-system complexity.

### 2026-06-11T16:01:10+0800

- Updated `docs/VISION.md` to narrow control skill scope.
- Recorded stun as the main planned control skill type.
- Recorded that silence, immobilize, and no-counter style control effects should stay out of the early scope to keep skill and AI rules simpler.

### 2026-06-11T16:02:50+0800

- Updated `docs/VISION.md` with the planned buff and debuff scope.
- Recorded attack, defense, hit rate, and movement as the main values affected by buffs and debuffs.
- Recorded that buff and debuff design should stay focused on these core combat values in the early scope.

### 2026-06-11T16:08:10+0800

- Updated `docs/VISION.md` with the current next-prototype class focus: warrior, archer, and mage.
- Recorded that combat formulas need detailed follow-up discussion before implementation.
- Recorded current status-effect duration direction: stun lasts 1 turn, other status effects last 3 turns, and buffs/debuffs can stack.
- Recorded enemy AI interaction with class and skill rules as a later AI design pass.
- Updated `docs/GOALS.md` with a draft next-prototype planning section that should be split further before implementation.

### 2026-06-11T16:18:57+0800

- Updated `docs/VISION.md` with the current prototype combat formula direction.
- Recorded damage as attack minus defense, hit rate as class hit coefficient minus target evasion, and critical rate as a direct percentage chance.
- Recorded that minimum damage, hit-rate limits, critical damage multiplier, and mage defense-ignoring details still need follow-up decisions.
- Updated `docs/GOALS.md` open planning items so combat formula discussion now focuses on the remaining detailed rules instead of the broad formula shape.

### 2026-06-11T16:26:55+0800

- Updated `docs/VISION.md` with detailed prototype combat formula decisions.
- Recorded minimum damage as 1, hit-rate cap as 95% with no lower bound, and critical damage as 1.5x after base damage is calculated.
- Updated `docs/GOALS.md` so remaining combat-formula open items focus on class hit coefficients, base critical percentages, mage damage details, and later modifier rules.

### 2026-06-11T16:31:41+0800

- Updated `docs/VISION.md` with the initial next-prototype class positioning for warrior, archer, and mage.
- Recorded archers as ranged physical units that cannot attack adjacent enemies.
- Updated `docs/GOALS.md` to note that attack range rules should support both minimum and maximum range.

### 2026-06-11T16:34:04+0800

- Updated `docs/VISION.md` to clarify mage class behavior.
- Recorded that mages can use normal attacks, while their main class difference comes from having more offensive skills.
- Recorded that mages can still use normal attacks when MP is insufficient.

### 2026-06-11T16:38:06+0800

- Updated `docs/VISION.md` with exact prototype attack ranges for warrior, archer, and mage normal attacks.
- Recorded warrior attack range as 1, archer attack range as 2-3, and mage normal attack range as 1.
- Recorded two initial mage prototype skills: a single-target high-damage skill and a range 1-2 medium-low damage skill.
- Updated `docs/GOALS.md` to remove exact class attack ranges from the remaining open discussion items.

### 2026-06-11T16:39:50+0800

- Corrected the mage prototype skill plan in `docs/VISION.md`.
- Recorded both mage skills as range 3 skills.
- Recorded the two mage skills as a single-target medium-damage skill and a cross-shaped low-damage skill.
- This supersedes the previous range 1-2 medium-low damage mage skill note.

### 2026-06-11T16:45:16+0800

- Updated `docs/VISION.md` to record fixed movement for the next prototype classes.
- Recorded warrior, archer, and mage movement as 3 for the next prototype phase.
- Updated `docs/GOALS.md` to keep movement differences out of the next prototype scope so attack range, damage, and skill differences can be tested first.

### 2026-06-11T16:48:10+0800

- Updated `docs/VISION.md` with the initial next-prototype stat table for warrior, archer, and mage.
- Recorded initial HP, MP, attack, defense, hit coefficient, evasion, movement, and critical rate values for the three prototype classes.
- Recorded initial mage skill values for a single-target medium-damage spell and a cross-shaped low-damage spell.
- Updated `docs/GOALS.md` to remove class hit coefficient, base critical percentage, and mage skill rule items from the remaining open discussion list.

### 2026-06-11T16:55:54+0800

- Updated `docs/VISION.md` with initial mage skill damage formulas.
- Recorded the single-target mage spell as attack + 4 and the cross-shaped mage spell as attack + 1.
- Recorded that both mage spells ignore defense, do not use hit checks, and cannot critically hit.
- Updated `docs/GOALS.md` to remove mage defense-ignoring damage details from the remaining open discussion list.

### 2026-06-11T16:59:30+0800

- Updated `docs/VISION.md` to promote mage skill behavior into a shared rule.
- Recorded that all mage skills ignore defense, do not use hit checks, and cannot critically hit.
- Kept the two next-prototype mage skills as concrete examples under that shared mage skill rule.

### 2026-06-11T17:05:09+0800

- Updated `docs/VISION.md` with remaining prototype combat and status-effect rules.
- Recorded that the next prototype will not apply any critical-rate modifiers beyond the class table values.
- Recorded that status effects do not stack; reapplying the same effect only resets its remaining duration.
- Recorded that status effects apply immediately, tick down at the effect holder's turn end, and are removed at 0 remaining turns.
- Updated `docs/GOALS.md` so the remaining open planning items focus on status-effect values and implementation order.

### 2026-06-11T17:10:24+0800

- Updated `docs/VISION.md` with initial buff and debuff value amounts.
- Recorded attack and defense buffs as 1.5x, and matching debuffs as 0.5x.
- Recorded hit-rate buffs and debuffs as +10 and -10 percentage points.
- Recorded movement buffs and debuffs as +1 and -1 movement.
- Updated `docs/GOALS.md` so implementation order is the only remaining open planning item.

### 2026-06-11T17:24:33+0800

- Updated `docs/GOALS.md` to define the third playable target around validating class differentiation.
- Recorded that the next prototype's stat values are for observing role differences, not final balance.
- Added done criteria for warrior, archer, mage, attack ranges, normal attacks, mage MP and skills, and UI feedback.
- Added validation questions for playtesting whether warrior frontline use, archer range management, mage skill choice, and MP tradeoffs are visible.
- Updated `docs/VISION.md` to record the same next-prototype balancing intent.

### 2026-06-11T17:31:21+0800

- Updated `docs/GOALS.md` with a detailed implementation plan for the third playable target.
- Split the next phase into class data foundation, unit stat integration, attack range rules, normal attack formula, UI feedback, mage skills, cross spell resolution, status effect foundation, and final verification.
- Recorded scope, done criteria, and out-of-scope items for each implementation step.
- No C# implementation was started in this documentation pass.

### 2026-06-12T16:20:52+0800

- Implemented the class data foundation for the third playable target.
- Added `UnitClass` identity for warrior, archer, and mage.
- Added `AttackRange`, `UnitClassDefinition`, and `UnitClassDefinitions` so class HP, MP, attack, defense, hit coefficient, evasion, movement, critical rate, and normal attack range can be derived from the prototype table in `docs/VISION.md`.
- Updated `Unit` so units can be created from a class definition while preserving the existing battle flow.
- Updated `docs/GOALS.md` to mark the class data foundation step complete.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-15T16:51:10+0800

- Implemented unit stats integration for the third playable target.
- Updated `Unit` to store current HP and current MP, while attack, defense, hit coefficient, evasion, movement, critical rate, and normal attack range stay available through class definitions.
- Updated battle setup so both player and enemy teams include warrior, archer, and mage units created from class data instead of hard-coded HP, attack, and movement values.
- Updated `docs/GOALS.md` to mark the unit stats integration step complete.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.

### 2026-06-15T17:27:13+0800

- Implemented normal attack range rules for the third playable target.
- Player attacks now use each unit's normal attack minimum and maximum range instead of adjacency-only checks.
- Invalid range attempts now show the unit's normal attack range and current target distance.
- Enemy attacks now use normal attack range, and simple enemy movement stops once a target enters that range.
- Updated `docs/GOALS.md` to mark the attack range rules step complete.
- Verified formatting with `dotnet format SRPG_practice.sln --verify-no-changes`.
- Verified the project with `dotnet build SRPG_practice.sln`: build succeeded with 0 warnings and 0 errors.
