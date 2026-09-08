# Windows API Code Pack for .NET

[![CI Build](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/ci.yml/badge.svg)](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/ci.yml)
[![Release](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/release.yml/badge.svg)](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/release.yml)
[![NuGet Version](https://img.shields.io/nuget/v/WindowsAPICodePackCore.svg)](https://www.nuget.org/packages/WindowsAPICodePackCore/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WindowsAPICodePackCore.svg)](https://www.nuget.org/packages/WindowsAPICodePackCore/)

Managed wrappers for selected Windows Shell and desktop APIs, originally released by Microsoft as the Windows API Code Pack 1.1 and maintained here for modern .NET.

**Active source:** [`Source/Current/Windows API CodePack`](Source/Current/Windows%20API%20CodePack)  
**License:** MIT — see [LICENSE](LICENSE)  
**Roadmap:** [Source/Current/ROADMAP.md](Source/Current/ROADMAP.md) · **Changes:** [Changelog.md](Changelog.md)

## Packages

| Package | Contents |
|---------|----------|
| [WindowsAPICodePack](https://www.nuget.org/packages/WindowsAPICodePack/) | Meta-package (all components) |
| [WindowsAPICodePackCore](https://www.nuget.org/packages/WindowsAPICodePackCore/) | Task Dialogs, power, network list, restart/recovery |
| [WindowsAPICodePackShell](https://www.nuget.org/packages/WindowsAPICodePackShell/) | Shell objects, Common File Dialogs, Explorer Browser, taskbar |
| [WindowsAPICodePackSensors](https://www.nuget.org/packages/WindowsAPICodePackSensors/) | Sensor platform |
| [WindowsAPICodePackExtendedLinguisticServices](https://www.nuget.org/packages/WindowsAPICodePackExtendedLinguisticServices/) | Extended Linguistic Services |
| [WindowsAPICodePackShellExtensions](https://www.nuget.org/packages/WindowsAPICodePackShellExtensions/) | Preview handlers and thumbnail providers |

**Target frameworks:** .NET Framework 4.6.2–4.8.1 and .NET 8 / 9 / 10 (`net*-windows`). Windows desktop (WinForms/WPF) only.

DirectX from the original Code Pack is **not shipped** in 8.x. The Current tree keeps an unbuilt stub; see [`Components/DirectX/README.md`](Source/Current/Windows%20API%20CodePack/Components/DirectX/README.md).

## Usage notes

### TaskDialog and comctl32 v6

If creating a `TaskDialog` throws `NotSupportedException` about comctl32.dll version 6, enable Common Controls v6 in the application manifest:

```xml
<dependency>
  <dependentAssembly>
    <assemblyIdentity
        type="win32"
        name="Microsoft.Windows.Common-Controls"
        version="6.0.0.0"
        processorArchitecture="*"
        publicKeyToken="6595b64144ccf1df"
        language="*" />
  </dependentAssembly>
</dependency>
```

Visual Studio can cache the old DLL in-process; restart the IDE if the error persists after adding the manifest.

### Authenticode signing

Optional during build. Disabled by default.

**Certificate file:**

```xml
<PropertyGroup>
  <EnableAuthenticodeSigning>true</EnableAuthenticodeSigning>
  <CodeSigningCertificatePath>path\to\your\certificate.pfx</CodeSigningCertificatePath>
  <CodeSigningCertificatePassword>your-password</CodeSigningCertificatePassword>
</PropertyGroup>
```

**Certificate store:**

```xml
<PropertyGroup>
  <EnableAuthenticodeSigning>true</EnableAuthenticodeSigning>
  <CodeSigningCertificateThumbprint>your-certificate-thumbprint</CodeSigningCertificateThumbprint>
</PropertyGroup>
```

Requires Windows SDK (`SignTool.exe`). The build continues with a warning if signing fails. For GitHub Actions, see [`.github/workflows/README.md`](.github/workflows/README.md) (`CODESIGN_CERTIFICATE_BASE64`, `CODESIGN_CERTIFICATE_PASSWORD`).

## Samples and original tree

- [`Source/Samples`](Source/Samples) — mix of modernized and legacy demos; prefer samples that reference `Source/Current`.
- [`Source/Original`](Source/Original) — archive of the Microsoft-era sources, tests, and full DirectX.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). Bug reports should include Windows version, TFM, and package version ([template](.github/ISSUE_TEMPLATE/bug_report.md)).
