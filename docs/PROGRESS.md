# 目前進度

## 目前狀態

- Godot C# 專案初始化已完成。
- `Main.tscn` 已存在，並設定為主場景。
- `Main.cs` 已掛載到 `Main.tscn` 的根 `Node2D`。
- `Main.cs.uid` 已存在，且與 `Main.tscn` 參照的 script UID 相符。
- `Main.cs` 會繪製 8x8 棋盤，並顯示三個玩家單位與三個敵方單位。
- 棋盤格點擊現在會解析為格子座標。
- 玩家單位可以透過點擊所在格被選取。
- 選取回饋會以高亮格、狀態文字與 console 輸出呈現。
- 基礎單位資料現在包含格子位置、目前 HP、目前 MP、職業識別、攻擊力、防禦、命中係數、迴避、移動範圍、暴擊率、普通攻擊射程與隊伍。
- 被選取的玩家單位可以移動到移動範圍內的有效空格。
- 移動使用曼哈頓距離，且不允許移動到已被佔用的格子。
- 被選取的玩家單位可以在相鄰時攻擊敵人。
- 敵人 HP 會依照玩家攻擊力降低。
- 單位 HP 會顯示在棋盤上。
- 選取玩家單位時，會高亮有效移動格。
- 敵人 HP 到達 0 時，會被標記為擊敗、隱藏、不再阻擋移動，且不能再被攻擊。
- 玩家成功移動或攻擊後，會清除選取單位以結束目前玩家行動。
- 玩家成功移動或攻擊後，若敵人尚未被擊敗，敵人會執行簡單回合。
- 敵人回合中，如果敵人與玩家相鄰，敵人會攻擊並降低玩家 HP。
- 否則，敵人會在移動範圍內朝玩家移動，並在變成相鄰時提前停止。
- 勝敗邏輯已實作。
- Gameplay 流程現在使用明確的 `GameState` 值表示玩家回合、敵方回合、勝利與失敗。
- 戰鬥狀態、單位查詢、移動規則與敵方回合結算已拆分到專用 C# 類別。
- 棋盤 layout 與戰鬥 rendering 已拆分到專用 C# 類別。
- 玩家行動結算已拆分到專用 C# 類別。
- C# gameplay 檔案已依照 `Core`、`Rules`、`Resolvers`、`Rendering` 資料夾整理。
- C# gameplay 型別已加入 `SRPGPractice` root namespace，並依 `Core`、`Rules`、`Resolvers`、`Rendering` 分層。
- 戰士、弓箭手與法師職業資料已實作，戰鬥配置現在會從職業定義建立單位。
- 普通攻擊射程檢查支援最小與最大射程。
- 普通攻擊公式已實作，包含命中、迴避、暴擊、最低傷害與狀態文字回饋。
- 選取玩家單位時，右側會顯示基礎單位資訊面板，包含名稱、職業、HP、法師 MP、攻防、命中/迴避、暴擊與普通攻擊射程。
- 設計取捨已記錄在 `docs/DESIGN_DECISIONS.md`。
- 產品願景正在記錄於 `docs/VISION.md`。
- 如果敵人 HP 到達 0，遊戲會顯示勝利狀態。
- 如果玩家 HP 到達 0，遊戲會顯示失敗狀態。
- 勝利或失敗後，會忽略後續 gameplay input。
- 第一個原型暫不規劃動畫或美術 pass。

## 下一個里程碑

第三個可玩目標。

完成條件：

- 可以建立戰士、弓箭手、法師單位，並顯示在戰鬥中。
- 三個職業使用 `docs/VISION.md` 裡的原型數值表。
- 普通攻擊使用原型傷害、命中、暴擊與最低傷害規則。
- 法師 MP 與原型技能可以遊玩。
- UI 顯示足夠的 HP、MP、職業與行動回饋，以支援測試。

下一個實作步驟：法師 MP 與技能選擇。

## 進度紀錄

### 2026-06-24T14:57:08+0800

- 完成基礎 UI 回饋：新增 `SelectedUnitPanel`，選取玩家單位時在棋盤右側顯示名稱、職業、HP、法師 MP、攻防、命中/迴避、暴擊與普通攻擊射程。
- 面板使用 Godot `PanelContainer`、`VBoxContainer` 與 `Label`，避免繼續用手動畫字串承載詳細資訊。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 與 `dotnet build SRPG_practice.sln` 驗證通過；build succeeded，0 warnings，0 errors。
- 下一步進入法師 MP 與技能選擇。

