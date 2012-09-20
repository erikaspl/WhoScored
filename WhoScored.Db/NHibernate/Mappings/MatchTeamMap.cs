using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class MatchTeamMap : ClassMap<MatchTeam> {

        public MatchTeamMap()
        {
			Table("match_team");
			LazyLoad();
			Id(x => x.MatchTeamId).GeneratedBy.Increment().Column("match_team_id");
            References(x => x.Team).Column("team_id").Cascade.SaveUpdate();
            Map(x => x.RatingRightDef).Column("rating_right_def").Not.Nullable();
            Map(x => x.TacticType).Column("tactic_type");
            Map(x => x.DressURI).Column("dress_uri").Not.Nullable();
            Map(x => x.TacticSkill).Column("tactic_skill");
            Map(x => x.RatingMidAtt).Column("rating_mid_att").Not.Nullable();
            Map(x => x.RatingMidfield).Column("rating_midfield").Not.Nullable();
            Map(x => x.RatingIndirectSetPiecesDef).Column("rating_indirect_set_pieces_def");
            Map(x => x.Formation).Column("formation").Not.Nullable();
            Map(x => x.RatingLeftAtt).Column("rating_left_att").Not.Nullable();
            Map(x => x.RatingRightAtt).Column("rating_right_att").Not.Nullable();
            Map(x => x.Goals).Column("goals").Not.Nullable();
            Map(x => x.PossessionFirstHalf).Column("possession_first_half").Not.Nullable();
            Map(x => x.PossessionSecondHalf).Column("possession_second_half").Not.Nullable();
            Map(x => x.RatingIndirectSetPiecesAtt).Column("rating_indirect_set_pieces_att");
            Map(x => x.RatingLeftDef).Column("rating_left_def").Not.Nullable();
            Map(x => x.RatingMidDef).Column("rating_mid_def").Not.Nullable();
            HasMany(x => x.MatchBookings).KeyColumn("match_team_id").Inverse();
            HasMany(x => x.MatchInjuries).KeyColumn("match_team_id").Inverse();
            HasMany(x => x.MatchScorers).KeyColumn("match_team_id").Inverse();
        }
    }
}
