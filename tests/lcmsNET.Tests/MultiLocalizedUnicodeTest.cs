﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace lcmsNET.Tests
{
    [TestClass()]
    public class MultiLocalizedUnicodeTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private MemoryStream Save(string resourceName)
        {
            MemoryStream ms = new MemoryStream();
            var thisExe = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(thisExe.FullName);
            using (var s = thisExe.GetManifestResourceStream(assemblyName.Name + resourceName))
            {
                s.CopyTo(ms);
            }
            return ms;
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 3;

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                // Assert
                Assert.IsNotNull(mlu);
            }
        }

        [TestMethod()]
        public void DuplicateTest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                mlu.SetASCII(MultiLocalizedUnicode.NoLanguage, MultiLocalizedUnicode.NoCountry, "Duplicate");

                using (var duplicate = mlu.Duplicate())
                {
                    // Assert
                    Assert.IsNotNull(duplicate);
                }
            }
        }

        [TestMethod()]
        public void SetASCIITest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;
            string languageCode = "en";
            string countryCode = "US";

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                bool set = mlu.SetASCII(languageCode, countryCode, "SetASCII");

                // Assert
                Assert.IsTrue(set);
            }
        }

        [TestMethod()]
        public void SetWideTest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;
            string languageCode = "en";
            string countryCode = "US";

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                bool set = mlu.SetWide(languageCode, countryCode, "SetWide");

                // Assert
                Assert.IsTrue(set);
            }
        }

        [TestMethod()]
        public void GetASCIITest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;
            string languageCode = "en";
            string countryCode = "US";
            string expected = "GetASCII";

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                mlu.SetASCII(languageCode, countryCode, expected);
                var actual = mlu.GetASCII(languageCode, countryCode);

                // Assert
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void GetWideTest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;
            string languageCode = "en";
            string countryCode = "US";
            string expected = "GetWide";

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                mlu.SetWide(languageCode, countryCode, expected);
                var actual = mlu.GetWide(languageCode, countryCode);

                // Assert
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void GetTranslationTest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;
            string expectedLanguageCode = "en";
            string expectedCountryCode = "US";

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                mlu.SetASCII(expectedLanguageCode, expectedCountryCode, "GetTranslation");
                var actual = mlu.GetTranslation(expectedLanguageCode, expectedCountryCode,
                        out string actualLanguageCode, out string actualCountryCode);

                // Assert
                Assert.AreEqual(expectedLanguageCode, actualLanguageCode);
                Assert.AreEqual(expectedCountryCode, actualCountryCode);
            }
        }

        [TestMethod()]
        public void TranslationsCountTest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;
            string languageCode = "en";
            string countryCode = "US";
            uint notExpected = 0;

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                mlu.SetASCII(languageCode, countryCode, "TranslationsCount");
                var actual = mlu.TranslationsCount;

                // Assert
                Assert.AreNotEqual(notExpected, actual);
            }
        }

        [TestMethod()]
        public void TranslationsCodeTest()
        {
            // Arrange
            IntPtr plugin = IntPtr.Zero;
            IntPtr userData = IntPtr.Zero;
            uint nItems = 0;
            string expectedLanguageCode = "en";
            string expectedCountryCode = "US";
            uint index = 0;

            // Act
            using (var context = Context.Create(plugin, userData))
            using (var mlu = MultiLocalizedUnicode.Create(context, nItems))
            {
                mlu.SetASCII(expectedLanguageCode, expectedCountryCode, "TranslationsCount");
                var actual = mlu.TranslationsCodes(index, out string actualLanguageCode, out string actualCountryCode);

                // Assert
                Assert.AreEqual(expectedLanguageCode, actualLanguageCode);
                Assert.AreEqual(expectedCountryCode, actualCountryCode);
            }
        }

        [TestMethod()]
        public void FromHandleTest()
        {
            // Arrange
            string expected = "sRGB IEC61966-2.1";

            using (MemoryStream ms = Save(".Resources.sRGB.icc"))
            {
                using (var profile = Profile.Open(ms.GetBuffer()))
                using (var mlu = MultiLocalizedUnicode.FromHandle(profile.ReadTag(TagSignature.ProfileDescription)))
                {
                    // Act
                    string actual = mlu.GetASCII(MultiLocalizedUnicode.NoLanguage, MultiLocalizedUnicode.NoCountry);

                    // Assert
                    Assert.AreEqual(expected, actual);
                }
            }
        }
    }
}