### 2026-06-24T08:54:09+0800

- 完成 namespace pass：加入 `SRPGPractice` root namespace，並將 C# 型別依 `Core`、`Rules`、`Resolvers`、`Rendering` 分層。
- 在 `.csproj` 設定 `RootNamespace` 為 `SRPGPractice`。
- 將 `MovementRangeResolver` 歸到 `Resolvers`，並同步更新 `docs/CODE_MAP.md` 與 `docs/DESIGN_DECISIONS.md` 的檔案路徑。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 與 `dotnet build SRPG_practice.sln` 驗證通過；build succeeded，0 warnings，0 errors。
- 下一步回到第三個可玩目標的基礎 UI 回饋。

### 2026-06-23T15:51:09+0800

- 完成普通攻擊公式練習：玩家與敵人的普通攻擊現在共用 `CombatResolver.ResolveNormalAttack()` 結算命中、暴擊與傷害。
- 命中率規格調整為下限 0%、上限 95%，並同步更新 `docs/GOALS.md`。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 與 `dotnet build SRPG_practice.sln` 驗證通過；build succeeded，0 warnings，0 errors。
- 下一個插入練習為 namespace pass。

### 2026-06-23T09:32:39+0800

- 將後續協作方式調整為學習導向流程：Codex 先提出下一階段任務卡，使用者先自行實作，完成後再由 Codex review、校正 C# / OOP 觀念並提出最小優化。
- 目前下一個實作練習仍是普通攻擊公式。

### 2026-05-26T15:58:42+0800

- 確認專案文件規則已存在於 `AGENTS.md`。
- 確認 `docs/GOALS.md` 與 `docs/PROGRESS.md` 存在，且是目前啟用的規劃文件。
- 確認 `project.godot` 的 `run/main_scene` 指向 `Main.tscn` 使用的 UID。
- 確認 `Main.tscn` 根節點名稱為 `Main`，且已掛載 `Main.cs`。
- 確認 `Main.cs` 只有啟動時的 `Hello World!!!` print，尚未有 gameplay 系統。
- `docs/GOALS.md` 中尚未有任何原型待辦項目完成。

### 2026-05-26T16:05:13+0800

- 在 `Main.cs` 實作第一個視覺原型。
- 遊戲現在會用簡單矩形繪製 8x8 棋盤。
- 在格子位置 `(1, 1)` 加入一個玩家單位，並在 `(6, 6)` 加入一個敵方單位。
- 玩家與敵方單位以不同顏色和簡單標籤顯示。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。
- 更新 `docs/GOALS.md`，標記棋盤與單位顯示項目完成。

### 2026-05-27T15:22:03+0800

- 在 `Main.cs` 實作棋盤格點擊處理。
- 加入從滑鼠位置解析格子座標的邏輯。
- 點擊玩家格時會選取玩家單位。
- 使用高亮格、狀態文字與 console 輸出加入選取回饋。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。
- 更新 `docs/GOALS.md`，標記格子點擊與玩家選取項目完成。

### 2026-05-28T08:52:56+0800

- 將 Godot C# 專案檔案從 `test_trpg` 重新命名為 `SRPG_practice`。
- 將 Godot .NET assembly name 更新為 `SRPG_practice`。
- 將專案資料夾從 `test-trpg` 重新命名為 `SRPG_practice`。
- 使用 `dotnet build SRPG_practice.sln` 驗證重新命名後的專案：build succeeded，0 warnings，0 errors。
- 確認 `SRPG_practice.sln` 可成功 build 後，移除過時的 `test_trpg.sln` 檔案。

### 2026-05-28T14:50:03+0800

- 在 `Main.cs` 加入簡單的 `Unit` 資料結構，用於格子位置、HP、攻擊力、移動範圍與隊伍。
- 將玩家與敵方狀態改為使用 `Unit` instance，而不是分散的格子位置欄位。
- 實作選取玩家單位後，可移動到移動範圍內的有效空格。
- 移動現在使用曼哈頓距離，並拒絕移動到敵人所在格。
- 執行 `dotnet format SRPG_practice.sln`，讓 `Main.cs` 縮排符合 `.editorconfig`。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 更新 `docs/GOALS.md`，標記基礎單位資料與玩家移動項目完成。

### 2026-05-28T15:00:48+0800

