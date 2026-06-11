# Prototype Goals

## Main Goal

Build a very small 2D tactical RPG prototype focused only on core gameplay logic.

No animations, sprites, sound effects, or polish are planned for the first version.

## First Playable Target

- [x] An 8x8 board exists.
- [x] One player unit and one enemy unit are placed on the board.
- [x] The player can select their unit.
- [x] The player can move the selected unit to a valid empty tile.
- [x] The player can attack the enemy when adjacent.
- [x] The enemy can take a very simple turn.
- [x] The game can detect win or loss.

## Second Playable Target

Focus on moving from a one-unit prototype to basic tactical decisions.

- [x] Add explicit turn states.
- [x] Allow a player unit to move, then attack if adjacent.
- [x] Add a second player unit and a second enemy unit.
- [x] Show simple HP information for units on the board.
- [x] Highlight valid movement tiles when a player unit is selected.

## Third Playable Target

Focus on validating whether three basic classes create visible tactical differences.

The goal is not final combat balance. The first stat values are intentionally simple and should be judged by whether class roles are observable during play.

Done when:

- Warrior, archer, and mage units can be created and shown in battle.
- The three classes use the prototype stat table from `docs/VISION.md`.
- Warrior, archer, and mage all have movement fixed at 3.
- Attack rules support both minimum and maximum range.
- Warrior normal attack range is 1.
- Archer normal attack range is 2-3 and cannot hit adjacent enemies.
- Mage normal attack range is 1.
- Normal attacks use the prototype damage, hit, critical, and minimum-damage rules.
- Mage has MP and can use normal attacks.
- Mage has a single-target spell and a cross-shaped spell using the prototype skill rules.
- Mage skills ignore defense, do not use hit checks, and cannot critically hit.
- UI shows enough HP, MP, class, and action feedback for playtesting.
- Enemy AI can stay simple, but it must not break the basic combat flow.

Validation questions:

- Does the player naturally place warriors near the front?
- Does the player try to keep archers at range because adjacent enemies are a problem?
- Does the player choose between mage normal attack, single-target spell, and cross-shaped spell for different situations?
- Does MP create a meaningful reason to avoid casting every time?
- Are the initial values good enough to reveal class differences, even if they are not balanced yet?

### Third Playable Implementation Plan

Implement this target in small reviewable steps. Each step should keep the project buildable before moving to the next one.

#### 1. Class Data Foundation

Scope:

- Add class identity for warrior, archer, and mage.
- Add a class definition table matching the prototype values in `docs/VISION.md`.
- Keep the existing battle flow unchanged.

Done when:

- Units can be assigned a class.
- Unit HP, MP, attack, defense, hit coefficient, evasion, movement, critical rate, and normal attack range can be derived from class data.
- Existing units still appear and can take turns.

Not included:

- Hit rolls.
- Critical rolls.
- Mage skills.
- UI redesign.

#### 2. Unit Stats Integration

Scope:

- Update unit state to store current HP and current MP.
- Keep max HP, max MP, attack, defense, hit coefficient, evasion, movement, critical rate, and normal attack range available through the unit or class definition.
- Initialize player and enemy units with the three prototype classes.

Done when:

- Battle setup uses warrior, archer, and mage data instead of hard-coded attack and move values.
- Movement still works with movement fixed at 3.
- Existing simple attacks still work, even if they temporarily keep the old fixed-damage behavior until the combat formula step.

Not included:

- Full combat formula.
- Skill selection.
- Status effects.

#### 3. Attack Range Rules

Scope:

- Replace adjacency-only attack checks with minimum and maximum attack range.
- Apply normal attack ranges:
  - Warrior: 1.
  - Archer: 2-3.
  - Mage normal attack: 1.

Done when:

- Warriors can attack adjacent enemies.
- Archers can attack enemies 2-3 tiles away.
- Archers cannot attack adjacent enemies.
- Mages can normal attack adjacent enemies.
- Invalid range attempts show a clear status message.

Not included:

- Attack range highlight.
- Enemy AI range awareness beyond what is required to avoid breaking combat flow.

#### 4. Normal Attack Formula

Scope:

- Implement normal attack hit checks, evasion, critical chance, critical damage, and minimum damage.
- Use the prototype formula from `docs/VISION.md`.

Done when:

- Normal attack damage is `attack - defense`, minimum 1.
- Hit rate is `attacker hit coefficient - defender evasion`, capped at 95%.
- Hit rate has no lower bound.
- Critical chance uses the class table value with no extra modifiers.
- Critical damage is calculated after base damage, then multiplied by 1.5.
- Status text shows hit, miss, damage, and critical results clearly enough for playtesting.

