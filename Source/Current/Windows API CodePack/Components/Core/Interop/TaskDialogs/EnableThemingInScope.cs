namespace Microsoft.WindowsAPICodePack.Core
{
    /// http://support.microsoft.com/kb/830033
    /// <devdoc>
    ///     This class is intended to use with the C# 'using' statement in
    ///     to activate an activation context for turning on visual theming at
    ///     the beginning of a scope, and have it automatically deactivated
    ///     when the scope is exited.
    /// </devdoc>

    [SuppressUnmanagedCodeSecurity]
    internal class EnableThemingInScope : IDisposable
    {
        // Private data
        private UIntPtr _cookie;
        private static ACTCTX _enableThemingActivationContext;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        private static IntPtr _hActCtx;

        private static bool _contextCreationSucceeded;

        public EnableThemingInScope(bool enable)
        {
            _cookie = UIntPtr.Zero;
            if (enable && OSFeature.Feature.IsPresent(OSFeature.Themes))
            {
                if (EnsureActivateContextCreated())
                {
                    if (!ActivateActCtx(_hActCtx, out _cookie))
                    {
                        // Be sure cookie always zero if activation failed
                        _cookie = UIntPtr.Zero;
                    }
                }
            }
        }

        ~EnableThemingInScope()
        {
            Dispose();
        }

        void IDisposable.Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        private void Dispose()
        {
            if (_cookie != UIntPtr.Zero)
            {
                try
                {
                    if (DeactivateActCtx(0, _cookie))
                    {
                        // deactivation succeeded...
                        _cookie = UIntPtr.Zero;
                    }
                }
                catch (SEHException)
                {
                    //Hopefully solved this exception
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity")]
        private static bool EnsureActivateContextCreated()
        {
            lock (typeof(EnableThemingInScope))
            {
                if (!_contextCreationSucceeded)
                {
                    // Pull manifest from the .NET Framework install
                    // directory

                    string? assemblyLoc;

                    FileIOPermission fiop = new FileIOPermission(PermissionState.None);
                    fiop.AllFiles = FileIOPermissionAccess.PathDiscovery;
                    fiop.Assert();
                    try
                    {
                        assemblyLoc = typeof(Object).Assembly.Location;
                    }
                    finally
                    {
                        CodeAccessPermission.RevertAssert();
                    }

                    string? manifestLoc = null;
                    string? installDir = null;
                    if (assemblyLoc != null)
                    {
                        installDir = Path.GetDirectoryName(assemblyLoc);
                        const string manifestName = "XPThemes.manifest";
                        manifestLoc = Path.Combine(installDir!, manifestName);
                    }

                    if (manifestLoc != null && installDir != null)
                    {
                        _enableThemingActivationContext = new ACTCTX();
                        _enableThemingActivationContext.cbSize = Marshal.SizeOf(typeof(ACTCTX));
                        _enableThemingActivationContext.lpSource = manifestLoc;

                        // Set the lpAssemblyDirectory to the install
                        // directory to prevent Win32 Side by Side from
                        // looking for comctl32 in the application
                        // directory, which could cause a bogus dll to be
                        // placed there and open a security hole.
                        _enableThemingActivationContext.lpAssemblyDirectory = installDir;
                        _enableThemingActivationContext.dwFlags = ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID;

                        // Note this will fail gracefully if file specified
                        // by manifestLoc doesn't exist.
                        _hActCtx = CreateActCtx(ref _enableThemingActivationContext);
                        _contextCreationSucceeded = (_hActCtx != new IntPtr(-1));
                    }
                }

                // If we return false, we'll try again on the next call into
                // EnsureActivateContextCreated(), which is fine.
                return _contextCreationSucceeded;
            }
        }

        // All the pinvoke goo...
        [DllImport("Kernel32.dll")]
        private extern static IntPtr CreateActCtx(ref ACTCTX actctx);
        [DllImport("Kernel32.dll")]
        private extern static bool ActivateActCtx(IntPtr hActCtx, out UIntPtr lpCookie);
        [DllImport("Kernel32.dll")]
        private extern static bool DeactivateActCtx(uint dwFlags, UIntPtr lpCookie);

        private const int ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID = 0x004;

        private struct ACTCTX
        {
            public int cbSize;
            public uint dwFlags;
            public string lpSource;
            public ushort wProcessorArchitecture;
            public ushort wLangId;
            public string lpAssemblyDirectory;
            public string lpResourceName;
            public string lpApplicationName;
        }
    }
}