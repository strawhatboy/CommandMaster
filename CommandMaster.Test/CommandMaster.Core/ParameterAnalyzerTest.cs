using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandMaster.Core;

namespace CommandMaster.Test.CommandMaster.Core
{
    /// <summary>
    /// Summary description for ParameterAnalyzerTest
    /// </summary>
    [TestClass]
    public class ParameterAnalyzerTest
    {
        public ParameterAnalyzerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void testAnalyze_WindowsKeyValueParameter()
        {
            //
            // TODO: Add test logic here
            //

            assert(getResult(@"/shit:whatthefuck", new WindowsParameterStyle()),
                @"[key: shit, value: whatthefuck]");
        }

        [TestMethod]
        public void testAnalyze_WindowsFlagParameter()
        {
            assert(getResult(@"-shit", new WindowsParameterStyle()),
                "[shit: true]");
        }

        [TestMethod]
        public void testAnalyze_LinuxKeyValueParameter()
        {
            assert(getResult(@"-shit=whatthefuck", new LinuxParameterStyle()),
                "[key: shit, value: whatthefuck]");
        }

        [TestMethod]
        public void testAnalyze_LinuxFlagParameter()
        {
            assert(getResult(@"--shit", new LinuxParameterStyle()),
                @"[shit: true]");
        }

        [TestMethod]
        public void testAnalyze_annoymousParameters_WindowsKeyValue()
        {
            assert(getResult(@"/shit:whatthefuck"),
                   @"[key: shit, value: whatthefuck]");
        }

        [TestMethod]
        public void testAnalyze_annoymousParameters_WindowsFlag()
        {
            assert(getResult(@"-shit"),
                   @"[shit: true]");
        }

        [TestMethod]
        public void testAnalyze_annoymousParameters_LinuxKeyValue()
        {
            assert(getResult(@"-shit=whatthefuck"),
                   @"[key: shit, value: whatthefuck]");
        }

        [TestMethod]
        public void testAnalyze_annoymousParameters_LinuxFlag()
        {
            assert(getResult(@"--shit"),
                   @"[shit: true]");
        }

        private IParameterType getResult(string parameter, IParameterStyle style = null)
        {
            return style == null ? ParameterAnalyzer.analyze(parameter) :
                ParameterAnalyzer.analyze(parameter, style);
        }
        private void assert(IParameterType result, string expectedString)
        {
            Assert.IsNotNull(result, "should not be null");
            Assert.AreEqual(result.ToString(), expectedString);
        }
    }
}
