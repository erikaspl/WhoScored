using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class TeamMap : ClassMap<Team> {

        public TeamMap()
        {
			Table("teams");
			LazyLoad();
            Id(x => x.TeamId).GeneratedBy.Assigned().Column("team_id");
            References(x => x.Country).Column("country_id").Not.Nullable();
            //Map(x => x.HtTeamId).Column("ht_team_id").Not.Nullable().Unique();
			Map(x => x.TeamName).Column("team_name").Not.Nullable();
            HasMany(x => x.MatchTeams).KeyColumn("team_id").Inverse();
            HasMany(x => x.SeriesFixtures).KeyColumn("away_team_id").KeyColumn("home_team_id").Inverse();
        }
    }
}