- 更新 `.editorconfig`，將整個專案改為使用 2-space indentation。
- 執行 `dotnet format SRPG_practice.sln` 套用新的縮排設定。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-05-28T15:10:12+0800

- 將 `Unit` 從 `Main.cs` 拆分到 `Unit.cs`。
- 將 `Team` 從 `Main.cs` 拆分到 `Team.cs`。
- 單位 rendering、input handling 與移動規則暫時保留在 `Main.cs`。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-01T10:47:16+0800

- 在 `Main.cs` 實作玩家相鄰攻擊敵人。
- 攻擊現在需要玩家單位與敵人以曼哈頓距離相鄰時，點擊敵人才能觸發。
- 敵人 HP 會依玩家攻擊力降低。
- 敵人 HP 到達 0 時，會被標記為擊敗、隱藏、不再阻擋移動，且不能再被攻擊。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。
- 更新 `docs/GOALS.md`，標記攻擊里程碑完成。

### 2026-06-02T15:58:41+0800

- 在 `Main.cs` 實作結束玩家行動里程碑。
- 成功移動後會清除選取單位，讓玩家不能在同一個行動中持續移動。
- 成功攻擊後會清除選取單位，讓玩家不能在同一個行動中持續攻擊。
- 維持簡單實作，未加入行動點數。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。
- 更新 `docs/GOALS.md`，標記結束玩家行動里程碑完成。

### 2026-06-02T16:23:48+0800

- 在 `Main.cs` 實作簡單敵方回合。
- 敵方回合現在會在玩家成功移動或攻擊後執行，除非敵人已被擊敗。
- 如果敵人與玩家相鄰，敵人會攻擊並降低玩家 HP。
- 否則，敵人會朝玩家移動一格，尚未實作完整尋路。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。
- 更新 `docs/GOALS.md`，標記簡單敵方回合里程碑完成。

### 2026-06-02T17:03:52+0800

- 調整 `Main.cs` 中的敵方移動，改為使用敵方單位的移動範圍。
- 敵方移動現在會朝玩家前進最多 `MoveRange` 格，而不是永遠只移動一格。
- 敵方移動在變成與玩家相鄰時會提前停止，且移動後不會在同一回合再次攻擊。
- 更新 `docs/GOALS.md` 與 `docs/PROGRESS.md`，將敵方移動描述為依移動範圍移動，而不是一格移動。

### 2026-06-03T09:07:03+0800

- 在 `Main.cs` 實作勝敗檢查。
- 敵人被擊敗時現在會顯示勝利狀態。
- 玩家在敵方回合中被擊敗時現在會顯示失敗狀態。
- 勝利或失敗後，會忽略 gameplay input。
- 更新 `docs/GOALS.md`，標記第一個可玩目標的勝敗里程碑完成。

### 2026-06-03T11:45:12+0800

- 在 `docs/GOALS.md` 定義第二個可玩目標。
- 下一個階段會專注在明確回合狀態、移動後攻擊流程、雙方各兩個單位、可見 HP，以及有效移動格高亮。
- 更新 `docs/PROGRESS.md`，讓下一個里程碑指向第二個可玩目標。

### 2026-06-04T09:06:31+0800

- 加入 `GameState.cs`，提供明確的玩家回合、敵方回合、勝利與失敗狀態。
- 更新 `Main.cs`，使用 `GameState` 取代分散的 game-over 與 enemy-defeated flags。
- 敵方顯示現在依照敵方 HP；gameplay input 只在玩家回合接受。
- 更新 `docs/GOALS.md`，標記第二個可玩目標的明確回合狀態完成。

### 2026-06-04T09:55:12+0800

- 在不改變 gameplay 行為的前提下，重構 `Main.cs` input handling。
- `_Input()` 現在會把點擊格解析、玩家回合點擊處理、選取單位行動處理委派給不同 method。
- 勝敗檢查仍集中在玩家 input handling 之後。

### 2026-06-04T11:07:10+0800

- 在 `Main.cs` 實作簡單繪製的 `End Turn` 按鈕。
- 玩家移動現在會消耗剩餘移動點，而不是立刻結束回合。
- 被選取的玩家單位只要還有剩餘移動點，就可以持續移動。
- 玩家單位相鄰時可以攻擊一次，之後玩家必須點擊 `End Turn` 觸發敵方回合。
- `End Turn` 按鈕會透過既有 `GameState` input guard，在非玩家回合時被忽略。
- 更新 `docs/GOALS.md`，標記第二個可玩目標的移動後攻擊里程碑完成。

