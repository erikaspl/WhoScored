using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhoScored.Models
{
    public class WorldDetailsModel
    {
        public Settings Settings { get; set; }
        public List<WorldDetails> WorldDetails { get; set; }
        public string SelectedCountry { get; set; }
    }
}