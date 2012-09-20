using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{      
    public class CountryMap : ClassMap<Country> {

        public CountryMap()
        {
			Table("countries");
			LazyLoad();
			Id(x => x.CountryId).GeneratedBy.Increment().Column("id");
            Map(x => x.HtCountryId).Column("ht_country_id").Not.Nullable().Unique();
			Map(x => x.CountryName).Column("country_name").Not.Nullable();
			Map(x => x.NumberOfLevels).Column("number_of_levels").Not.Nullable();
			Map(x => x.EnglishName).Column("english_name").Not.Nullable();
			Map(x => x.SeasonOffset).Column("season_offset").Not.Nullable();
			Map(x => x.CountryInWhoScored).Column("country_in_whoscored").Not.Nullable();
            Map(x => x.SeriesMatchTime).Column("series_match_time");
            Map(x => x.SeriesMatchWeekDay).Column("series_match_week_day");
            HasMany(x => x.Series).Cascade.SaveUpdate().Inverse();
            HasMany(x => x.Teams).Inverse();
            HasMany(x => x.SupportedSeriesId).Cascade.SaveUpdate().Inverse();
        }
    }
}
