using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class Series {
        public Series()
        {
            SeriesFixtures = new List<SeriesFixture>();
            Matches = new List<Match>();
        }
        public virtual int Id { get; set; }
        public virtual int HtSeriesId { get; set; }
        public virtual IList<SeriesFixture> SeriesFixtures { get; set; }
        public virtual IList<Match> Matches { get; set; } 
        public virtual short LeagueLevel { get; set; }
        public virtual string LeagueLevelUnitName { get; set; }
        public virtual Country Country { get; set; }

        public virtual void AddSeriesFixture(SeriesFixture fixture)
        {
            fixture.Series = this;
            SeriesFixtures.Add(fixture);
        }

        public virtual void AddMatch(Match match)
        {
            match.Series = this;
            Matches.Add(match);
        }

        public virtual void SetCountry(Country country)
        {
            country.Series.Add(this);
            Country = country;
        }
    }
}
