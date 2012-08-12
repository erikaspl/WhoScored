function Map() {
    emit({ SeriesId: this.LeagueLevelUnitID, Season: this.MatchSeason },
	                {
	                    'Season': this.MatchSeason,
	                    'HomeTeamID': this.MatchHomeTeam.HomeTeamID,
	                    'HomeTeamName': this.MatchHomeTeam.HomeTeamName,
	                    'HomeGoals': this.MatchHomeTeam.HomeGoals,
	                    'AwayTeamID': this.MatchAwayTeam.AwayTeamID,
	                    'AwayTeamName': this.MatchAwayTeam.AwayTeamName,
	                    'AwayGoals': this.MatchAwayTeam.AwayGoals
	                });
}