### 2026-06-04T11:25:25+0800

- 調整 `Main.cs` 的回合流程命名。
- 玩家回合現在會透過呼叫 `StartEnemyTurn()` 結束。
- 敵方回合處理拆分為 `StartEnemyTurn()`、`ResolveEnemyTurnAction()` 與 `EndEnemyTurn()`，再回到 `StartPlayerTurn()`。

### 2026-06-04T11:28:21+0800

- 整合 `Main.cs` 中的敵方存活檢查。
- `IsEnemyAlive()` 現在只由 input handling 之後的集中勝敗檢查使用。
- 移動與回合轉換程式現在依賴 `GameState` flow，而不是重複敵方存活 guard。

### 2026-06-04T14:15:00+0800

- 在 `Main.cs` 加入 `InitTurn()`，用於初始回合設定。
- 對玩家/敵方開始與結束回合 method 加入輕量 `GameState` guard。
- 讓初始化與一般玩家回合轉換分開，使未來第一回合歸屬更容易調整。

### 2026-06-04T14:28:40+0800

- 加入第二個玩家單位與第二個敵方單位。
- 更新 `Main.cs`，使用玩家與敵方單位清單，而不是單一單位欄位。
- 玩家回合狀態現在保存於每個單位，包括剩餘移動點與該單位本回合是否已攻擊。
- 選取、移動、攻擊、敵方回合與勝敗檢查現在會針對雙方存活單位運作。
- 敵方回合現在會讓每個存活敵人各行動一次。
- 更新 `docs/GOALS.md`，標記第二個可玩目標的多單位里程碑完成。

### 2026-06-09T11:14:44+0800

- 記錄一個已知敵方 AI 限制供後續處理。
- 目前敵方移動在朝最近玩家的直接步伐被佔用時會立刻停止，即使其他相鄰格仍可移動。
- 更新 `docs/GOALS.md`，加入後續項目：在之後的敵方 AI pass 中嘗試其他有效移動選項。

### 2026-06-09T14:22:28+0800

- 在不改變預期 gameplay 行為的前提下重構 gameplay 架構。
- 加入 `BattleState.cs`，負責玩家與敵方單位集合，以及存活狀態檢查。
- 加入 `UnitQuery.cs`，提供可重用的存活單位、佔用格、相鄰單位與最近單位查詢。
- 加入 `MovementRules.cs`，負責曼哈頓移動消耗與直接步伐移動 helper。
- 加入 `EnemyTurnResolver.cs`，將敵方回合與簡單敵方 AI 行為從 `Main.cs` 隔離。
- 讓 `Main.cs` 專注在 Godot input、drawing、玩家行動與回合轉換。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-09T16:49:20+0800

- 在不改變預期 gameplay 行為的前提下，重構 rendering 與棋盤 layout 職責。
- 加入 `BoardLayout.cs`，負責棋盤尺寸、格子/單位矩形、狀態文字位置、結束回合按鈕範圍，以及 screen-to-grid 轉換。
- 加入 `BattleRenderer.cs`，負責繪製棋盤、單位、選取框、狀態文字與結束回合按鈕。
- 讓 `Main.cs` 專注在 input handling、玩家行動與回合轉換，而不是 draw details。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-09T16:59:24+0800

- 在不改變預期 gameplay 行為的前提下，重構玩家行動職責。
- 加入 `PlayerActionResolver.cs`，負責玩家單位選取、移動嘗試、攻擊嘗試與行動狀態文字。
- 加入 `PlayerActionResult.cs`，從玩家行動結算回傳選取單位與狀態文字。
- 讓 `Main.cs` 專注在 Godot input、結束回合處理、回合轉換、勝敗檢查與 redraw requests。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-09T17:16:15+0800

- 依照職責整理 C# 檔案，未改變預期 gameplay 行為。
- 將核心狀態與資料檔案移到 `Core/`。
- 將查詢與移動規則檔案移到 `Rules/`。
- 將玩家與敵方 action resolver 檔案移到 `Resolvers/`。
- 將棋盤 layout 與 rendering 檔案移到 `Rendering/`。
- 保留 `Main.cs` 在專案根目錄，讓 `Main.tscn` 繼續參照 `res://Main.cs`。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-09T17:19:36+0800

