# 程式碼地圖

這份文件用來快速找回目前程式結構。它不是完整架構文件；只記錄回到專案時最需要知道的入口流程與檔案責任。

## 目前主流程

玩家點擊棋盤時：

`Main._Input()` -> `BoardLayout.TryGetGridPosition()` -> `Main.HandlePlayerTurnClick()` -> `PlayerActionResolver.ResolveClick()` -> 選取或依目前行動模式移動/攻擊 -> `Main.UpdateGameStateAfterInput()`

玩家切換行動模式或待機時：

`Main._Input()` -> 鍵盤 `M` / `A` / `W` -> `Unit.TrySetState()` 或 `Unit.MarkWaited()` -> 更新狀態文字與選取狀態。

玩家按下結束回合時：

`Main._Input()` -> `Main.TryHandleEndTurnButtonClick()` -> `Main.EndPlayerTurn()` -> `Main.StartEnemyTurn()` -> `EnemyTurnResolver.ResolveTurn()` -> `Main.StartPlayerTurn()`

畫面更新時：

`Main._Draw()` -> `BattleRenderer.Draw()` -> 畫棋盤、可移動格、選取框、單位、結束回合按鈕與狀態文字。

## 檔案責任

### 根目錄

- `Main.cs`：Godot 場景入口。負責 input、draw、回合切換、勝敗檢查，以及把玩家點擊交給 resolver。

### Core

- `Core/BattleState.cs`：保存玩家與敵方單位清單，並提供雙方是否仍有存活單位的檢查。
- `Core/Unit.cs`：保存單位目前狀態，例如位置、HP、MP、剩餘移動點、本回合是否已攻擊/待機，以及目前行動模式。
- `Core/UnitActionMode.cs`：定義選取後的暫時操作模式，例如查看、移動與普通攻擊。
- `Core/UnitClass.cs`：定義目前原型使用的職業種類。
- `Core/UnitClassDefinition.cs`：定義單一職業的數值資料格式。
- `Core/UnitClassDefinitions.cs`：保存戰士、弓箭手、法師的原型數值表。
- `Core/AttackRange.cs`：表示普通攻擊的最小與最大射程。
- `Core/Team.cs`：定義玩家隊伍與敵方隊伍。
- `Core/GameState.cs`：定義玩家回合、敵方回合、勝利、失敗。

### Rules

- `Rules/MovementRules.cs`：處理曼哈頓距離、移動消耗，以及敵人朝目標前進的一步方向。
- `Rules/UnitQuery.cs`：查詢單位，例如存活單位、指定格子的單位、最近單位、普通攻擊範圍內目標。

### Resolvers

- `Resolvers/PlayerActionResolver.cs`：處理玩家點擊棋盤後要做什麼，包括選取，以及依目前行動模式執行移動或普通攻擊。
- `Resolvers/PlayerActionResolver.cs` 內的 `PlayerActionResult`：回傳玩家行動後的選取單位與狀態文字。
- `Resolvers/EnemyTurnResolver.cs`：處理簡單敵方回合；敵人能攻擊就攻擊，否則朝最近玩家移動。
- `Resolvers/MovementRangeResolver.cs`：用 static helper 計算選取玩家單位目前可以移動到哪些空格。

### Rendering

- `Rendering/BoardLayout.cs`：處理棋盤與 UI 的座標，例如格子矩形、單位矩形、狀態文字位置、結束回合按鈕範圍。
- `Rendering/BattleRenderer.cs`：負責畫出目前戰鬥畫面。
- `Rendering/SelectedUnitPanel.cs`：負責顯示選取玩家單位的基礎資訊面板。

## 下一步實作入口

下一個功能是顯示普通攻擊範圍。建議先看：

1. `BattleRenderer.Draw()`
2. `MovementRangeResolver.GetValidMovementTiles()`
3. `PlayerActionResolver.ResolveAttackClick()`
4. `Unit.NormalAttackRange`

6D 第一版只顯示普通攻擊範圍，不處理技能範圍或障礙物。
