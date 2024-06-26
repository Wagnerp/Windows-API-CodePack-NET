﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable MergeSequentialChecks
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable MergeConditionalExpression
namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
    /// <summary>
    /// Defines a strongly-typed property object. 
    /// All writable property objects must be of this type 
    /// to be able to call the value setter.
    /// </summary>
    /// <typeparam name="T">The type of this property's value. 
    /// Because a property value can be empty, only nullable types 
    /// are allowed.</typeparam>
    public class ShellProperty<T> : IShellProperty
    {
        #region Private Fields

        private PropertyKey _propertyKey;
        string? _imageReferencePath;
        int? _imageReferenceIconIndex;
        private readonly ShellPropertyDescription? _description;

        #endregion

        #region Private Methods

        private ShellObject? ParentShellObject { get; set; }

        private IPropertyStore? NativePropertyStore { get; set; }

        private void GetImageReference()
        {
            IPropertyStore? store = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);

            using (PropVariant propVar = new())
            {
                store.GetValue(ref _propertyKey, propVar);

                if (store != null)
                {
                    Marshal.ReleaseComObject(store);
                }
                store = null;

                string? refPath;
                ((IPropertyDescription2)Description?.NativePropertyDescription!).GetImageReferenceForValue(
                    propVar, out refPath);

                if (refPath == null) { return; }

                int index = ShellNativeMethods.PathParseIconLocation(ref refPath);
                if (refPath != null)
                {
                    _imageReferencePath = refPath;
                    _imageReferenceIconIndex = index;
                }
            }
        }

        private void StorePropVariantValue(PropVariant propVar)
        {
            Guid guid = new(ShellIIDGuid.IPropertyStore);
            IPropertyStore? writablePropStore = null;
            try
            {
                int hr = ParentShellObject!.NativeShellItem2!.GetPropertyStore(
                        ShellNativeMethods.GetPropertyStoreOptions.ReadWrite,
                        ref guid,
                        out writablePropStore);

                if (!CoreErrorHelper.Succeeded(hr))
                {
                    throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty,
                        Marshal.GetExceptionForHR(hr));
                }

                if (writablePropStore != null)
                {
                    HResult result = writablePropStore.SetValue(ref _propertyKey, propVar);

                    if (!AllowSetTruncatedValue && (int)result == ShellNativeMethods.InPlaceStringTruncated)
                    {
                        throw new ArgumentOutOfRangeException(nameof(propVar), LocalizedMessages.ShellPropertyValueTruncated);
                    }

                    if (!CoreErrorHelper.Succeeded(result))
                    {
                        throw new PropertySystemException(LocalizedMessages.ShellPropertySetValue, Marshal.GetExceptionForHR((int)result));
                    }
                }

                writablePropStore?.Commit();

            }
            catch (InvalidComObjectException? e)
            {
                throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, e);
            }
            catch (InvalidCastException)
            {
                throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty);
            }
            finally
            {
                if (writablePropStore != null)
                {
                    Marshal.ReleaseComObject(writablePropStore);
                }
            }
        }

        #endregion

        #region Internal Constructor

        /// <summary>
        /// Constructs a new Property object
        /// </summary>
        /// <param name="propertyKey"></param>
        /// <param name="description"></param>
        /// <param name="parent"></param>
        internal ShellProperty(
            PropertyKey propertyKey,
            ShellPropertyDescription? description,
            ShellObject? parent)
        {
            _propertyKey = propertyKey;
            _description = description;
            ParentShellObject = parent;
            AllowSetTruncatedValue = false;
        }

        /// <summary>
        /// Constructs a new Property object
        /// </summary>
        /// <param name="propertyKey"></param>
        /// <param name="description"></param>
        /// <param name="propertyStore"></param>
        internal ShellProperty(
            PropertyKey propertyKey,
            ShellPropertyDescription? description,
            IPropertyStore? propertyStore)
        {
            _propertyKey = propertyKey;
            _description = description;
            NativePropertyStore = propertyStore;
            AllowSetTruncatedValue = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the strongly-typed value of this property.
        /// The value of the property is cleared if the value is set to null.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException">
        /// If the property value cannot be retrieved or updated in the Property System</exception>
        /// <exception cref="NotSupportedException">If the type of this property is not supported; e.g. writing a binary object.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="AllowSetTruncatedValue"/> is false, and either 
        /// a string value was truncated or a numeric value was rounded.</exception>        
        public T? Value
        {
            get
            {
                // Make sure we load the correct type
                Debug.Assert(ValueType == ShellPropertyFactory.VarEnumToSystemType(Description!.VarEnumType));

                using (PropVariant propVar = new())
                {
                    if (ParentShellObject?.NativePropertyStore! != null)
                    {
                        // If there is a valid property store for this shell object, then use it.
                        ParentShellObject.NativePropertyStore.GetValue(ref _propertyKey, propVar);
                    }
                    else if (ParentShellObject != null)
                    {
                        // Use IShellItem2.GetProperty instead of creating a new property store
                        // The file might be locked. This is probably quicker, and sufficient for what we need
                        ParentShellObject?.NativeShellItem2!.GetProperty(ref _propertyKey, propVar);
                    }
                    else if (NativePropertyStore != null)
                    {
                        NativePropertyStore?.GetValue(ref _propertyKey, propVar);
                    }

                    //Get the value
                    return propVar.Value != null ? (T)propVar.Value : default(T);
                }
            }
            set
            {
                // Make sure we use the correct type
                Debug.Assert(ValueType == ShellPropertyFactory.VarEnumToSystemType(Description!.VarEnumType));

                if (typeof(T) != ValueType)
                {
                    throw new NotSupportedException(
                        string.Format(CultureInfo.InvariantCulture,
                        LocalizedMessages.ShellPropertyWrongType, ValueType.Name));
                }

                if (value is Nullable)
                {
                    Type t = typeof(T);
                    PropertyInfo? pi = t.GetProperty("HasValue");
                    if (pi != null)
                    {
                        bool hasValue = (bool)pi.GetValue(value, null)!;
                        if (!hasValue)
                        {
                            ClearValue();
                            return;
                        }
                    }
                }
                else if (value == null)
                {
                    ClearValue();
                    return;
                }

                if (ParentShellObject != null)
                {
                    using (ShellPropertyWriter propertyWriter = ParentShellObject?.Properties!.GetPropertyWriter()!)
                    {
                        propertyWriter.WriteProperty(this, value, AllowSetTruncatedValue);
                    }
                }
                else if (NativePropertyStore != null)
                {
                    throw new InvalidOperationException(LocalizedMessages.ShellPropertyCannotSetProperty);
                }
            }
        }


        #endregion

        #region IProperty Members

        /// <summary>
        /// Gets the property key identifying this property.
        /// </summary>
        public PropertyKey PropertyKey => _propertyKey;

        /// <summary>
        /// Returns a formatted, Unicode string representation of a property value.
        /// </summary>
        /// <param name="format">One or more of the PropertyDescriptionFormat flags 
        /// that indicate the desired format.</param>
        /// <param name="formattedString">The formatted value as a string, or null if this property 
        /// cannot be formatted for display.</param>
        /// <returns>True if the method successfully locates the formatted string; otherwise 
        /// False.</returns>
        public bool TryFormatForDisplay(PropertyDescriptionFormatOptions format, out string? formattedString)
        {


            if (Description == null || Description.NativePropertyDescription == null)
            {
                // We cannot do anything without a property description
                formattedString = null;
                return false;
            }

            IPropertyStore? store = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);

            using PropVariant propVar = new PropVariant();
            store?.GetValue(ref _propertyKey, propVar);

            // Release the Propertystore
            if (store != null)
            {
                Marshal.ReleaseComObject(store);
            }

            HResult hr = Description.NativePropertyDescription.FormatForDisplay(propVar, ref format, out formattedString);

            // Sometimes, the value cannot be displayed properly, such as for blobs
            // or if we get argument exception
            if (!CoreErrorHelper.Succeeded(hr))
            {
                formattedString = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a formatted, Unicode string representation of a property value.
        /// </summary>
        /// <param name="format">One or more of the PropertyDescriptionFormat flags 
        /// that indicate the desired format.</param>
        /// <returns>The formatted value as a string, or null if this property 
        /// cannot be formatted for display.</returns>
        public string? FormatForDisplay(PropertyDescriptionFormatOptions format)
        {
            string? formattedString;

            if (Description == null || Description.NativePropertyDescription == null)
            {
                // We cannot do anything without a property description
                return null;
            }

            IPropertyStore? store = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);

            using (PropVariant propVar = new())
            {
                store.GetValue(ref _propertyKey, propVar);

                // Release the Propertystore
                if (store != null)
                {
                    Marshal.ReleaseComObject(store);
                }

                HResult hr = Description.NativePropertyDescription.FormatForDisplay(propVar, ref format, out formattedString);

                // Sometimes, the value cannot be displayed properly, such as for blobs
                // or if we get argument exception
                if (!CoreErrorHelper.Succeeded(hr))
                    throw new ShellException(hr);

                return formattedString;
            }
        }

        /// <summary>
        /// Get the property description object.
        /// </summary>
        public ShellPropertyDescription? Description => _description;

        /// <summary>
        /// Gets the case-sensitive name of a property as it is known to the system,
        /// regardless of its localized name.
        /// </summary>
        public string CanonicalName => Description?.CanonicalName!;

        /// <summary>
        /// Clears the value of the property.
        /// </summary>
        public void ClearValue()
        {
            using (PropVariant propVar = new())
            {
                StorePropVariantValue(propVar);
            }
        }

        /// <summary>
        /// Gets the value for this property using the generic Object type.
        /// To obtain a specific type for this value, use the more type strong
        /// Property&lt;T&gt; class.
        /// Also, you can only set a value for this type using Property&lt;T&gt;
        /// </summary>
        public object? ValueAsObject
        {
            get
            {
                using (PropVariant propVar = new())
                {
                    if (ParentShellObject != null)
                    {

                        IPropertyStore? store = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);

                        store.GetValue(ref _propertyKey, propVar);

                        if (store != null)
                        {
                            Marshal.ReleaseComObject(store);
                        }
                    }
                    else
                    {
                        NativePropertyStore?.GetValue(ref _propertyKey, propVar);
                    }

                    return propVar != null ? propVar.Value : null;
                }
            }
        }

        /// <summary>
        /// Gets the associated runtime type.
        /// </summary>
        public Type ValueType
        {
            get
            {
                // The type for this object need to match that of the description
                Debug.Assert(Description?.ValueType! == typeof(T));

                return Description?.ValueType!;
            }
        }

        /// <summary>
        /// Gets the image reference path and icon index associated with a property value (Windows 7 only).
        /// </summary>
        public IconReference IconReference
        {
            get
            {
                if (!CoreHelpers.RunningOnWin7OrHigher)
                {
                    throw new PlatformNotSupportedException(LocalizedMessages.ShellPropertyWindows7);
                }

                GetImageReference();
                int index = (_imageReferenceIconIndex.HasValue ? _imageReferenceIconIndex.Value : -1);

                return new IconReference(_imageReferencePath, index);
            }
        }

        /// <summary>
        /// Gets or sets a value that determines if a value can be truncated. The default for this property is false.
        /// </summary>
        /// <remarks>
        /// An <see cref="ArgumentOutOfRangeException"/> will be thrown if
        /// this property is not set to true, and a property value was set
        /// but later truncated. 
        /// 
        /// </remarks>
        public bool AllowSetTruncatedValue { get; set; }

        #endregion
    }
}
