# DirectX component (not shipped)

This folder is a **Win32 stub DLL**, not the original C++/CLI DirectX wrappers from the 2009 Code Pack.

- It is **not** included in any NuGet package (`WindowsAPICodePack*` 8.x).
- CI does **not** build this project.
- The full historical implementation lives under `Source/Original`.

For new work, use [Vortice](https://github.com/amerkoleci/Vortice.Windows), Windows App SDK / Win2D, or another maintained DirectX/.NET stack. SharpDX is unmaintained.