- 實作簡單的棋盤上單位 HP 顯示。
- 更新 `Rendering/BattleRenderer.cs`，讓每個存活單位在單位矩形內繪製 `HP: n` 標籤。
- 更新 `docs/GOALS.md`，標記第二個可玩目標的 HP 顯示項目完成。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-09T17:24:32+0800

- 實作選取玩家單位時的有效移動格高亮。
- 加入 `Rules/MovementRangeResolver.cs`，計算選取單位剩餘曼哈頓移動範圍內的空格。
- 更新 `Rendering/BattleRenderer.cs`，在繪製選取框與單位前先繪製高亮移動格。
- 更新 `docs/GOALS.md`，標記第二個可玩目標的移動高亮項目完成。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-11T09:42:29+0800

- 加入 `docs/DESIGN_DECISIONS.md`，作為設計取捨的繁體中文紀錄。
- 記錄為什麼目前移動範圍高亮是掃描整個棋盤，而不是從選取單位使用 BFS / flood fill。
- 記錄為什麼目前敵方移動會在朝目標的直接步伐被佔用時停止。
- 記錄每個簡化做法應該重新檢視的時機。

### 2026-06-11T11:08:00+0800

- 加入 `docs/VISION.md`，在定義下一個原型範圍前記錄預期最終產品願景。
- 記錄目前核心體驗：固定角色隊伍成長、章節式戰鬥、劇情驅動目標，以及在已知資訊與有限資源下做決策。
- 記錄目前原型目的：核心戰術規則、架構練習，以及未來完整遊戲的技術基礎。
- 列出後續討論主題，用於逐步細化產品願景。

### 2026-06-11T11:12:30+0800

- 更新 `docs/VISION.md`，補上目前隊伍與職業識別方向。
- 記錄隊友應透過劇情章節加入。
- 記錄角色是固定的、有清楚類職業戰術定位，且初期綁定自己的職業，因為職業識別與劇情、角色背景有關。

### 2026-06-11T11:20:45+0800

- 更新 `docs/VISION.md`，補上目前角色成長與職業進階方向。
- 記錄等級式成長、主動技能由目前職業與等級習得、被動技能綁定職業，以及樹狀職業進階且不跨職業切換。
- 記錄三個開放的數值成長選項：固定角色成長、Fire Emblem 式隨機成長、Pokemon 式努力值成長。

### 2026-06-11T11:23:35+0800

- 更新 `docs/VISION.md`，補上目前戰鬥資訊透明度方向。
- 記錄戰術資訊應大多透明，而不確定性主要來自命中率與迴避率。

### 2026-06-11T11:26:31+0800

- 更新 `docs/VISION.md`，補上目前章節目標與關卡變化方向。
- 記錄目標設計應拆成主要通關條件、可選條件與關卡特性。
- 記錄對多樣劇情驅動目標、可選挑戰條件，以及援軍壓力作為關卡特性的興趣。

### 2026-06-11T11:28:42+0800

- 更新 `docs/VISION.md`，補上目前敵方 AI framing。
- 記錄敵方 AI 應從基礎戰鬥邏輯與根據章節目標而來的目標驅動行為兩面思考。

### 2026-06-11T11:37:15+0800

- 更新 `docs/VISION.md`，補上規劃中的敵方基礎目標選擇邏輯。
- 記錄職業 matchup 考量、敵方集火行為，以及目前攻擊目標優先順序：可擊殺目標、有利 matchup、高戰術價值、低 HP，最後是最近目標。

### 2026-06-11T13:42:15+0800

- 更新 `docs/VISION.md`，補上目前資源管理方向。
- 記錄 MP 作為規劃中的技能資源、永久武器且無耐久度、規劃中的消耗品類型、章節間金錢/裝備/商店資源，以及移動加攻擊或等待作為主要行動模型。
- 記錄資源管理應維持為支援系統，主要 gameplay 重點仍放在職業識別。

### 2026-06-11T13:55:50+0800

- 更新 `docs/VISION.md`，補上目前死亡、撤退、受傷與重開規則方向。
- 記錄 HP 到達 0 應造成撤退而非永久死亡，且受傷會延續到後續章節，直到章節間治療。
- 記錄章節重開與返回戰前準備應可使用。
- 記錄章節內存檔仍是開放決策，因為它在現代便利性與 save/load 濫用、戰術風險間有所取捨。
- 記錄目前不規劃難度模式，以避免增加另一層數值平衡複雜度。

