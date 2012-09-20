using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class Team {
        public Team()
        {
            MatchTeams = new List<MatchTeam>();
            SeriesFixtures = new List<SeriesFixture>();
        }
        public virtual int TeamId { get; set; }
        //public virtual int HtTeamId { get; set; }
        public virtual IList<MatchTeam> MatchTeams { get; set; }
        public virtual IList<SeriesFixture> SeriesFixtures { get; set; }
        public virtual string TeamName { get; set; }
        public virtual Country Country { get; set; }

        public virtual void AddMatchTeam(MatchTeam team)
        {
            team.Team = this;
            MatchTeams.Add(team);
        }
    }
}
