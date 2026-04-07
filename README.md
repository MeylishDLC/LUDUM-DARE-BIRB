# LUDUM-DARE-BIRB

## Replay System: Quick Start

This project has a built-in replay flow in `Assets/_Source/Replay`.
Replay controls are shown as an on-screen debug overlay (not a separate in-game console window).

### How to run and use replay

1. Open the project in Unity.
2. Open your gameplay scene (the one with `MainSceneInstaller` and `ReplayCoordinator` in runtime bindings).
3. Enter Play Mode.
4. In the top-left corner, use buttons:
   - `record_start` - starts recording input commands.
   - `record_stop` - stops recording and stores the replay in memory.
   - `replay_play_last` - reloads current scene and starts playback of the last recorded replay.

## How replay works (important)

- Replay data is recorded as command stream (`Move`, `Action`, `Select`) by tick.
- Tick rate is fixed at `50 Hz`.
- Max recording length is `30` seconds.
- Replay uses deterministic seed-based RNG (`IRng` / `SeededRng`) to keep gameplay reproducible.
- Last replay is stored in `ReplayCoordinator.LastReplay`.
- For playback after scene reload, data/seed are passed via `ReplaySession`.

## "Console" and logs

### Unity Console window

In Unity Editor:

- Open via menu: `Window -> General -> Console`
- Shortcut (default): `Ctrl + Shift + C`

You can filter by text `Replay::` to find replay-related warnings/messages.

### Log files location (Windows)

If you need persistent logs outside Unity Console:

- **Editor log**: `%LOCALAPPDATA%\Unity\Editor\Editor.log`
- **Player build log**: `%USERPROFILE%\AppData\LocalLow\DefaultCompany\LUDUM-DARE-BIRB\Player.log`

## Where to find replay recording file?

At the moment, there is **no replay file export** implemented.

- Recorded replay is kept in memory only (`ReplayData` object).
- Current implementation does not write `.json`, `.bytes`, or any other replay file to disk.

If needed, add serialization for `ReplayData` and save/load it explicitly.

