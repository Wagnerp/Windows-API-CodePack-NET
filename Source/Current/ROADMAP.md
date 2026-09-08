# Windows API Code Pack — Current Codebase Roadmap

**Scope:** `Source/Current/Windows API CodePack`  
**Audit date:** 2026-07-19  
**Library version at audit:** 8.0.15.2  

This document is an audit-driven roadmap for improving, modernizing, and sustaining the active Code Pack. Items are prioritized **P0 → P2** and grouped by theme. Effort is approximate (S / M / L).

---

## Executive summary

`Current` is a solid multi-TFM modernization of the classic Windows API Code Pack (SDK-style projects, nullable, .NET 8–10, NuGet packaging, CI/release pipelines). The largest gaps are **no automated tests**, **legacy interop and UI coupling**, a **DirectX stub that misleads consumers**, and **ecosystem debt** (stale docs, broken samples, inconsistent package metadata).

| Area | Health | Headline |
|------|--------|----------|
| Multi-TFM / packaging | Good | net462–481 + net8/9/10-windows; signed packages; meta-package |
| CI / release | Good− | Mature workflows; tests and some validation are no-ops/fragile |
| Code quality | Mixed | Nullable + XML docs on; heavy COM/`DllImport`/`Hashtable` legacy |
| Tests | Poor | Zero tests under Current |
| Samples / docs | Poor | Many samples broken; root README stale |
| DirectX | Deprecated | Empty native stub; not packaged |

**Preserve:** TaskDialog, CommonFileDialogs, ExplorerBrowser (incl. search), Taskbar/JumpList/TabbedThumbnails, Shell property system, Sensors, ShellExtensions preview/thumbnail handlers, Authenticode signing support.

---

## Component map

