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
        [DeploymentItem("Mongo/MapReduce/TeamForm")]
        public void GetSeriesStandings_TestExecution()
        {
            var repository = new WhoScoredRepository();
            var result = repository.GetSeriesStandingsWithResults(29747, 30, 14);

        }
    }
}
