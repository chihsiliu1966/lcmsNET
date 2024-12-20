﻿// Copyright(c) 2019-2022 John Stevenson-Hoare
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace lcmsNET.Tests.Plugin
{
    [TestClass()]
    public class VEC3Test
    {
        [TestMethod]
        public void VEC3_WhenInstantiated_ShouldHaveInitialisedElements()
        {
            // Arrange
            double expected_x = 1.0;
            double expected_y = 2.0;
            double expected_z = 3.0;

            // Act
            VEC3 v = new(expected_x, expected_y, expected_z);
            double actual_x = v[0];
            double actual_y = v[1];
            double actual_z = v[2];

            // Assert
            Assert.AreEqual(expected_x, actual_x);
            Assert.AreEqual(expected_y, actual_y);
            Assert.AreEqual(expected_z, actual_z);
        }

        [TestMethod]
        public void Minus_WhenInvoked_ShouldSubtractValues()
        {
            // Arrange
            double ax = 10.0, ay = 9.0, az = 8.0;
            VEC3 a = new(ax, ay, az);
            double bx = 9.0, by = 7.0, bz = 5.0;
            VEC3 b = new(bx, by, bz);
            double expected_x = ax - bx,
                   expected_y = ay - by,
                   expected_z = az - bz;

            // Act
            VEC3 actual = a - b;
            double actual_x = actual[0];
            double actual_y = actual[1];
            double actual_z = actual[2];

            // Assert
            Assert.AreEqual(expected_x, actual_x);
            Assert.AreEqual(expected_y, actual_y);
            Assert.AreEqual(expected_z, actual_z);
        }

        [TestMethod]
        public void Cross_WhenInvoked_ShouldCalculateCrossVectorProduct()
        {
            // Arrange
            double ax = 2.0, ay = 3.0, az = 4.0;
            VEC3 a = new(ax, ay, az);
            double bx = 5.0, by = 6.0, bz = 7.0;
            VEC3 b = new(bx, by, bz);
            double expected_x = (ay * bz) - (az * by),
                   expected_y = (az * bx) - (ax * bz),
                   expected_z = (ax * by) - (ay * bx);

            // Act
            VEC3 actual = VEC3.Cross(a, b);
            double actual_x = actual[0];
            double actual_y = actual[1];
            double actual_z = actual[2];

            // Assert
            Assert.AreEqual(expected_x, actual_x);
            Assert.AreEqual(expected_y, actual_y);
            Assert.AreEqual(expected_z, actual_z);
        }

        [TestMethod]
        public void Dot_WhenInvoked_ShouldCalculateDotScalarProduct()
        {
            // Arrange
            double ax = 2.0, ay = 3.0, az = 4.0;
            VEC3 a = new(ax, ay, az);
            double bx = 5.0, by = 6.0, bz = 7.0;
            VEC3 b = new(bx, by, bz);
            double expected = (ax * bx) + (ay * by) + (az * bz);

            // Act
            double actual = VEC3.Dot(a, b);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Length_WhenInvoked_ShouldReturnEuclideanLength()
        {
            // Arrange
            double x = 3.0, y = 4.0, z = 5.0;
            VEC3 v = new (x, y, z);
            double expected = Math.Sqrt((x * x) + (y * y) + (z * z));

            // Act
            double actual = v.Length;

            // Assert
            Assert.AreEqual(expected, actual, double.Epsilon);
        }

        [TestMethod]
        public void Distance_WhenInvoked_ShouldCalculateEuclideanDistanceBetweenPoints()
        {
            // Arrange
            double ax = 2.0, ay = 3.0, az = 4.0;
            VEC3 a = new(ax, ay, az);
            double bx = 5.0, by = 6.0, bz = 7.0;
            VEC3 b = new(bx, by, bz);
            VEC3 d = a - b;
            double expected = Math.Sqrt((d[0] * d[0]) + (d[1] * d[1]) + (d[2] * d[2]));

            // Act
            double actual = VEC3.Distance(a, b);

            // Assert
            Assert.AreEqual(expected, actual, double.Epsilon);
        }
    }
}
