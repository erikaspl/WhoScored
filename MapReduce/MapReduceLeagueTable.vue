{
  "Collection_MapReduce" : {
    "jsonVersion" : 1,
    "reduceCode" : "function Reduce(key, values) {\r\n\r\n\tvar teamStandings = new Object();\r\n\tvalues.forEach(function(value) {\t\r\n\t\tvar homeTeam;\r\n\t\tif (value.HomeTeamID in teamStandings){\r\n\t\t\thomeTeam = teamStandings[value.HomeTeamID];\r\n\t\t}else{\r\n\t\t\thomeTeam = {\r\n\t\t\t\t\tTeamId:value.HomeTeamID,\r\n\t\t\t\t\tTeamName:value.HomeTeamName,\r\n\t\t\t\t\tGoalsScored:0,\r\n\t\t\t\t\tGoalsConceded:0,\r\n\t\t\t\t\tGoalDifference:0,\r\n\t\t\t\t\tHomePoints:0,\r\n\t\t\t\t\tAwayPoints:0,\r\n\t\t\t\t\tTotalPoints:0,\r\n\t\t\t\t\tWon:0,\r\n\t\t\t\t\tLost:0,\r\n\t\t\t\t\tDrawn:0,\r\n\t\t\t\t\tPlayed:0\r\n\t\t\t}\r\n\t\t\tteamStandings[value.HomeTeamID] = homeTeam;\t\r\n\t\t}\r\n\t\t\r\n\t\tvar awayTeam;\r\n\t\tif (value.AwayTeamID in teamStandings){\r\n\t\t\tawayTeam = teamStandings[value.AwayTeamID];\r\n\t\t}else{\r\n\t\t\tawayTeam = {\r\n\t\t\t\t\tTeamId:value.AwayTeamID,\r\n\t\t\t\t\tTeamName:value.AwayTeamName,\r\n\t\t\t\t\tGoalsScored:0,\r\n\t\t\t\t\tGoalsConceded:0,\r\n\t\t\t\t\tGoalDifference:0,\r\n\t\t\t\t\tHomePoints:0,\r\n\t\t\t\t\tAwayPoints:0,\r\n\t\t\t\t\tTotalPoints:0,\r\n\t\t\t\t\tWon:0,\r\n\t\t\t\t\tLost:0,\r\n\t\t\t\t\tDrawn:0,\r\n\t\t\t\t\tPlayed:0\r\n\t\t\t}\r\n\t\t\tteamStandings[value.AwayTeamID] = awayTeam;\t\r\n\t\t}\r\n\t\t\r\n\t\thomeTeam.Played += 1;\r\n\t\tawayTeam.Played += 1;\r\n\t\t\r\n\t\thomeTeam.GoalsScored += parseInt(value.HomeGoals);\r\n\t\tawayTeam.GoalsScored += parseInt(value.AwayGoals);\r\n\t\thomeTeam.GoalsConceded += parseInt(value.AwayGoals);\r\n\t\tawayTeam.GoalsConceded += parseInt(value.HomeGoals);\r\n\t\t\r\n\t\thomeTeam.GoalDifference = homeTeam.GoalsScored - homeTeam.GoalsConceded;\r\n\t\tawayTeam.GoalDifference = awayTeam.GoalsScored - awayTeam.GoalsConceded;\r\n\t\t\r\n\t\tif (value.HomeGoals === value.AwayGoals){\r\n\t\t\t\thomeTeam.TotalPoints += 1;\r\n\t\t\t\thomeTeam.HomePoints += 1;\t\t\t\r\n\t\t\t\tawayTeam.TotalPoints += 1;\r\n\t\t\t\tawayTeam.AwayPoints += 1;\r\n\t\t\t\t\r\n\t\t\t\thomeTeam.Drawn += 1;\r\n\t\t\t\tawayTeam.Drawn += 1;\r\n\t\t\t}\r\n\t\t\t\r\n\t\t\tif (value.HomeGoals > value.AwayGoals){\r\n\t\t\t\thomeTeam.TotalPoints += 3;\r\n\t\t\t\thomeTeam.HomePoints += 3;\r\n\t\t\t\thomeTeam.Won += 1;\r\n\t\t\t\tawayTeam.Lost += 1;\r\n\t\t\t}\r\n\t\t\t\r\n\t\t\tif (value.HomeGoals < value.AwayGoals){\r\n\t\t\t\tawayTeam.TotalPoints += 3;\r\n\t\t\t\tawayTeam.HomePoints += 3;\r\n\t\t\t\tawayTeam.Won += 1;\r\n\t\t\t\thomeTeam.Lost += 1;\r\n\t\t\t}\t\r\n\t\t\r\n\t});\r\n\t\r\n\tvar reduced = {teams :[]};\r\n\treduced.teams = teamStandings;\r\n//\t\r\n//\tfor (i=0; i < teamStandings.lenght; i++) {\r\n//\t\t.push(teamStandings[i]);\r\n//\t}\r\n\t\r\n\treturn reduced;\r\n}",
    "mapCode" : "function Map() {\r\n\temit({SeriesId : this.LeagueLevelUnitID, Season: this.MatchSeason},\r\n\t{\t\r\n\t\t'Season': this.MatchSeason,\r\n\t\t'HomeTeamID': this.MatchHomeTeam.HomeTeamID,\r\n\t\t'HomeTeamName': this.MatchHomeTeam.HomeTeamName,\r\n\t\t'HomeGoals': this.MatchHomeTeam.HomeGoals,\r\n\t\t'AwayTeamID': this.MatchAwayTeam.AwayTeamID,\r\n\t\t'AwayTeamName': this.MatchAwayTeam.AwayTeamName,\r\n\t\t'AwayGoals': this.MatchAwayTeam.AwayGoals\r\n\t});\r\n}",
    "jsMode" : false,
    "dbName" : "",
    "isSharded" : false,
    "keepTemp" : false,
    "format" : {
      "index" : 2,
      "text" : "Inline"
    },
    "collName" : "",
    "doLimit" : false,
    "limit" : 0.0,
    "sort" : "",
    "where" : "",
    "query" : "",
    "finalizeCode" : "function Finalize(key, reduced) {\r\n\treturn reduced;\r\n}"
  }
}