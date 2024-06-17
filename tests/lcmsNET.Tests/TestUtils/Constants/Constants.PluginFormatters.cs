﻿// Copyright(c) 2019-2024 John Stevenson-Hoare
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using lcmsNET.Plugin;
using System;

namespace lcmsNET.Tests.TestUtils
{
    public static partial class Constants
    {
        public static class PluginFormatters
        {
            public static readonly PluginBase Base = new()
            {
                Magic = Cms.PluginMagicNumber,
                ExpectedVersion = (uint)2060,
                Type = PluginType.Formatters,
                Next = IntPtr.Zero
            };

            public static readonly uint TYPE_RGB_565 = COLORSPACE_SH(PixelType.RGB) | CHANNELS_SH(3) | BYTES_SH(0) | (1 << 23);

            private static uint COLORSPACE_SH(PixelType s) { return Convert.ToUInt32(s) << 16; }
            private static uint CHANNELS_SH(uint s) { return s << 3; }
            private static uint BYTES_SH(uint s) { return s; }
        }
    }
}