### 2026-06-11T14:04:38+0800

- 更新 `docs/VISION.md`，補上目前劇情呈現與章節間基地流程方向。
- 記錄線性劇情推進、戰前與戰後劇情、戰鬥中劇情事件，以及用於準備與角色互動的章節間基地。
- 記錄基地應包含裝備準備、商店、職業進階、酒館式下一張地圖情報與角色互動，且不規劃世界地圖系統。

### 2026-06-11T14:14:25+0800

- 更新 `docs/VISION.md`，補上初始職業設計原則。
- 記錄職業差異可來自數值、移動、射程、主動技能、被動、攻擊屬性與目標貢獻；目前認為基礎數值、移動與被動能力最重要。

### 2026-06-11T14:18:31+0800

- 更新 `docs/VISION.md`，釐清職業識別是核心設計軸線。
- 記錄數值、移動、被動、主動技能、matchup 規則、敵方 AI 目標選擇、目標貢獻、劇情識別與進階路線，都應透過它們如何強化職業差異與戰術決策來評估。

### 2026-06-11T14:29:15+0800

- 更新 `docs/VISION.md`，補上初始戰鬥數值模型。
- 記錄 HP、MP、攻擊、防禦、命中、迴避、幸運與移動作為目前基礎數值。
- 記錄目前規劃避免拆出魔法攻擊與魔法防禦，改用法師技能規則，例如無視物理防禦，來創造對高防禦單位的 matchup 價值。
- 記錄命中率與暴擊率是分開的戰鬥機率，幸運與暴擊率互動仍是開放設計。

### 2026-06-11T14:38:36+0800

- 更新 `docs/VISION.md`，補上目前章節內存檔方向。
- 記錄應允許章節內存檔，但存檔資料應保存 RNG 狀態，讓讀檔後相同行動產生相同結果，不能只用來重抽命中、迴避或暴擊結果。

### 2026-06-11T14:56:23+0800

- 更新 `docs/VISION.md`，補上初始主動技能設計原則。
- 記錄多數非法師職業的主動技能較稀疏、職業進階時間點會影響主動與被動技能取得、法師職業每次進階會取得更多主動技能與被動、技能成本只使用 MP、不做位移或地形效果技能，且法師技能不使用命中或暴擊判定。

### 2026-06-11T15:10:32+0800

- 更新 `docs/VISION.md`，補上調整後的職業進階節奏。
- 記錄角色可能從等級 2-3 左右開始，主要隊友應在遊戲早期到中期加入，第一次真正分支進階約在等級 15，後續強化進階約在等級 25，等級上限約為 30。
- 記錄六個初始職業群，以及目前每群規劃三個真正功能性分支；純強化進階不計入功能性職業數增加。

### 2026-06-11T15:14:37+0800

- 更新 `docs/VISION.md`，補上調整後的技能取得時程。
- 記錄一般職業會取得初始被動、等級 10 主動技能、等級 15 分支被動，以及等級 25 主動加被動。
- 記錄法師職業會取得初始主動加被動、等級 10 主動技能、等級 15 主動加被動，以及等級 25 兩個主動技能加一個被動。

### 2026-06-11T15:51:10+0800

- 更新 `docs/VISION.md`，補上目前主動技能效果範圍。
- 記錄傷害、治療、buff/debuff、防禦技能與控制技能作為規劃中的技能類型。
- 記錄條件式職業技能目前不在範圍內，以保持早期技能規則與平衡較簡單。

### 2026-06-11T15:56:22+0800

- 更新 `docs/VISION.md`，縮小防禦技能範圍。
- 記錄傷害減免與嘲諷作為主要規劃中的防禦技能類型。
- 記錄 guard 與 counter-stance 風格防禦互動應保持在早期範圍外，以避免加入過多前線系統複雜度。

### 2026-06-11T16:01:10+0800

- 更新 `docs/VISION.md`，縮小控制技能範圍。
- 記錄暈眩作為主要規劃中的控制技能類型。
- 記錄沉默、定身與無反擊風格控制效果應保持在早期範圍外，以維持技能與 AI 規則較簡單。

### 2026-06-11T16:02:50+0800

- 更新 `docs/VISION.md`，補上規劃中的 buff 與 debuff 範圍。
- 記錄攻擊、防禦、命中率與移動作為 buff/debuff 主要影響的數值。
- 記錄 buff/debuff 設計在早期範圍內應專注於這些核心戰鬥數值。

