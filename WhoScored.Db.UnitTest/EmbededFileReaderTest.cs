using WhoScored.Db.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WhoScored.Db.UnitTest
{
    
    [TestClass()]
    public class EmbededFileReaderTest
    {
        [TestMethod]
        public void ReadFileTest_TeamFormMap_ExpectContent()
        {
            var target = new EmbededFileReader();
            const string name = "TeamForm";
            const string mapName = "map.js";
            string actual = target.ReadFile(name, mapName);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void ReadFileTest_TeamFormReduce_ExpectContent()
        {
            var target = new EmbededFileReader();
            const string name = "TeamForm";
            const string mapName = "reduce.js";
            string actual = target.ReadFile(name, mapName);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void ReadFileTest_TeamFormFinalize_ExpectContent()
        {
            var target = new EmbededFileReader();
            const string name = "TeamForm";
            const string mapName = "finalize.js";
            string actual = target.ReadFile(name, mapName);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void ReadFileTest_SeriesStandingsMap_ExpectContent()
        {
            var target = new EmbededFileReader();
            const string name = "SeriesStandings";
            const string mapName = "map.js";
            string actual = target.ReadFile(name, mapName);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void ReadFileTest_SeriesStandingsReduce_ExpectContent()
        {
            var target = new EmbededFileReader();
            const string name = "SeriesStandings";
            const string mapName = "reduce.js";
            string actual = target.ReadFile(name, mapName);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void ReadFileTest_SeriesStandingsFinalize_ExpectContent()
        {
            var target = new EmbededFileReader();
            const string name = "SeriesStandings";
            const string mapName = "finalize.js";
            string actual = target.ReadFile(name, mapName);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }
    }
}
