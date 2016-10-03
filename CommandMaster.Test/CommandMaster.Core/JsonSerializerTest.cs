using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandMaster.Core.Utils;
using CommandMaster.Core;
using System.Linq;

namespace CommandMaster.Test.CommandMaster.Core
{
    /// <summary>
    /// Summary description for JsonSerializerTest
    /// </summary>
    [TestClass]
    public class JsonSerializerTest
    {
        public JsonSerializerTest()
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
        public void testSerialize()
        {
            var cmd = new Command { name = "shit", type = ".exe" };
            var result = JsonSerializer.objectToString(cmd); 
            var cmd2 = (ICommand)JsonSerializer.stringToObject<Command>(result);
            Assert.AreEqual(cmd2.name, "shit");
            Assert.AreEqual(cmd2.type, ".exe");
        }

        [TestMethod]
        public void testDeserialize()
        {
            var cmd = JsonSerializer.stringToObject<Command>(@"
                {
                    ""name"": ""shit"",
                    ""type"": "".exe""
                }
            ");

            Assert.AreEqual(cmd.name, "shit");
            Assert.AreEqual(cmd.type, ".exe");
        }

        [TestMethod]
        public void testSerializeList()
        {
            var lst = new List<Command>
            {
                new Command { name = "shit", type = ".exe" },
                new Command { name = "wtf", type = ".com" }
            };

            var result = JsonSerializer.objectToString(lst);
            var lst2 = JsonSerializer.stringToObject<List<Command>>(result);
            Assert.AreEqual(lst[0].name, lst2[0].name);
            Assert.AreEqual(lst[1].name, lst2[1].name);
        }

        [TestMethod]
        public void testSerializeFile()
        {
            var a = new Command { name = "hahha", type = "xixixi" };
            JsonSerializer.objectToFile(a, "test.json");
            var newB = (ICommand)JsonSerializer.fileToObject(a.GetType(), "test.json");
            Assert.AreEqual(a.name, newB.name);
            Assert.AreEqual(a.type, newB.type);
        }
    }
}
