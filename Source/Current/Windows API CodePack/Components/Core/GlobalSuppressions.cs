// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 


#region CA1709 Identifers should use proper casing - "API" acronym chosen for clarity
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.ApplicationServices", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.Net", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.Shell.PropertySystem", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.Dialogs", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "MS.WindowsAPICodePack.Internal", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.Shell.PropertySystem", MessageId = "API")]
#endregion


#region CA1709 - Identifiers should use proper casing - "v" for "Version" chosen for clarity
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv6Internet", MessageId = "Pv")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv6NoTraffic", MessageId = "Pv")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv4Subnet", MessageId = "Pv")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv4NoTraffic", MessageId = "Pv")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv4Internet", MessageId = "Pv")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv4LocalNetwork", MessageId = "Pv")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv6Subnet", MessageId = "Pv")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Net.ConnectivityStates.#IPv6LocalNetwork", MessageId = "Pv")]
#endregion

[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ApplicationServices.PowerPersonalityGuids.#All")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.PropVariant.#.cctor()")]

[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "Microsoft.WindowsAPICodePack.Resources.LocalizedMessages.resources", MessageId = "comctl")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "Microsoft.WindowsAPICodePack.Resources.LocalizedMessages.resources", MessageId = "Wh")]

#region CA1811 - Potentially uncalled code
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Shell.PropertySystem.PropVariantNativeMethods.#InitPropVariantFromPropVariantVectorElem(MS.WindowsAPICodePack.Internal.PropVariant,System.uint,MS.WindowsAPICodePack.Internal.PropVariant)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.DialogsDefaults.#get_Caption()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.DialogsDefaults.#get_Content()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreErrorHelper.#HResultFromWin32(System.Int32)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreErrorHelper.#Failed(System.Int32)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods+Size.#get_Height()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods+Size.#set_Height(System.Int32)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods+Size.#set_Width(System.Int32)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods+Size.#get_Width()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.DialogsDefaults.#get_MainInstruction()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialogSettings.#get_InvokeHelp()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialog.#get_SelectedRadioButtonId()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.TaskDialogNativeMethods+IconUnion.#get_MainIcon()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,MS.WindowsAPICodePack.Internal.WindowMessage,System.IntPtr,System.IntPtr)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,System.uint,System.IntPtr,System.String)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,System.uint,System.Int32,System.String)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods.#GetLoWord(System.Int64)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods.#GetHiWord(System.Int64,System.Int32)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods.#PostMessage(System.IntPtr,MS.WindowsAPICodePack.Internal.WindowMessage,System.IntPtr,System.IntPtr)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,System.uint,System.Int32&,System.Text.StringBuilder)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.CoreErrorHelper.#Matches(System.Int32,System.Int32)")]
#endregion


[assembly: SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant", Justification = "There are places where unsigned values are used, which is considered not Cls compliant.")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.Shell.PropertySystem", Justification = "Uses nested classes to organize the namespce, there is a workitem to resolve this.")]

[assembly: SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Scope = "type", Target = "Microsoft.WindowsAPICodePack.Dialogs.TaskDialogResult", Justification = "Does not represent flags.")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.PropVariant.#Value", Justification = "Uses a switch statement.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Justification = "These return snapshots of a state, which are likely to change between calls.", Target = "Microsoft.WindowsAPICodePack.ApplicationServices.PowerManager.#GetCurrentBatteryState()")]
[assembly: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Justification = "These return snapshots of a state, which are likely to change between calls.", Target = "Microsoft.WindowsAPICodePack.Net.NetworkListManager.#GetNetworkConnections()")]

#region CA2122, CA2123 - LinkDemand related
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.PropVariant.#.ctor(System.Single[])")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.PropVariant.#.ctor(System.Decimal[])")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ApplicationServices.ApplicationRestartRecoveryManager.#RegisterForApplicationRecovery(Microsoft.WindowsAPICodePack.ApplicationServices.RecoverySettings)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialog.#AllocateAndMarshalButtons(Microsoft.WindowsAPICodePack.Dialogs.TaskDialogNativeMethods+TaskDialogButton[])")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialog.#Dispose(System.Boolean)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialog.#FreeOldString(Microsoft.WindowsAPICodePack.Dialogs.TaskDialogNativeMethods+TaskDialogElements)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialog.#MakeNewString(System.String,Microsoft.WindowsAPICodePack.Dialogs.TaskDialogNativeMethods+TaskDialogElements)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialog.#NativeShow()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.NativeTaskDialogSettings.#.ctor()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.PropVariant.#GetBlobData()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.PropVariant.#.ctor(System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.PropVariant.#Value")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.SafeWindowHandle.#ReleaseHandle()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ApplicationServices.Power.#GetSystemBatteryState()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ApplicationServices.Power.#GetSystemPowerCapabilities()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.ZeroInvalidHandle.#.ctor()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.SafeIconHandle.#ReleaseHandle()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.SafeRegionHandle.#ReleaseHandle()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.SafeWindowHandle.#ReleaseHandle()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "MS.WindowsAPICodePack.Internal.ZeroInvalidHandle.#IsInvalid")]
#endregion

[assembly: SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Scope = "member", Target = "Microsoft.WindowsAPICodePack.Dialogs.TaskDialogNativeMethods.#TaskDialogIndirect(Microsoft.WindowsAPICodePack.Dialogs.TaskDialogNativeMethods+TaskDialogConfiguration,System.Int32&,System.Int32&,System.Boolean&)", Justification="This does exist, I believe it is caused by FxCop using the wrong version of the DLL.")]
