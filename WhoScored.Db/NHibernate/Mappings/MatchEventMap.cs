using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class MatchEventMap : ClassMap<MatchEvent> {

        public MatchEventMap()
        {
			Table("match_events");
			LazyLoad();
			Id(x => x.MatchEventId).GeneratedBy.Identity().Column("match_event_id");
            References(x => x.Match).Column("match_id").Not.Nullable().Cascade.SaveUpdate();
			Map(x => x.Minute).Column("minute").Not.Nullable();
			Map(x => x.EventIndex).Column("event_index").Not.Nullable();
			Map(x => x.ObjectPlayerID).Column("object_player_id").Nullable();
			Map(x => x.SubjectPlayerID).Column("subject_player_id").Nullable();
			Map(x => x.EventTypeID).Column("event_type_id").Not.Nullable();
			Map(x => x.EventText).Column("event_text").Not.Nullable().Length(1000);
			Map(x => x.EventVariation).Column("event_variation");
			Map(x => x.SubjectTeamID).Column("subject_team_id").Nullable();
        }
    }
}
