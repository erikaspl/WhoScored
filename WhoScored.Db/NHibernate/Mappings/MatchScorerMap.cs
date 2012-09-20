using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class MatchScorerMap : ClassMap<MatchScorer> {

        public MatchScorerMap()
        {
			Table("match_scorers");
			LazyLoad();
			Id(x => x.MatchScorerId).GeneratedBy.Increment().Column("match_scorer_id");
            References(x => x.Match).Column("match_id").Not.Nullable();
            References(x => x.MatchTeam).Column("match_team_id").Not.Nullable();
			Map(x => x.PlayerId).Column("player_id").Not.Nullable();
			Map(x => x.EventIndex).Column("event_index").Not.Nullable();
			Map(x => x.TeamGoals).Column("team_goals").Not.Nullable();
			Map(x => x.ScorerMinute).Column("scorer_minute").Not.Nullable();
			Map(x => x.PlayerName).Column("player_name").Not.Nullable();
			Map(x => x.OppositionGoals).Column("opposition_goals").Not.Nullable();
        }
    }
}
