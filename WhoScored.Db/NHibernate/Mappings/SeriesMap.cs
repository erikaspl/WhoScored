using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class SeriesMap : ClassMap<Series> {

        public SeriesMap()
        {
			Table("series");
			LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("series_id");
            References(x => x.Country).Column("country_id").Not.Nullable();
            Map(x => x.HtSeriesId).Column("ht_series_id");
			Map(x => x.LeagueLevel).Column("league_level").Not.Nullable();
			Map(x => x.LeagueLevelUnitName).Column("league_level_unit_name").Not.Nullable();			
			HasMany(x => x.SeriesFixtures).Cascade.SaveUpdate().Inverse();
            HasMany(x => x.Matches).Cascade.SaveUpdate().Inverse();
        }
    }
}
