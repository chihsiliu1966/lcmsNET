﻿using lcmsNET.Impl;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace lcmsNET
{
    public sealed class IT8 : IDisposable
    {
        private IntPtr _handle;

        internal IT8(IntPtr handle, Context context = null)
        {
            Helper.CheckCreated<IT8>(handle);

            _handle = handle;
            Context = context;
        }

        public static IT8 Create(Context context)
        {
            return new IT8(Interop.IT8Alloc(context?.Handle ?? IntPtr.Zero), context);
        }

        #region Properties
        public Context Context { get; private set; }
        #endregion

        #region Tables
        public uint TableCount => Interop.IT8TableCount(_handle);

        public int SetTable(uint nTable)
        {
            EnsureNotDisposed();

            return Interop.IT8SetTable(_handle, nTable);
        }
        #endregion

        #region Persistence
        public static IT8 Open(Context context, string filepath)
        {
            return new IT8(Interop.IT8LoadFromFile(context?.Handle ?? IntPtr.Zero, filepath), context);
        }

        public static IT8 Open(Context context, byte[] memory)
        {
            return new IT8(Interop.IT8LoadFromMem(context?.Handle ?? IntPtr.Zero, memory), context);
        }

        public bool Save(string filepath)
        {
            EnsureNotDisposed();

            return 0 != Interop.IT8SaveToFile(_handle, filepath);
        }

        public bool Save(byte[] it8, out int bytesNeeded)
        {
            EnsureNotDisposed();

            return 0 != Interop.IT8SaveToMem(_handle, it8, out bytesNeeded);
        }
        #endregion

        #region Type and Comments
        public string SheetType
        {
            get { return Interop.IT8GetSheetType(_handle); }
            set
            {
                EnsureNotDisposed();
                if (0 == Interop.IT8SetSheetType(_handle, value))
                {
                    throw new LcmsNETException($"Failed to set sheet type: '{value}'.");
                }
            }
        }

        public bool AddComment(string comment)
        {
            EnsureNotDisposed();
            return Interop.IT8SetComment(_handle, comment) != 0;
        }
        #endregion

        #region Properties
        public string GetProperty(string name)
        {
            return Interop.IT8GetProperty(_handle, name);
        }

        public double GetDoubleProperty(string name)
        {
            return Interop.IT8GetPropertyDouble(_handle, name);
        }

        public bool SetProperty(string name, string value)
        {
            return Interop.IT8SetProperty(_handle, name, value) != 0;
        }

        public bool SetProperty(string name, double value)
        {
            return Interop.IT8SetPropertyDouble(_handle, name, value) != 0;
        }

        public bool SetProperty(string name, uint hex)
        {
            return Interop.IT8SetPropertyHex(_handle, name, hex) != 0;
        }

        public bool SetUncookedProperty(string name, string value)
        {
            return Interop.IT8SetPropertyUncooked(_handle, name, value) != 0;
        }

        public bool SetProperty(string key, string subkey, string value)
        {
            return Interop.IT8SetProperty(_handle, key, subkey, value) != 0;
        }

        public IEnumerable<string> Properties => Interop.IT8EnumProperties(_handle);

        public IEnumerable<string> GetProperties(string name)
        {
            EnsureNotDisposed();

            return Interop.IT8EnumPropertyMulti(_handle, name);
        }
        #endregion

        #region Datasets
        public string GetData(int row, int column)
        {
            return Interop.IT8GetDataRowCol(_handle, row, column);
        }

        public string GetData(string patch, string sample)
        {
            return Interop.IT8GetData(_handle, patch, sample);
        }

        public double GetDoubleData(int row, int column)
        {
            return Interop.IT8GetDataRowColDouble(_handle, row, column);
        }

        public double GetDoubleData(string patch, string sample)
        {
            return Interop.IT8GetDataDbl(_handle, patch, sample);
        }

        public bool SetData(int row, int column, string value)
        {
            return Interop.IT8SetDataRowCol(_handle, row, column, value) != 0;
        }

        public bool SetData(int row, int column, double value)
        {
            return Interop.IT8SetDataRowColDbl(_handle, row, column, value) != 0;
        }

        public int FindDataFormat(string sample)
        {
            return Interop.IT8FindDataFormat(_handle, sample);
        }

        public bool SetDataFormat(int column, string sample)
        {
            return Interop.IT8SetDataFormat(_handle, column, sample) != 0;
        }

        public IEnumerable<string> SampleNames => Interop.IT8EnumDataFormat(_handle);

        public string GetPatchName(int nPatch)
        {
            return Interop.IT8GetPatchName(_handle, nPatch);
        }

        public string DoubleFormat
        {
            set => Interop.IT8DefineDblFormat(_handle, value);
        }
        #endregion

        #region IDisposable Support
        public bool IsDisposed => _handle == IntPtr.Zero;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureNotDisposed()
        {
            if (_handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException(nameof(IT8));
            }
        }

        private void Dispose(bool disposing)
        {
            var handle = Interlocked.Exchange(ref _handle, IntPtr.Zero);
            if (handle != IntPtr.Zero)
            {
                Interop.IT8Free(handle);
                Context = null;
            }
        }

        ~IT8()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        internal IntPtr Handle => _handle;
    }
}
