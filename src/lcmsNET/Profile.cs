﻿using lcmsNET.Impl;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace lcmsNET
{
    public sealed class Profile : IDisposable
    {
        private IntPtr _handle;

        internal Profile(IntPtr handle, Context context = null, IOHandler iohandler = null)
        {
            Helper.CheckCreated<Profile>(handle);

            _handle = handle;
            Context = context;
            IOHandler = iohandler;
        }

        #region Predefined virtual profiles
        public static Profile CreatePlaceholder(Context context)
        {
            return new Profile(Interop.CreatePlaceholder(context?.Handle ?? IntPtr.Zero), context);
        }

        public static Profile CreateRGB(in CIExyY whitePoint, in CIExyYTRIPLE primaries, ToneCurve[] transferFunction)
        {
            if (transferFunction?.Length != 3) throw new ArgumentException($"'{nameof(transferFunction)}' array size must equal 3.");

            return new Profile(Interop.CreateRGB(whitePoint, primaries, transferFunction.Select(_ => _.Handle).ToArray()));
        }

        public static Profile CreateRGB(Context context, in CIExyY whitePoint, in CIExyYTRIPLE primaries, ToneCurve[] transferFunction)
        {
            if (transferFunction?.Length != 3) throw new ArgumentException($"'{nameof(transferFunction)}' array size must equal 3.");

            return new Profile(Interop.CreateRGB(context?.Handle ?? IntPtr.Zero, whitePoint, primaries, transferFunction.Select(_ => _.Handle).ToArray()), context);
        }

        public static Profile CreateGray(in CIExyY whitePoint, ToneCurve transferFunction)
        {
            return new Profile(Interop.CreateGray(whitePoint, transferFunction.Handle));
        }

        public static Profile CreateGray(Context context, in CIExyY whitePoint, ToneCurve transferFunction)
        {
            return new Profile(Interop.CreateGray(context?.Handle ?? IntPtr.Zero, whitePoint, transferFunction.Handle), context);
        }

        public static Profile CreateLinearizationDeviceLink(ColorSpaceSignature space, ToneCurve[] transferFunction)
        {
            return new Profile(Interop.CreateLinearizationDeviceLink(Convert.ToUInt32(space), transferFunction.Select(_ => _.Handle).ToArray()));
        }

        public static Profile CreateLinearizationDeviceLink(Context context, ColorSpaceSignature space, ToneCurve[] transferFunction)
        {
            return new Profile(Interop.CreateLinearizationDeviceLink(context?.Handle ?? IntPtr.Zero, Convert.ToUInt32(space),
                    transferFunction.Select(_ => _.Handle).ToArray()), context);
        }

        public static Profile CreateInkLimitingDeviceLink(ColorSpaceSignature space, double limit)
        {
            return new Profile(Interop.CreateInkLimitingDeviceLink(Convert.ToUInt32(space), limit));
        }

        public static Profile CreateInkLimitingDeviceLink(Context context, ColorSpaceSignature space, double limit)
        {
            return new Profile(Interop.CreateInkLimitingDeviceLink(context?.Handle ?? IntPtr.Zero, Convert.ToUInt32(space), limit), context);
        }

        public static Profile CreateDeviceLink(Transform transform, double version, CmsFlags flags)
        {
            return new Profile(Interop.Transform2DeviceLink(transform.Handle, version, Convert.ToUInt32(flags)));
        }

        public static Profile CreateLab2(in CIExyY whitePoint)
        {
            return new Profile(Interop.CreateLab2(whitePoint));
        }

        public static Profile CreateLab2(Context context, in CIExyY whitePoint)
        {
            return new Profile(Interop.CreateLab2(context?.Handle ?? IntPtr.Zero, whitePoint), context);
        }

        public static Profile CreateLab4(in CIExyY whitePoint)
        {
            return new Profile(Interop.CreateLab4(whitePoint));
        }

        public static Profile CreateLab4(Context context, in CIExyY whitePoint)
        {
            return new Profile(Interop.CreateLab4(context?.Handle ?? IntPtr.Zero, whitePoint), context);
        }

        public static Profile CreateXYZ()
        {
            return new Profile(Interop.CreateXYZ());
        }

        public static Profile CreateXYZ(Context context)
        {
            return new Profile(Interop.CreateXYZ(context?.Handle ?? IntPtr.Zero), context);
        }

        public static Profile Create_sRGB()
        {
            return new Profile(Interop.Create_sRGB());
        }

        public static Profile Create_sRGB(Context context)
        {
            return new Profile(Interop.Create_sRGB(context?.Handle ?? IntPtr.Zero), context);
        }

        public static Profile CreateNull()
        {
            return new Profile(Interop.CreateNull());
        }

        public static Profile CreateNull(Context context)
        {
            return new Profile(Interop.CreateNull(context?.Handle ?? IntPtr.Zero), context);
        }

        public static Profile CreateBCHSWabstract(int nLutPoints, double bright, double contrast,
                double hue, double saturation, int tempSrc, int tempDest)
        {
            return new Profile(Interop.CreateBCHSWabstract(nLutPoints, bright, contrast, hue, saturation, tempSrc, tempDest));
        }

        public static Profile CreateBCHSWabstract(Context context, int nLutPoints, double bright, double contrast,
                double hue, double saturation, int tempSrc, int tempDest)
        {
            return new Profile(Interop.CreateBCHSWabstract(context?.Handle ?? IntPtr.Zero, nLutPoints, bright, contrast, hue, saturation, tempSrc, tempDest), context);
        }
        #endregion

        #region Access functions
        public static Profile Open(string filepath, string access)
        {
            return new Profile(Interop.OpenProfile(filepath, access));
        }

        public static Profile Open(Context context, string filepath, string access)
        {
            return new Profile(Interop.OpenProfile(context?.Handle ?? IntPtr.Zero, filepath, access), context);
        }

        public static Profile Open(byte[] memory)
        {
            return new Profile(Interop.OpenProfile(memory));
        }

        public static Profile Open(Context context, byte[] memory)
        {
            return new Profile(Interop.OpenProfile(context?.Handle ?? IntPtr.Zero, memory), context);
        }

        public static Profile Open(Context context, IOHandler iohandler)
        {
            return new Profile(Interop.OpenProfile(context?.Handle ?? IntPtr.Zero, iohandler?.Handle ?? IntPtr.Zero), context, iohandler);
        }

        public static Profile Open(Context context, IOHandler iohandler, bool writeable)
        {
            return new Profile(Interop.OpenProfile(context?.Handle ?? IntPtr.Zero, iohandler?.Handle ?? IntPtr.Zero, writeable ? 1 : 0), context, iohandler);
        }

        public bool Save(string filepath)
        {
            EnsureNotDisposed();

            return 0 != Interop.SaveProfile(_handle, filepath);
        }

        public bool Save(byte[] profile, out int bytesNeeded)
        {
            EnsureNotDisposed();

            return 0 != Interop.SaveProfile(_handle, profile, out bytesNeeded);
        }

        public int Save(IOHandler iohandler)
        {
            EnsureNotDisposed();

            return Interop.SaveProfile(_handle, iohandler?.Handle ?? IntPtr.Zero);
        }
        #endregion

        #region Obtain localized information
        public string GetProfileInfo(InfoType info, string languageCode, string countryCode)
        {
            EnsureNotDisposed();

            return Interop.GetProfileInfo(_handle, Convert.ToUInt32(info), languageCode, countryCode);
        }

        public string GetProfileInfoASCII(InfoType info, string languageCode, string countryCode)
        {
            EnsureNotDisposed();

            return Interop.GetProfileInfoASCII(_handle, Convert.ToUInt32(info), languageCode, countryCode);
        }
        #endregion

        #region Feature detection
        public bool DetectBlackPoint(out CIEXYZ blackPoint, Intent intent, CmsFlags flags = CmsFlags.None)
        {
            return Interop.DetectBlackPoint(_handle, out blackPoint, Convert.ToUInt32(intent), Convert.ToUInt32(flags)) != 0;
        }

        public bool DetectDestinationBlackPoint(out CIEXYZ blackPoint, Intent intent, CmsFlags flags = CmsFlags.None)
        {
            return Interop.DetectDestinationBlackPoint(_handle, out blackPoint, Convert.ToUInt32(intent), Convert.ToUInt32(flags)) != 0;
        }
        #endregion

        #region Access profile header
        public bool GetHeaderCreationDateTime(out DateTime dest)
        {
            EnsureNotDisposed();

            return Interop.GetHeaderCreationDateTime(_handle, out dest) != 0;
        }
        #endregion

        #region Info on profile implementation
        public bool IsCLUT(Intent intent, UsedDirection direction)
        {
            EnsureNotDisposed();

            return Interop.IsCLUT(_handle, Convert.ToUInt32(intent), Convert.ToUInt32(direction)) != 0;
        }
        #endregion

        #region Access tags
        public TagSignature GetTag(uint n)
        {
            EnsureNotDisposed();

            return (TagSignature)Interop.GetTagSignature(_handle, n);
        }

        public bool HasTag(TagSignature tag)
        {
            EnsureNotDisposed();

            return Interop.IsTag(_handle, Convert.ToUInt32(tag)) != 0;
        }

        public IntPtr ReadTag(TagSignature tag)
        {
            EnsureNotDisposed();

            return Interop.ReadTag(_handle, Convert.ToUInt32(tag));
        }

        public T ReadTag<T>(TagSignature tag)
        {
            EnsureNotDisposed();

            IntPtr ptr = ReadTag(tag);
            if (ptr == IntPtr.Zero) return default;

            Type t = typeof(T);
            MethodInfo method = t.GetMethod("FromHandle", BindingFlags.Public | BindingFlags.Static,
                    null, new Type[] { typeof(IntPtr) }, null);
            if (method is null) throw new MissingMethodException(nameof(T), "FromHandle(IntPtr)");

            return (T)method.Invoke(null, new object[] { ptr });
        }

        public bool WriteTag(TagSignature tag, IWrapper wrapper)
        {
            return WriteTag(tag, wrapper.Handle);
        }

        public bool WriteTag(TagSignature tag, IntPtr data)
        {
            EnsureNotDisposed();

            return Interop.WriteTag(_handle, Convert.ToUInt32(tag), data) != 0;
        }

        public bool WriteTag<T>(TagSignature tag, in T data)
            where T: struct
        {
            EnsureNotDisposed();

            int size = Marshal.SizeOf<T>();
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, false);
            try
            {
                return WriteTag(tag, ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool LinkTag(TagSignature tag, TagSignature dest)
        {
            EnsureNotDisposed();

            return Interop.LinkTag(_handle, Convert.ToUInt32(tag), Convert.ToUInt32(dest)) != 0;
        }

        public TagSignature TagLinkedTo(TagSignature tag)
        {
            EnsureNotDisposed();

            return (TagSignature)Interop.TagLinkedTo(_handle, Convert.ToUInt32(tag));
        }
        #endregion

        #region Intents
        public bool IsIntentSupported(Intent intent, UsedDirection usedDirection)
        {
            EnsureNotDisposed();

            return Interop.IsIntentSupported(_handle, Convert.ToUInt32(intent), Convert.ToUInt32(usedDirection)) != 0;
        }
        #endregion

        #region MD5 message digest
        public bool ComputeMD5()
        {
            EnsureNotDisposed();

            return Interop.MD5ComputeID(_handle) != 0;
        }
        #endregion

        #region PostScript generation
        public uint GetPostScriptColorResource(Context context, PostScriptResourceType type, Intent intent, CmsFlags flags, IOHandler handler)
        {
            EnsureNotDisposed();

            return Interop.GetPostScriptColorResource(_handle, context?.ID ?? IntPtr.Zero, Convert.ToUInt32(type),
                    Convert.ToUInt32(intent), Convert.ToUInt32(flags), handler?.Handle ?? IntPtr.Zero);
        }

        public byte[] GetPostScriptColorSpaceArray(Context context, Intent intent, CmsFlags flags)
        {
            EnsureNotDisposed();

            return Interop.GetPostScriptCSA(_handle, context?.ID ?? IntPtr.Zero, Convert.ToUInt32(intent), Convert.ToUInt32(flags));
        }

        public byte[] GetPostScriptColorRenderingDictionary(Context context, Intent intent, CmsFlags flags)
        {
            EnsureNotDisposed();

            return Interop.GetPostScriptCRD(_handle, context?.ID ?? IntPtr.Zero, Convert.ToUInt32(intent), Convert.ToUInt32(flags));
        }
        #endregion

        #region Properties
        public Context Context { get; private set; }

        public ColorSpaceSignature ColorSpace
        {
            get { return (ColorSpaceSignature)Interop.GetColorSpace(_handle); }
            set { Interop.SetColorSpace(_handle, Convert.ToUInt32(value)); }
        }

        public ColorSpaceSignature PCS
        {
            get { return (ColorSpaceSignature)Interop.GetPCS(_handle); }
            set { Interop.SetPCS(_handle, Convert.ToUInt32(value)); }
        }

        public double TotalAreaCoverage => Interop.DetectTAC(_handle);

        public ProfileClassSignature DeviceClass
        {
            get { return (ProfileClassSignature)Interop.GetDeviceClass(_handle); }
            set { Interop.SetDeviceClass(_handle, Convert.ToUInt32(value)); }
        }

        public uint HeaderFlags
        {
            get { return Interop.GetHeaderFlags(_handle); }
            set { Interop.SetHeaderFlags(_handle, value); }
        }

        public uint HeaderManufacturer
        {
            get { return Interop.GetHeaderManufacturer(_handle); }
            set { Interop.SetHeaderManufacturer(_handle, value); }
        }

        public uint HeaderModel
        {
            get { return Interop.GetHeaderModel(_handle); }
            set { Interop.SetHeaderModel(_handle, value); }
        }

        public DeviceAttributes HeaderAttributes
        {
            get { return (DeviceAttributes)Interop.GetHeaderAttributes(_handle); }
            set { Interop.SetHeaderAttributes(_handle, (ulong)value); }
        }

        public double Version
        {
            get { return Interop.GetProfileVersion(_handle); }
            set { Interop.SetProfileVersion(_handle, value); }
        }

        public uint EncodedICCVersion
        {
            get { return Interop.GetEncodedICCVersion(_handle); }
            set { Interop.SetEncodedICCVersion(_handle, value); }
        }

        public bool IsMatrixShaper => Interop.IsMatrixShaper(_handle) != 0;

        public int TagCount => Interop.GetTagCount(_handle);

        public Intent HeaderRenderingIntent
        {
            get { return (Intent)Interop.GetHeaderRenderingIntent(_handle); }
            set { Interop.SetHeaderRenderingIntent(_handle, Convert.ToUInt32(value)); }
        }

        public byte[] HeaderProfileID
        {
            get
            {
                byte[] profileID = new byte[16];
                Interop.GetHeaderProfileID(_handle, profileID);
                return profileID;
            }
            set
            {
                if (value?.Length != 16) throw new ArgumentException($"'{nameof(value)}' array size must equal 16.");
                Interop.SetHeaderProfileID(_handle, value);
            }
        }

        public IOHandler IOHandler
        {
            get
            {
                EnsureNotDisposed();

                if (_iohandler is null)
                {
                    _iohandler = new IOHandler(Interop.GetProfileIOHandler(_handle), Context, isOwner: false);
                }
                return _iohandler;
            }
            private set
            {
                _iohandler = value;
                if (!(_iohandler is null)) _iohandler.IsOwner = false; // take ownership to avoid double free()
            }
        }
        private IOHandler _iohandler;
        #endregion

        #region IDisposable Support
        public bool IsDisposed => _handle == IntPtr.Zero;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureNotDisposed()
        {
            if (_handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException(nameof(Profile));
            }
        }

        private void Dispose(bool disposing)
        {
            var handle = Interlocked.Exchange(ref _handle, IntPtr.Zero);
            if (handle != IntPtr.Zero)
            {
                Interop.CloseProfile(handle);
                IOHandler = null; // profile closure also closes i/o handler
                Context = null;
            }
        }

        ~Profile()
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
