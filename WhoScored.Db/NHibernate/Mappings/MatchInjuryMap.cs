using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class MatchInjuryMap : ClassMap<MatchInjury> {

        public MatchInjuryMap()
        {
			Table("match_injuries");
			LazyLoad();
			Id(x => x.MatchInjuryId).GeneratedBy.Identity().Column("match_injury_id");
            References(x => x.Match).Column("match_id").Not.Nullable().Cascade.SaveUpdate();
            References(x => x.MatchTeam).Column("match_team_id").Not.Nullable().Cascade.SaveUpdate();
			Map(x => x.PlayerId).Column("player_id").Not.Nullable();
			Map(x => x.InjuryMinute).Column("injury_minute").Not.Nullable();
			Map(x => x.EventIndex).Column("event_index").Not.Nullable();
			Map(x => x.PlayerName).Column("player_name").Not.Nullable();
			Map(x => x.InjuryType).Column("injury_type").Not.Nullable();
        }
    }
}
