using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
   
    
    public class SeriesFixtureMap : ClassMap<SeriesFixture> {
        
        public SeriesFixtureMap() {
			Table("series_fixtures");
			LazyLoad();
			Id(x => x.MatchId).GeneratedBy.Identity().Column("match_id");
            References(x => x.AwayTeam, "away_team_id").Not.Nullable().Cascade.SaveUpdate();
            References(x => x.HomeTeam, "home_team_id").Not.Nullable().Cascade.SaveUpdate();
            References(x => x.Series).Column("series_id").Not.Nullable();
            Map(x => x.HtMatchId).Column("ht_match_id").Not.Nullable().Unique();
			Map(x => x.AwayGoals).Column("away_goals");
			Map(x => x.MatchDate).Column("match_date").Not.Nullable();
			Map(x => x.HomeGoals).Column("home_goals");
			Map(x => x.Season).Column("season").Not.Nullable();
            Map(x => x.MatchRound).Column("match_round").Not.Nullable();
            Map(x => x.MigratedMatchCount).Formula("(select count(*) from Matches where Matches.ht_match_id = ht_match_id)");
        }
    }
}
