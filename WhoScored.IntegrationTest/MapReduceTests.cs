using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoScored.Db.Mongo;

namespace WhoScored.IntegrationTest
{
    [TestClass]
    public class MapReduceTests
    {
        [TestMethod]
        public void GetSeriesStandings_TestExecution()
        {
            var repository = new WhoScoredRepository();
            var result = repository.GetSeriesStandings(29747, 30);

        }
    }
}
