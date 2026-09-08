# Contributing

Active development is under `Source/Current/Windows API CodePack`. `Source/Original` is an archive (legacy tests, full DirectX, original samples).

## Build

```powershell
dotnet restore "Source/Current/Windows API CodePack/Windows API CodePack.sln"
dotnet build "Source/Current/Windows API CodePack/Windows API CodePack.sln" -c Release
dotnet test "Source/Current/Windows API CodePack/Windows API CodePack.sln" -c Release
```

Requires a Windows machine (WinForms/WPF/COM Shell APIs). Supported library TFMs: `net462`–`net481` and `net8.0-windows` / `net9.0-windows` / `net10.0-windows`.

## Pull requests

- Prefer small, focused changes with tests when the behavior is unit-testable.
- Do not add machine-specific HintPaths or commit `bin`/`obj` outputs.
- DirectX in Current is an unshipped stub; do not expand it. See `Source/Current/Windows API CodePack/Components/DirectX/README.md`.
- Update `Changelog.md` for user-visible fixes and features.

See [Source/Current/ROADMAP.md](Source/Current/ROADMAP.md) for planned work.
