using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CommandMaster.Core.Utils;
using CommandMaster.Core;

namespace CommandMaster.Test.CommandMaster.Core
{
    /// <summary>
    /// Summary description for DirectoryAnalyzerTest
    /// </summary>
    [TestClass]
    public class DirectoryAnalyzerTest
    {
        static string dir = "directory_analyzer_test_dir";
        public DirectoryAnalyzerTest()
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
        public void analyzeCommandsInDirectory()
        {
            //
            // TODO: Add test logic here
            //
            createDir();
            createFile("a.exe");
            createFile("b.cmd");
            createFile("c.bat");
            createFile("d.com");

            var result = DirectoryAnalyzer.analyze(dir);
            Assert.IsTrue(isContainsCommand(result, new Command
            {
                name = "a",
                type = ".exe"
            })); 
            Assert.IsTrue(isContainsCommand(result, new Command
            {
                name = "b",
                type = ".cmd"
            })); 
            Assert.IsTrue(isContainsCommand(result, new Command
            {
                name = "c",
                type = ".bat"
            })); 
            Assert.IsTrue(isContainsCommand(result, new Command
            {
                name = "d",
                type = ".com"
            }));
        }

        [TestMethod]
        public void analyzeInvalidCommand()
        {
            createDir();
            createFile("a.xlsx");
            createFile("a.ppt");
            createFile("bbbb.exe");

            var result = DirectoryAnalyzer.analyze(dir);
            Assert.IsTrue(isContainsCommand(result, new Command
            {
                name = "bbbb",
                type = ".exe"
            }));
            Assert.IsFalse(isContainsCommand(result, new Command
            {
                name = "a",
                type = ".xlsx"
            }));
            Assert.IsFalse(isContainsCommand(result, new Command
            {
                name = "a",
                type = ".ppt"
            }));
        }

        [TestMethod]
        public void analyzeAmbiguousCommand()
        {
            createDir();
            createFile("a.cmd");
            createFile("a.bat");

            var result = DirectoryAnalyzer.analyze(dir);
            Assert.IsTrue(isContainsCommand(result, new Command
            {
                name = "a",
                type = ".cmd"
            }));
            Assert.IsTrue(isContainsCommand(result, new Command
            {
                name = "a",
                type = ".bat"
            }));
        }

        bool isContainsCommand(IEnumerable<ICommand> result, ICommand expected)
        {
            foreach (var cmd in result) {
                if (cmd.name == expected.name &&
                    cmd.type == expected.type)
                    return true;
            }
            return false;
        }

        void createDir()
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            Directory.CreateDirectory(dir);
        }

        void createFile(string fileName)
        {
            using (File.Create(Path.Combine(dir, fileName))) { };
        }
    }
}
