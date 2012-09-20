using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoScored.Model;

namespace WhoScored.Models
{
    public class WorldDetailsModel
    {
        public Model.Settings Settings { get; set; }
        public IList<ICountryDetails> WorldDetails { get; set; }
        public string SelectedCountry { get; set; }
        public int CurrentSeason { get; set; }
    }
}