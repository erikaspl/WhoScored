using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhoScored.Models
{
    public class MigrationModel
    {
        public Settings Settings { get; set; }
        public List<LeagueDetails> LeagueDetails { get; set; }
    }
}