Not included:

- Combat forecast UI.
- Saved RNG state.
- Balancing pass.

#### 5. Basic UI Feedback

Scope:

- Show enough unit information for playtesting class differences.
- Prefer simple text over polished UI.

Done when:

- Units show HP.
- Mage units show MP.
- Units show class identity in a minimal readable way.
- Status text explains failed actions, damage results, misses, critical hits, and MP shortage.

Not included:

- Full character panels.
- Detailed combat forecast.
- Animation or visual polish.

#### 6. Mage MP And Skill Selection

Scope:

- Add a minimal way for mage units to choose normal attack, single-target spell, or cross spell.
- Spend MP when a mage skill is used.
- Apply shared mage skill rules.

Done when:

- Mages can still use normal attacks.
- Single-target spell costs 3 MP, has range 3, deals `mage attack + 4`, ignores defense, does not miss, and cannot crit.
- Cross spell costs 4 MP, has range 3, deals `mage attack + 1`, ignores defense, does not miss, and cannot crit.
- Skills cannot be used without enough MP.

Not included:

- Large skill menu.
- Skill hotkeys.
- Skill range preview.

#### 7. Cross Spell Area Resolution

Scope:

- Resolve the cross spell area as target tile plus up, down, left, and right by 1 tile.
- Damage all enemy units in the affected tiles.

Done when:

- Cross spell can hit multiple enemies.
- Empty affected tiles are ignored.
- Allied units are not affected in the first version.
- Defeated enemies are ignored by later actions.

Not included:

- Friendly fire.
- Complex area preview.
- Obstacles blocking area effects.

#### 8. Status Effect Foundation

Scope:

- Add data structures for temporary status effects.
- Implement duration handling rules without requiring every planned status to be used immediately.

Done when:

- A unit can hold status effects.
- The same status effect cannot stack.
- Reapplying the same status resets its remaining duration.
- Status effects apply immediately.
- Status effects tick down at the effect holder's turn end.
- Status effects are removed at 0 remaining turns.

Not included:

- Full buff/debuff skill set.
- Taunt AI behavior.
- Complete stun behavior across all edge cases.

#### 9. Third Playable Verification

Scope:

- Run the project checks.
- Playtest the target manually enough to answer the validation questions.
- Update documentation with what was learned.

Done when:

- `dotnet format SRPG_practice.sln --verify-no-changes` passes.
- `dotnet build SRPG_practice.sln` succeeds.
- `docs/PROGRESS.md` records what was implemented and verified.
- Any follow-up balancing or AI work is recorded without blocking this target unless it breaks the core flow.

## Todo List

### 1. Create the Board

- [x] Create an 8x8 grid.
- [x] Each tile should be clickable.
- [x] Use simple colored rectangles or default Godot nodes.
- [x] No tile art is needed yet.

### 2. Create Basic Unit Data

- [x] Create one player unit.
- [x] Create one enemy unit.
- [x] Each unit needs:
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

- [x] Clicking the player unit tile selects the unit.
- [x] Store the selected unit in game state.
- [x] A console log or simple label is enough for feedback.

### 5. Move the Player Unit

- [x] After selecting the player unit, clicking an empty tile attempts movement.
- [x] Allow movement only within move range.
- [x] Use Manhattan distance only.
- [x] Do not allow diagonal movement.
- Do not implement terrain costs yet.

### 6. Attack the Enemy

- [x] If the player unit is adjacent to the enemy, clicking the enemy attacks it.
- [x] Reduce enemy HP by the player's attack power.
- [x] Remove the enemy when HP is 0 or below.

### 7. End the Player Action

- [x] After the player moves or attacks, end the player's action.
- Keep the first version to one player unit only.
- Do not implement action points yet.

### 8. Add a Simple Enemy Turn

- [x] If the enemy is adjacent to the player, it attacks.
- [x] Otherwise, the enemy moves toward the player within its move range.
- [x] Do not implement full pathfinding yet.
- [ ] Later enemy AI pass: when the direct step toward a target is occupied, try alternate valid movement options instead of stopping immediately.

### 9. Add Win and Loss Checks

- [x] If the enemy HP reaches 0, show a win state.
- [x] If the player HP reaches 0, show a loss state.

### 10. Keep the First Version Simple

- Keep the first gameplay implementation in `Main.cs`.
- Split into files like `Board.cs`, `Unit.cs`, or `TurnManager.cs` only after the logic becomes harder to read.