### 2026-06-11T16:08:10+0800

- 更新 `docs/VISION.md`，補上目前下一個原型的職業焦點：戰士、弓箭手與法師。
- 記錄戰鬥公式在實作前需要進一步細談。
- 記錄目前狀態效果持續時間方向：暈眩持續 1 回合，其他狀態效果持續 3 回合，buff/debuff 可以堆疊。
- 記錄敵方 AI 與職業/技能規則互動會作為後續 AI 設計 pass。
- 更新 `docs/GOALS.md`，加入下一個原型規劃草稿區塊，之後實作前應再拆細。

### 2026-06-11T16:18:57+0800

- 更新 `docs/VISION.md`，補上目前原型戰鬥公式方向。
- 記錄傷害為攻擊減防禦，命中率為職業命中係數減目標迴避，暴擊率為直接百分比機率。
- 記錄最低傷害、命中率限制、暴擊傷害倍率與法師無視防禦細節仍需後續決定。
- 更新 `docs/GOALS.md` 開放規劃項目，讓戰鬥公式討論聚焦在剩餘詳細規則，而不是整體公式形狀。

### 2026-06-11T16:26:55+0800

- 更新 `docs/VISION.md`，補上詳細原型戰鬥公式決策。
- 記錄最低傷害為 1，命中率上限為 95% 且沒有下限，暴擊傷害為基礎傷害計算後乘以 1.5。
- 更新 `docs/GOALS.md`，讓剩餘戰鬥公式開放項目聚焦在職業命中係數、基礎暴擊百分比、法師傷害細節與後續 modifier 規則。

### 2026-06-11T16:31:41+0800

- 更新 `docs/VISION.md`，補上戰士、弓箭手與法師的初始下一個原型職業定位。
- 記錄弓箭手是遠程物理單位，不能攻擊相鄰敵人。
- 更新 `docs/GOALS.md`，註明攻擊射程規則應支援最小與最大射程。

### 2026-06-11T16:34:04+0800

- 更新 `docs/VISION.md`，釐清法師職業行為。
- 記錄法師可以使用普通攻擊，而主要職業差異來自擁有更多攻擊技能。
- 記錄法師在 MP 不足時仍可以使用普通攻擊。

### 2026-06-11T16:38:06+0800

- 更新 `docs/VISION.md`，補上戰士、弓箭手與法師普通攻擊的精確原型射程。
- 記錄戰士攻擊射程為 1，弓箭手攻擊射程為 2-3，法師普通攻擊射程為 1。
- 記錄兩個初始法師原型技能：單體高傷害技能，以及射程 1-2 的中低傷害技能。
- 更新 `docs/GOALS.md`，從剩餘開放討論項目移除精確職業攻擊射程。

### 2026-06-11T16:39:50+0800

- 修正法師原型技能規劃於 `docs/VISION.md`。
- 記錄兩個法師技能都為射程 3。
- 記錄兩個法師技能分別為單體中傷害技能與十字範圍低傷害技能。
- 此紀錄取代前一筆射程 1-2 的中低傷害法師技能註記。

### 2026-06-11T16:45:16+0800

- 更新 `docs/VISION.md`，記錄下一個原型職業的固定移動力。
- 記錄戰士、弓箭手與法師在下一個原型階段的移動力皆為 3。
- 更新 `docs/GOALS.md`，將移動差異排除在下一個原型範圍外，讓測試先聚焦在攻擊射程、傷害與技能差異。

### 2026-06-11T16:48:10+0800

- 更新 `docs/VISION.md`，補上戰士、弓箭手與法師的初始下一個原型數值表。
- 記錄三個原型職業的初始 HP、MP、攻擊、防禦、命中係數、迴避、移動力與暴擊率。
- 記錄單體中傷害法術與十字範圍低傷害法術的初始法師技能數值。
- 更新 `docs/GOALS.md`，從剩餘開放討論清單移除職業命中係數、基礎暴擊百分比與法師技能規則項目。

### 2026-06-11T16:55:54+0800

- 更新 `docs/VISION.md`，補上初始法師技能傷害公式。
- 記錄單體法師法術為攻擊 + 4，十字範圍法師法術為攻擊 + 1。
- 記錄兩個法師法術都無視防禦、不使用命中判定，且不能暴擊。
- 更新 `docs/GOALS.md`，從剩餘開放討論清單移除法師無視防禦傷害細節。