| Component | Role | ~CS files | Notes |
|-----------|------|-----------|-------|
| **Core** | TaskDialog, power, network, AppRestartRecovery, PropVariant, SafeHandles | ~124 | `UseWindowsForms` may be broader than needed |
| **Shell** | Shell objects, dialogs, ExplorerBrowser, Taskbar, KnownFolders, DWM | ~335 | Largest surface; WPF + hard-coded WinForms refs |
| **ShellExtensions** | Preview / thumbnail COM handlers | ~69 | Same WinForms HintPath pattern as Shell |
| **Sensors** | Sensor API wrappers | ~93 | Recent NRE fix (#47) |
| **ExtendedLinguisticServices** | ELS / mapping services | ~75 | Niche; still useful |
| **WindowsAPICodePack** | Meta NuGet (all DLLs) | — | Best “one package” story |
| **DirectX** | Stub DLL | 0 CS | Not shipped; CI still builds it |
| **BugTest / ScratchProject** | Manual repro apps | — | Should not stay in shipping solution long-term |

---

## P0 — Critical (trust, correctness, consumer clarity)

### 1. Add automated tests under Current
**Effort:** L · **Why:** No regression gate; CI “Run tests” is effectively a no-op (`continue-on-error`). Recent bugs (ShellThumbnail COM lifetime, Sensors NRE, CommonFileDialog extension/name, ExplorerBrowser focus) are high-value regression cases.

**Actions:**
- Create `Source/Current/Windows API CodePack/Tests/` (xUnit or NUnit), TFMs at least `net48` + `net10.0-windows`.
- Port viable tests from `Source/Original/source/Tests/` (PropVariant, PropertyKey, KnownFolders, Sensors metadata, ELS).
- Add focused regression tests for Changelog-fixed issues (8.0.12–8.0.15.x).
- Wire into `Windows API CodePack.slnx` and make CI fail on test failure.

### 2. Fix Shell / ShellExtensions WinForms references
**Effort:** S · **Why:** Hard-coded HintPaths to .NET Framework 4.8.1 reference assemblies break portable/CI-clean builds and fight SDK-style TFMs:

```xml
<!-- Shell.csproj / ShellExtensions.csproj today -->
<Reference Include="System.Windows.Forms">
  <HintPath>C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8.1\...</HintPath>
</Reference>
```

**Actions:**
- Prefer `<UseWindowsForms>true</UseWindowsForms>` (and keep `UseWPF`) instead of absolute HintPaths.
- Verify multi-TFM build (net462–net481 and net*-windows) without machine-specific paths.

### 3. Clarify / retire DirectX in Current
**Effort:** S · **Why:** `Components/DirectX` is an empty Win32 DLL stub (not C++/CLI). Root README still describes C++/CLI AnyCPU limits. Consumers are misled; CI spends matrix time building it; toolset mismatch (project v145 vs workflows often v143).

**Actions:**
- Document clearly: DirectX is **not** in NuGet 8.x; recommend SharpDX / Vortice / Windows App SDK / Win2D as appropriate.
- Prefer removing stub + CI steps from Current (keep Original as archive), or stop building it in CI if retained for solution compatibility.

### 4. Rewrite consumer-facing documentation
**Effort:** M · **Why:** Root `README.md` and component `Readme.md` files still read like the Microsoft-era pack (empty “Current Version”, 64-bit exception headline, DirectX C++/CLI notes). License on disk is MIT (Wagner modifications); README still hedges as if unclear.

**Actions:**
- Document: active path (`Source/Current`), NuGet IDs, supported TFMs, Windows requirements, samples status.
- Point to `Changelog.md`, this roadmap, and workflow docs.
- Refresh NuGet package **Description** fields (several still say “Windows Forms Development in the 1.1 version, released by Microsoft in 2009” despite .NET 8–10 support).
- Fix package metadata inconsistencies (`RepositoryUrl` is literally `git` on Core/meta; Authors = package title).

### 5. Fix fragile CI packaging validation
**Effort:** S · **Why:** `package-validation` packing with `--no-build` in a separate job without artifacts is unreliable; PR nupkg “zip” validation is suspect.

**Actions:**
- Pack in the build job or pass artifacts between jobs.
- Validate packages with `dotnet nuget` / proper tooling, not ad-hoc zip assumptions.

---

## P1 — High value (maintainability & modernization)

### 6. Modernize P/Invoke surface
**Effort:** L · **Evidence:** ~127 `DllImport`, **0** `LibraryImport`; only **4** SafeHandle types; ~62 `Marshal.ReleaseComObject`.

**Actions:**
- Migrate declarations to `[LibraryImport]` / source-generated interop where TFMs allow (net8+), with dual-path or polyfills for net462+.
- Evaluate **CsWin32** for new/changed APIs to reduce hand-written marshalling bugs.
- Expand SafeHandle usage (icons, windows, GDI objects, shell item lifetimes) beyond `SafeIconHandle` / `SafeWindowHandle` / `SafeRegionHandle`.
- Prefer `Marshal.FinalReleaseComObject` / careful RCW ownership patterns; audit sites that release COM while still referenced (ShellThumbnail parent-ref pattern is the template).

### 7. Reduce UI-framework coupling
**Effort:** L · **Why:** Core/Sensors/ELS force `UseWindowsForms`; Shell forces WPF + WinForms. That pulls desktop UI stacks into consumers who only want TaskDialog, Jump Lists, or KnownFolders.

**Actions (incremental):**
- Split or multi-target: e.g. `Core` APIs that don’t need WinForms vs dialog hosts.
- Consider optional packages: `WindowsAPICodePack.Shell.WinForms` / `.Wpf` vs a thinner Shell.Interop core.
- Ensure `System.Drawing` usage is intentional (`System.Drawing.Common` implications on modern .NET).

### 8. Replace legacy collections & CAS leftovers
**Effort:** M · **Evidence:** ~38 `Hashtable` usages (notably generated `StronglyTypedProperties.cs`), ~6 `ArrayList`; `PowerManager` disables `SYSLIB0003` (CAS); obsolete `ClassInterfaceType.AutoDual` on ExplorerBrowser view events; ~982 SuppressMessage hits (many legacy FxCop/LinkDemand).

**Actions:**
- Replace `Hashtable`/`ArrayList` with `Dictionary<,>` / `List<>` in hand-written code; regenerate property wrappers with modern collections.
- Remove CAS/`SecurityPermission` demands where obsolete on .NET Core+; keep netfx behavior documented if needed.
- Prune `GlobalSuppressions` (Shell’s file alone ~199 KB) — many CA2122/LinkDemand rules are irrelevant on modern runtimes.

### 9. Address known incomplete / risky Shell APIs
**Effort:** M · **Examples:**
- `StronglyTypedProperties.cs` — **~14k lines**, marked `// TODO: FIX THIS!!!`, Hashtable-backed cache.
- `SystemProperties.cs` — **~10k lines** of generated property keys; keep as generated, improve generator.
- `ShellItemArrayWrapper` — most `IShellItemArray` members throw `NotImplementedException`.
- `ShellLibrary` — several `NotImplementedException` paths.
- `TaskbarWindow.WindowToTellTaskbarAbout` — open TODO: throws `InvalidOperationException` in valid construction races.
- `TabbedThumbnailManager` — TODO around `SetImage(IntPtr.Zero)`.
- `IEntity.cs` — empty `// TODO`.

**Actions:**
- Triage: implement, obsolete, or document “partial COM surface” per type.
- Fix TaskbarWindow title/proxy logic with tests (aligns with active TabbedThumbnail work).
- Move property-system generation to an explicit tool/script in-repo so `FIX THIS` isn’t tribal knowledge.

### 10. Migrate and catalog samples
**Effort:** L · **Why:** Mix of modernized SDK samples and broken .NET 3.5 / wrong project-reference samples.

**Actions:**
- Add `Source/Samples/README.md` with status matrix (✅ / 🔧 / ❌).
- Fix broken ExplorerBrowser demos that reference non-existent split Shell projects.
- Prioritize migrating: TaskDialog, CommonFileDialogs, Taskbar, KnownFolders, ShellThumbnail.
- Add `net10.0-windows` to modernized samples for parity.
- Optional CI job building a curated samples solution.

### 11. Retire BugTest / ScratchProject from the product solution
**Effort:** S · **Why:** Manual repro harnesses duplicate ShellThumbnail scenarios; not automated tests.

**Actions:**
- Fold scenarios into Tests; move leftovers to an internal/samples folder or delete.
- Keep `IsPackable=false`; exclude from release packaging.

### 12. Align versioning story
**Effort:** S · **Why:** Packages use 4-part versions (`8.0.15.2`); docs describe semver 3-part; version-increment workflow examples are stale.

**Actions:**
- Pick one scheme and document it in `.github/VERSIONING.md`.
- Keep `Directory.Build.props` / targets / NuGet / assembly versions aligned (already a past pain point).

### 13. Security & contribution hygiene
**Effort:** S  
**Actions:**
- Replace boilerplate `SECURITY.md` with reporting channel + supported versions.
- Add `CONTRIBUTING.md` (build Current, TFMs, samples policy, PR expectations).
- Windows/.NET-focused issue templates (OS build, TFM, package version, minimal repro).

### 14. Local build parity
**Effort:** S  
**Actions:**
- Add root `build.ps1` (`restore` / `build` / `test` / `pack` / optional sign) matching CI.
- Document dual `.sln` vs `.slnx` or consolidate.

### 15. Dependency cleanup
**Effort:** S  
**Actions:**
- Revisit `Microsoft.NETCore.Platforms` 7.0.4 on Core/Sensors/ELS — often unnecessary on modern SDKs.
- Enable consistent analyzers (`AnalysisLevel`, nullable warnings as errors in CI for net8+ first).

---

## P2 — Enhancements & future API surface

### 16. Feature enhancements (product)
| Idea | Notes |
|------|--------|
| **Win11 taskbar / Jump List polish** | Validate against current Shell behavior; document limitations vs WinUI/WASDK |
| **IFileDialog customization** | More CommonFileDialog controls / events parity with native dialogs |
| **ExplorerBrowser** | Continue search/navigation/event work (#14, #20, #21, #40); keyboard/focus edge cases |
| **Shell property keys** | Refresh PKEY set for newer Windows properties; keep generation pipeline |
| **Async APIs** | Optional `*Async` wrappers for long Shell ops / ELS mapping where useful |
| **WinRT / Windows App SDK interop notes** | Not necessarily reimplement — document when to use WASDK vs Code Pack |
| **Dark mode / DWM** | Optional helpers for modern glass/backdrop where DWM APIs still apply |
| **AOT / trimming guidance** | Library is COM-heavy; publish trim/AOT warnings and supported scenarios rather than claiming full AOT |

### 17. Packaging / architecture experiments
- Source-link + deterministic builds verification.
- Separate “interop-only” package without WPF/WinForms for advanced consumers.
- Public API analyzers (`Microsoft.CodeAnalysis.PublicApiAnalyzers`) to catch breaking changes intentionally (PR workflow already has heuristics).

### 18. ELS & Sensors longevity
- Document Windows platform support (Sensors API availability varies).
- Keep Sensors enumeration defensive (post-#47).
- ELS: consider deprecation notice if usage is near-zero, or refresh samples.

### 19. VB samples policy
- Migrate top VB demos **or** formally deprecate and link C# equivalents.

### 20. Housekeeping
- Remove deprecated `build.yml` if unused; refresh `WORKFLOW_STATUS.md`.
- Deduplicate component NuGet readmes (component-specific content, not copy-paste root).
- Archive note at top of `Source/Original` (“read-only; tests/DirectX live here historically”).

---

## Suggested phasing

### Phase A — Stabilize (in progress)

1. Tests + blocking CI  
2. Shell WinForms HintPath fix  
3. DirectX docs/removal from CI  
4. README + NuGet description/metadata fix  
5. CI pack validation fix  

### Phase B — Harden (2–4 releases)
6. Port Original tests + regression suite  
7. TaskbarWindow / TabbedThumbnail TODOs  
8. Sample migration wave 1  
9. LibraryImport pilot on Core native methods  
10. Retire ScratchProject/BugTest  

### Phase C — Modernize (ongoing)
11. Property-system generator rewrite  
12. UI framework package split exploration  
13. Suppressions / Hashtable cleanup  
14. Optional async / Win11 polish  
15. Public API analyzer + samples CI  

---

## Metrics to track

| Metric | Current (audit) | Target |
|--------|-----------------|--------|
| Automated tests in Current | 0 | Growing suite; CI required |
| `LibraryImport` usage | 0 | Increasing share of new/changed P/Invokes |
| Broken samples (known) | Large share of `Source/Samples` | Curated set all ✅ |
| Hard-coded Framework HintPaths | 2 projects | 0 |
| DirectX in Current CI | Built | Removed or documented-only |
| Package Description accuracy | Stale (2009-era) | Matches TFMs & features |
| Open `TODO` / incomplete COM stubs | Several known | Triaged (fixed or documented) |

---

## Strengths to keep

- Active maintenance and Changelog discipline (ExplorerBrowser search, Authenticode, .NET 10, real bugfixes).
- Broad TFM matrix for desktop apps still on .NET Framework **and** modern .NET.
- Coherent component split + unified meta-package.
- XML documentation generation enabled on library projects.
- Nullable reference types enabled across components.
- Strong-name + optional Authenticode story for enterprise consumers.

---

## Out of scope / non-goals (recommended)

- Full reimplementation of Original DirectX C++/CLI in Current.
- Competing with Windows App SDK / WinUI for new app models — **interop and desktop WinForms/WPF** remain the niche.
- Breaking public API churn without PublicApi analyzer + major version bump.

---

## Quick reference paths

```
Source/Current/Windows API CodePack/
├── Components/
│   ├── Core/
│   ├── Shell/
│   ├── ShellExtensions/
│   ├── Sensors/
│   ├── ExtendedLinguisticServices/
│   ├── WindowsAPICodePack/          # meta-package
│   └── DirectX/                     # stub — decide fate
├── BugTest/                         # manual repro
├── ScratchProject/                  # manual repro
├── Directory.Build.props|targets
└── Windows API CodePack.sln(x)

Related (outside Current, but roadmap-relevant):
├── Source/Original/source/Tests/    # legacy tests to port
├── Source/Samples/                  # mixed modern/broken
├── .github/workflows/               # CI/release
├── README.md / Changelog.md / SECURITY.md
└── Directory.Build.props            # LibraryVersion
```

---

*This roadmap is a living document. Update priorities as releases ship and as DirectX / samples / test decisions are made.*
