﻿// Copyright (c) Microsoft Corporation.  All rights reserved.

#pragma warning disable CS8613, CS8614, CS8603, CS8602, CS8601
namespace Microsoft.WindowsAPICodePack.Sensors
{
    /// <summary>
    /// Defines a mapping of data field identifiers to the data returned in a sensor report.
    /// </summary>    
    public class SensorData : IDictionary<Guid, IList<object>>
    {
        #region implementation
        internal static SensorData? FromNativeReport(ISensor? iSensor, ISensorDataReport iReport)
        {
            SensorData? data = new();

            IPortableDeviceKeyCollection? keyCollection;
            IPortableDeviceValues? valuesCollection;
            iSensor.GetSupportedDataFields(out keyCollection);
            iReport.GetSensorValues(keyCollection, out valuesCollection);

            uint items = 0;
            if (keyCollection != null)
            {
                keyCollection.GetCount(out items);
                for (uint index = 0; index < items; index++)
                {
                    PropertyKey key;
                    using (PropVariant propValue = new())
                    {
                        keyCollection.GetAt(index, out key);
                        if (valuesCollection != null) valuesCollection.GetValue(ref key, propValue);

                        if (data.ContainsKey(key.FormatId))
                        {
                            data[key.FormatId].Add(propValue.Value);
                        }
                        else
                        {
                            data.Add(key.FormatId, new List<object?> { propValue.Value });
                        }
                    }
                }

                if (keyCollection != null)
                {
                    Marshal.ReleaseComObject(keyCollection);
                    keyCollection = null;
                }
            }

            if (valuesCollection != null)
            {
                Marshal.ReleaseComObject(valuesCollection);
                valuesCollection = null;
            }


            return data;
        }
        #endregion

        private readonly Dictionary<Guid, IList<object?>> _sensorDataDictionary = new();

        #region IDictionary<Guid,IList<object>> Members

        /// <summary>
        /// Adds a data item to the dictionary.
        /// </summary>
        /// <param name="key">The data field identifier.</param>
        /// <param name="value">The data list.</param>
        public void Add(Guid key, IList<object?> value)
        {
            _sensorDataDictionary.Add(key, value);
        }
        /// <summary>
        /// Determines if a particular data field itentifer is present in the collection.
        /// </summary>
        /// <param name="key">The data field identifier.</param>
        /// <returns><b>true</b> if the data field identifier is present.</returns>
        public bool ContainsKey(Guid key)
        {
            return _sensorDataDictionary.ContainsKey(key);
        }
        /// <summary>
        /// Gets the list of the data field identifiers in this collection.
        /// </summary>
        public ICollection<Guid> Keys => _sensorDataDictionary.Keys;

        /// <summary>
        /// Removes a particular data field identifier from the collection.
        /// </summary>
        /// <param name="key">The data field identifier.</param>
        /// <returns><b>true</b> if the item was removed.</returns>
        public bool Remove(Guid key)
        {
            return _sensorDataDictionary.Remove(key);
        }
        /// <summary>
        /// Attempts to get the value of a data item.
        /// </summary>
        /// <param name="key">The data field identifier.</param>
        /// <param name="value">The data list.</param>
        /// <returns><b>true</b> if able to obtain the value; otherwise <b>false</b>.</returns>
        public bool TryGetValue(Guid key, out IList<object?> value) => _sensorDataDictionary.TryGetValue(key, out value);

        /// <summary>
        /// Gets the list of data values in the dictionary.
        /// </summary>
        public ICollection<IList<object?>> Values => _sensorDataDictionary.Values;

        /// <summary>
        /// Gets or sets the index operator for the dictionary by key.
        /// </summary>
        /// <param name="key">A GUID.</param>
        /// <returns>The item at the specified index.</returns>
        public IList<object?> this[Guid key]
        {
            get => _sensorDataDictionary[key];
            set => _sensorDataDictionary[key] = value;
        }

        #endregion

        #region ICollection<KeyValuePair<Guid,IList<object>>> Members
        /// <summary>
        /// Adds a specified key/value data pair to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(KeyValuePair<Guid, IList<object>> item)
        {
            ICollection<KeyValuePair<Guid, IList<object>>> c = _sensorDataDictionary as ICollection<KeyValuePair<Guid, IList<object>>>;
            c.Add(item);
        }

        /// <summary>
        /// Clears the items from the collection.
        /// </summary>
        public void Clear()
        {
            ICollection<KeyValuePair<Guid, IList<object>>> c = _sensorDataDictionary as ICollection<KeyValuePair<Guid, IList<object>>>;
            c.Clear();
        }

        /// <summary>
        /// Determines if the collection contains the specified key/value pair. 
        /// </summary>
        /// <param name="item">The item to locate.</param>
        /// <returns><b>true</b> if the collection contains the key/value pair; otherwise false.</returns>
        public bool Contains(KeyValuePair<Guid, IList<object>> item)
        {
            ICollection<KeyValuePair<Guid, IList<object>>> c = _sensorDataDictionary as ICollection<KeyValuePair<Guid, IList<object>>>;
            return c.Contains(item);
        }

        /// <summary>
        /// Copies an element collection to another collection.
        /// </summary>
        /// <param name="array">The destination collection.</param>
        /// <param name="arrayIndex">The index of the item to copy.</param>
        public void CopyTo(KeyValuePair<Guid, IList<object>>[] array, int arrayIndex)
        {
            ICollection<KeyValuePair<Guid, IList<object>>> c = _sensorDataDictionary as ICollection<KeyValuePair<Guid, IList<object>>>;
            c.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the number of items in the collection.
        /// </summary>
        public int Count => _sensorDataDictionary.Count;

        /// <summary>
        /// Gets a value that determines if the collection is read-only.
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Removes a particular item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><b>true</b> if successful; otherwise <b>false</b></returns>
        public bool Remove(KeyValuePair<Guid, IList<object>> item)
        {
            ICollection<KeyValuePair<Guid, IList<object>>> c = _sensorDataDictionary as ICollection<KeyValuePair<Guid, IList<object>>>;
            return c.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<Guid,IList<object>>> Members
        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<KeyValuePair<Guid, IList<object>>> GetEnumerator()
        {
            return (_sensorDataDictionary as IEnumerator<KeyValuePair<Guid, IList<object>>>);
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _sensorDataDictionary as System.Collections.IEnumerator;

        #endregion
    }
}
