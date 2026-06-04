# Project Agent Rules

## Language

- Prefer Traditional Chinese and Taiwan wording when responding to the user.
- Use English only when it is clearer for code, APIs, file names, or technical terms.

## Coding Style

- For simple guard clauses or early returns, omit braces and keep the whole statement on a single line when readability remains clear, for example `if (unit.Hp == 0) return;`.

## Progress Tracking

When the user asks to "紀錄目前專案狀態", "記錄目前專案狀態", "更新目前進度", or makes an equivalent request:

1. Read `docs/PROGRESS.md` and `docs/GOALS.md` first.
2. Update `docs/PROGRESS.md` with a short summary of the current project status.
3. Every new progress entry must include a timestamp.
   - Prefer ISO-like local time, for example `2026-05-26T16:00:00+0800`.
   - Use `date +%Y-%m-%dT%H:%M:%S%z` when checking the current time.
4. Keep progress notes concise and summary-based.
   - Do not paste long command output.
   - Record what changed, what was verified, and what remains next.
5. If any goal or todo item is completed, update the matching item in `docs/GOALS.md`.
   - Mark completed items with `[x]` when the list uses checkboxes.
   - If the list does not use checkboxes, add a short completion note near the corresponding item.
   - Keep the original goal wording unless a real scope change happened.

## Progress Reporting

When the user asks about the current project progress:

1. Read `docs/PROGRESS.md` and `docs/GOALS.md` before answering.
2. Base the response primarily on those two files.
3. Mention any mismatch between the files and the actual repository state if discovered.
4. Keep the answer short:
   - Current status
   - Completed items
   - Next recommended step

## Goals Document

- `docs/GOALS.md` is the source of truth for prototype goals and todo items.
- Update it only when goals change or a todo item is completed.

## Progress Document

- `docs/PROGRESS.md` is the source of truth for chronological project progress.
- New status updates should be appended or added in a clearly dated section.
- Prefer summaries over detailed logs.
