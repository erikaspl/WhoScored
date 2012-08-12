using WhoScored.Db.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WhoScored.Model;
using System.Collections.Generic;

namespace WhoScored.IntegrationTest
{
    [TestClass()]
    public class WhoScoredRepositoryTest
    {

        [TestMethod()]
        public void GetMatchResults_FindRecordsInDb()
        {
            WhoScoredRepository target = new WhoScoredRepository();
            int seriesId = 29755; 
            int season = 30;
            List<IMatchResult> actual = target.GetSeriesResults(seriesId, season);
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
