﻿// Copyright(c) 2019-2021 John Stevenson-Hoare
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lcmsNET.Tests
{
    [TestClass()]
    public class ColorimetricTest
    {
        [TestMethod()]
        public void XYZ2Lab_WhenInvoked_ShouldConvertXYZToLab()
        {
            // Arrange
            CIEXYZ whitePoint = new() { X = 0.9642, Y = 1.0, Z = 0.8249 }; // D50 XYZ normalized to Y = 1.0
            CIEXYZ xyz = new() { X = 0.9642, Y = 1.0, Z = 0.8249 };

            // Act
            CIELab lab = Colorimetric.XYZ2Lab(whitePoint, xyz);

            // Assert
            Assert.AreEqual(100.0, lab.L);
            Assert.AreEqual(0.0, lab.a);
            Assert.AreEqual(0.0, lab.b);
        }

        [TestMethod()]
        public void Lab2XYZ_WhenInvoked_ShouldConvertLabToXYZ()
        {
            // Arrange
            CIEXYZ whitePoint = new() { X = 0.9642, Y = 1.0, Z = 0.8249 }; // D50 XYZ normalized to Y = 1.0
            CIELab lab = new() { L = 100.0, a = 0.0, b = 0.0 };

            // Act
            CIEXYZ xyz = Colorimetric.Lab2XYZ(whitePoint, lab);

            // Assert
            Assert.AreEqual(0.9642, xyz.X);
            Assert.AreEqual(1.0, xyz.Y);
            Assert.AreEqual(0.8249, xyz.Z);
        }

        [TestMethod()]
        public void Lab2LCh_WhenInvoked_ShouldConvertLabToLCh()
        {
            // Arrange
            CIELab lab = new() { L = 100.0, a = 0.0, b = 0.0 };

            // Act
            CIELCh lch = Colorimetric.Lab2LCh(lab);

            // Assert
            Assert.AreEqual(100.0, lch.L);
            Assert.AreEqual(0.0, lch.C);
            Assert.AreEqual(0.0, lch.h);
        }

        [TestMethod()]
        public void LCh2Lab_WhenInvoked_ShouldConvertLChToLab()
        {
            // Arrange
            CIELCh lch = new() { L = 100.0, C = 0.0, h = 0.0 };

            // Act
            CIELab lab = Colorimetric.LCh2Lab(lch);

            // Assert
            Assert.AreEqual(100.0, lab.L);
            Assert.AreEqual(0.0, lab.a);
            Assert.AreEqual(0.0, lab.b);
        }

        [TestMethod()]
        public void LabV4Encoding_WhenRoundTripped_ShouldHaveValueSet()
        {
            // Arrange
            ushort[] inWLab = new ushort[3];
            for (ushort u = 0; u < ushort.MaxValue; u++)
            {
                inWLab[0] = inWLab[1] = inWLab[2] = u;

                // Act
                CIELab lab = Colorimetric.LabEncoded2Float(inWLab);
                ushort[] outWLab = Colorimetric.Float2LabEncoded(lab);

                // Assert
                CollectionAssert.AreEqual(inWLab, outWLab);
            }
        }

        [TestMethod()]
        public void LabV2Encoding_WhenRoundTripped_ShouldHaveValueSet()
        {
            // Arrange
            ushort[] inWLab = new ushort[3];
            for (ushort u = 0; u < ushort.MaxValue; u++)
            {
                inWLab[0] = inWLab[1] = inWLab[2] = u;

                // Act
                CIELab lab = Colorimetric.LabEncoded2FloatV2(inWLab);
                ushort[] outWLab = Colorimetric.Float2LabEncodedV2(lab);

                // Assert
                CollectionAssert.AreEqual(inWLab, outWLab);
            }
        }

        [TestMethod()]
        public void XYZEncoding_WhenRoundTripped_ShouldHaveValueSet()
        {
            // Arrange
            ushort[] inXyz = new ushort[3];
            for (ushort u = 0; u < ushort.MaxValue; u++)
            {
                inXyz[0] = inXyz[1] = inXyz[2] = u;

                // Act
                CIEXYZ fxyz = Colorimetric.XYZEncoded2Float(inXyz);
                ushort[] outXyz = Colorimetric.Float2XYZEncoded(fxyz);

                // Assert
                CollectionAssert.AreEqual(inXyz, outXyz);
            }
        }

        [TestMethod()]
        public void D50_XYZ_WhenGetting_ShouldHaveCorrectValue()
        {
            // Act
            var d50 = Colorimetric.D50_XYZ;

            // Assert
            Assert.AreEqual(0.9642, d50.X, double.Epsilon);
            Assert.AreEqual(1.0, d50.Y, double.Epsilon);
            Assert.AreEqual(0.8249, d50.Z, double.Epsilon);
        }

        [TestMethod()]
        public void D50_xyY_WhenGetting_ShouldHaveCorrectValue()
        {
            // Act
            var d50 = Colorimetric.D50_xyY;

            // Assert
            Assert.AreEqual(0.3457, d50.x, 0.0001);
            Assert.AreEqual(0.3585, d50.y, 0.0001);
            Assert.AreEqual(1.0, d50.Y, double.Epsilon);
        }

        [TestMethod()]
        public void Desaturate_WhenInvoked_ShouldSucceed()
        {
            // Arrange
            CIELab lab = new() { L = 97.4, a = 62.3, b = 81.2 };
            double aMax = 55, aMin = -55, bMax = 75, bMin = -75;

            // Act
            bool desaturated = Colorimetric.Desaturate(ref lab, aMax, aMin, bMax, bMin);

            // Assert
            Assert.IsTrue(desaturated);
        }
    }
}
