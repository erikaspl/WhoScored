using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class MatchMap : ClassMap<Match> {

        public MatchMap()
        {
			Table("matches");
			LazyLoad();
            Id(x => x.MatchId).GeneratedBy.Identity().Column("match_id");
			References(x => x.MatchHomeTeam).Column("match_team_id_home").Cascade.SaveUpdate().Not.Nullable();
            References(x => x.MatchArena).Column("match_arena_id").Cascade.SaveUpdate().Not.Nullable();
			References(x => x.MatchAwayTeam).Column("match_team_id_away").Cascade.SaveUpdate().Not.Nullable();
            References(x => x.Series).Column("series_id").Cascade.SaveUpdate().Not.Nullable();
            Map(x => x.HtMatchId).Column("ht_match_id").Not.Nullable().Unique();
    		Map(x => x.MatchSeason).Column("match_season").Not.Nullable();
			Map(x => x.MatchDate).Column("match_date").Not.Nullable();
			Map(x => x.MatchRound).Column("match_round").Not.Nullable();
			Map(x => x.FinishedDate).Column("finished_date");
			Map(x => x.MatchType).Column("match_type").Not.Nullable();
            HasMany(x => x.MatchBookings).Cascade.SaveUpdate().Inverse();
            HasMany(x => x.MatchEvents).Cascade.SaveUpdate().Inverse();
            HasMany(x => x.MatchInjuries).Cascade.SaveUpdate().Inverse();
            HasMany(x => x.MatchScorers).Cascade.SaveUpdate().Inverse();
        }
    }
}
