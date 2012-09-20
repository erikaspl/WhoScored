using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System;
using WhoScored.Model;


namespace WhoScored.Model
{

    public class Country
    {
        public Country() {
			Series = new List<Series>();
			Teams = new List<Team>();
            SupportedSeriesId = new List<CountrySeriesId>();
        }
        public virtual int CountryId { get; set; }
        public virtual int HtCountryId { get; set; }
        public virtual IList<Series> Series { get; set; }
        public virtual IList<Team> Teams { get; set; }
        public virtual IList<CountrySeriesId> SupportedSeriesId { get; set; }
        public virtual string EnglishName { get; set; }
        public virtual int NumberOfLevels { get; set; }
        public virtual int SeasonOffset { get; set; }

        public virtual string CountryName { get; set; }
        public virtual bool CountryInWhoScored { get; set; }

        public virtual TimeSpan SeriesMatchTime { get; set; }
        public virtual short SeriesMatchWeekDay { get; set; }

        public virtual bool IsNewValue { get; set; }

        public virtual void AddSeriesId(int seriesId)
        {
            if (SupportedSeriesId.FirstOrDefault(c => c.HtSeriesId == seriesId) == null)
            {
                var newSeriesId = new CountrySeriesId { Country = this, HtSeriesId = seriesId };
                SupportedSeriesId.Add(newSeriesId);
            }
        }

        public virtual void AddSeriesIdRange(List<int> seriesIdRange)
        {
            seriesIdRange.ForEach(AddSeriesId);            
        }

        public virtual void AddSeries(Series series)
        {
            series.Country = this;
            Series.Add(series);
        }
    }
}
