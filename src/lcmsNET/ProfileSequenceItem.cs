﻿using lcmsNET.Impl;
using System;
using System.Runtime.InteropServices;

namespace lcmsNET
{
    public sealed class ProfileSequenceItem
    {
        internal ProfileSequenceItem(IntPtr ptr, ProfileSequenceDescriptor parent)
        {
            Ptr = ptr;
            Parent = parent;
        }

        public uint DeviceMfg
        {
            get { return GetDeviceMfg(); }
            set { SetDeviceMfg(value); }
        }

        public uint DeviceModel
        {
            get { return GetDeviceModel(); }
            set { SetDeviceModel(value); }
        }

        public DeviceAttributes Attributes
        {
            get { return GetAttributes(); }
            set { SetAttributes(value); }
        }

        public TechnologySignature Technology
        {
            get { return GetTechnology(); }
            set { SetTechnology(value); }
        }

        public byte[] ProfileID
        {
            get { return GetProfileID(); }
            set { SetProfileID(value); }
        }

        public MultiLocalizedUnicode Manufacturer
        {
            get
            {
                if (this.manufacturer is null)
                {
                    this.manufacturer = GetManufacturer();
                }
                return this.manufacturer;
            }
            set
            {
                SetManufacturer(value);
                value?.Release(); // object is now owned by the profile sequence descriptor
                this.manufacturer = null; // force re-load
            }
        }
        private MultiLocalizedUnicode manufacturer;

        public MultiLocalizedUnicode Model
        {
            get
            {
                if (this.model is null)
                {
                    this.model = GetModel();
                }
                return this.model;
            }
            set
            {
                SetModel(value);
                value?.Release(); // object is now owned by the profile sequence descriptor
                this.model = null; // force re-load
            }
        }
        private MultiLocalizedUnicode model;

        public MultiLocalizedUnicode Description
        {
            get
            {
                if (this.description is null)
                {
                    this.description = GetDescription();
                }
                return this.description;
            }
            set
            {
                SetDescription(value);
                value?.Release(); // object is now owned by the profile sequence descriptor
                this.description = null; // force re-load
            }
        }
        private MultiLocalizedUnicode description;

        private unsafe PSeqDesc* SeqDesc
        {
            get
            {
                EnsureNotDisposed();
                PSeqDesc* pSeqDesc = (PSeqDesc*)Ptr.ToPointer();
                return pSeqDesc;
            }
        }

        private unsafe uint GetDeviceMfg() => SeqDesc->deviceMfg;
        private unsafe void SetDeviceMfg(uint value) => SeqDesc->deviceMfg = value;
        private unsafe uint GetDeviceModel() => SeqDesc->deviceModel;
        private unsafe void SetDeviceModel(uint value) => SeqDesc->deviceModel = value;
        private unsafe DeviceAttributes GetAttributes() => (DeviceAttributes)SeqDesc->attributes;
        private unsafe void SetAttributes(DeviceAttributes value) => SeqDesc->attributes = Convert.ToUInt64(value);
        private unsafe TechnologySignature GetTechnology() => SeqDesc->technology;
        private unsafe void SetTechnology(TechnologySignature value) => SeqDesc->technology = value;
        private unsafe byte[] GetProfileID()
        {
            byte[] profileID = new byte[16];
            IntPtr ptr = new IntPtr(SeqDesc->ProfileID);
            Marshal.Copy(ptr, profileID, 0, 16);
            return profileID;
        }
        private unsafe void SetProfileID(byte[] value)
        {
            if (value?.Length != 16) throw new ArgumentException($"'{nameof(value)}' array size must equal 16.");
            IntPtr ptr = new IntPtr(SeqDesc->ProfileID);
            Marshal.Copy(value, 0, ptr, 16);
        }
        private unsafe MultiLocalizedUnicode GetManufacturer()
        {
            IntPtr ptr = SeqDesc->Manufacturer;
            return (ptr != IntPtr.Zero) ? MultiLocalizedUnicode.CopyRef(ptr) : null;
        }
        private unsafe void SetManufacturer(MultiLocalizedUnicode value)
        {
            SeqDesc->Manufacturer = value?.Handle ?? IntPtr.Zero;
        }
        private unsafe MultiLocalizedUnicode GetModel()
        {
            IntPtr ptr = SeqDesc->Model;
            return (ptr != IntPtr.Zero) ? MultiLocalizedUnicode.CopyRef(ptr) : null;
        }
        private unsafe void SetModel(MultiLocalizedUnicode value)
        {
            SeqDesc->Model = value?.Handle ?? IntPtr.Zero;
        }
        private unsafe MultiLocalizedUnicode GetDescription()
        {
            IntPtr ptr = SeqDesc->Description;
            return (ptr != IntPtr.Zero) ? MultiLocalizedUnicode.CopyRef(ptr) : null;
        }
        private unsafe void SetDescription(MultiLocalizedUnicode value)
        {
            SeqDesc->Description = value?.Handle ?? IntPtr.Zero;
        }

        private void EnsureNotDisposed()
        {
            if (Parent.IsDisposed)
            {
                throw new ObjectDisposedException(nameof(ProfileSequenceItem));
            }
        }

        private ProfileSequenceDescriptor Parent { get; set; }

        private IntPtr Ptr { get; set; }
    }
}
