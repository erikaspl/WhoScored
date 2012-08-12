function Map() {
    emit({ SeriesId: this.LeagueLevelUnitID, Season: this.MatchSeason },
	    {
	        'Season': this.MatchSeason,
	        'HomeTeamID': this.MatchHomeTeam.TeamID,
	        'HomeTeamName': this.MatchHomeTeam.TeamName,
	        'HomeGoals': this.MatchHomeTeam.Goals,
	        'AwayTeamID': this.MatchAwayTeam.TeamID,
	        'AwayTeamName': this.MatchAwayTeam.TeamName,
	        'AwayGoals': this.MatchAwayTeam.Goals,
	        'MatchRound': this.MatchRound
	    });
}