### 2026-06-11T16:59:30+0800

- 更新 `docs/VISION.md`，將法師技能行為提升為共用規則。
- 記錄所有法師技能都無視防禦、不使用命中判定，且不能暴擊。
- 將兩個下一個原型法師技能保留為該共用法師技能規則下的具體例子。

### 2026-06-11T17:05:09+0800

- 更新 `docs/VISION.md`，補上剩餘原型戰鬥與狀態效果規則。
- 記錄下一個原型不會套用職業表數值以外的任何暴擊率 modifier。
- 記錄狀態效果不堆疊；重新套用同一效果只會重置剩餘持續時間。
- 記錄狀態效果立即套用，在效果持有者回合結束時遞減，並在剩餘 0 回合時移除。
- 更新 `docs/GOALS.md`，讓剩餘開放規劃項目聚焦在狀態效果數值與實作順序。

### 2026-06-11T17:10:24+0800

- 更新 `docs/VISION.md`，補上初始 buff 與 debuff 數值。
- 記錄攻擊與防禦 buff 為 1.5x，對應 debuff 為 0.5x。
- 記錄命中率 buff 與 debuff 為 +10 與 -10 percentage points。
- 記錄移動 buff 與 debuff 為 +1 與 -1 移動。
- 更新 `docs/GOALS.md`，讓實作順序成為唯一剩餘開放規劃項目。

### 2026-06-11T17:24:33+0800

- 更新 `docs/GOALS.md`，將第三個可玩目標定義為驗證職業差異。
- 記錄下一個原型的數值是用於觀察角色定位差異，不是最終平衡。
- 加入戰士、弓箭手、法師、攻擊射程、普通攻擊、法師 MP 與技能、UI 回饋的完成條件。
- 加入驗證問題，用於測試戰士前線使用、弓箭手距離管理、法師技能選擇與 MP 取捨是否可觀察。
- 更新 `docs/VISION.md`，記錄相同的下一個原型平衡意圖。

### 2026-06-11T17:31:21+0800

- 更新 `docs/GOALS.md`，加入第三個可玩目標的詳細實作計畫。
- 將下一階段拆分為職業資料基礎、單位數值整合、攻擊射程規則、普通攻擊公式、UI 回饋、法師技能、十字法術結算、狀態效果基礎與最終驗證。
- 記錄每個實作步驟的範圍、完成條件與排除範圍。
- 這次文件 pass 未開始任何 C# 實作。

### 2026-06-12T16:20:52+0800

- 實作第三個可玩目標的職業資料基礎。
- 加入戰士、弓箭手與法師的 `UnitClass` 識別。
- 加入 `AttackRange`、`UnitClassDefinition` 與 `UnitClassDefinitions`，讓職業 HP、MP、攻擊、防禦、命中係數、迴避、移動力、暴擊率與普通攻擊射程可以從 `docs/VISION.md` 原型表推導。
- 更新 `Unit`，讓單位可以從職業定義建立，同時保留既有戰鬥流程。
- 更新 `docs/GOALS.md`，標記職業資料基礎步驟完成。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-15T16:51:10+0800

- 實作第三個可玩目標的單位數值整合。
- 更新 `Unit` 以保存目前 HP 與目前 MP，而攻擊、防禦、命中係數、迴避、移動力、暴擊率與普通攻擊射程仍透過職業定義取得。
- 更新戰鬥配置，讓玩家與敵方隊伍都包含從職業資料建立的戰士、弓箭手與法師單位，而不是硬編碼 HP、攻擊與移動值。
- 更新 `docs/GOALS.md`，標記單位數值整合步驟完成。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。

### 2026-06-15T17:27:13+0800

- 實作第三個可玩目標的普通攻擊射程規則。
- 玩家攻擊現在使用每個單位的普通攻擊最小與最大射程，而不是只檢查相鄰。
- 無效射程嘗試現在會顯示單位的普通攻擊射程與目前目標距離。
- 敵方攻擊現在使用普通攻擊射程，且簡單敵方移動會在目標進入射程後停止。
- 更新 `docs/GOALS.md`，標記攻擊射程規則步驟完成。
- 使用 `dotnet format SRPG_practice.sln --verify-no-changes` 驗證格式。
- 使用 `dotnet build SRPG_practice.sln` 驗證專案：build succeeded，0 warnings，0 errors。
