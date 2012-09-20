using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public class CountrySeriesId
    {
        public virtual Country Country { get; set; }
        public virtual int SeriesId { get; set; }
        public virtual int HtSeriesId { get; set; }
    }
}
