{
  "Collection_MapReduce" : {
    "jsonVersion" : 1,
    "reduceCode" : "function Reduce(key, values) {\r\n\r\n\tvar teamForm = new Object();\r\n\t\r\n\tvalues.forEach(function(value) {\t\r\n\t\tvar homeTeam;\r\n\t\tif (value.HomeTeamID in teamForm){\r\n\t\t\thomeTeam = teamForm[value.HomeTeamID];\r\n\t\t}else{\r\n\t\t\thomeTeam = {\r\n\t\t\t\t\tTeamId:value.HomeTeamID,\r\n\t\t\t\t\tTeamName:value.HomeTeamName,\r\n\t\t\t\t\tTeamProgress: {}\r\n\t\t\t}\r\n\t\t\tteamForm[value.HomeTeamID] = homeTeam;\t\r\n\t\t}\r\n\t\t\r\n\t\tvar awayTeam;\r\n\t\tif (value.AwayTeamID in teamForm){\r\n\t\t\tawayTeam = teamForm[value.AwayTeamID];\r\n\t\t}else{\r\n\t\t\tawayTeam = {\r\n\t\t\t\t\tTeamId:value.HomeTeamID,\r\n\t\t\t\t\tTeamName:value.HomeTeamName,\r\n\t\t\t\t\tTeamProgress: {}\r\n\t\t\t}\r\n\t\t\tteamForm[value.AwayTeamID] = awayTeam;\t\r\n\t\t}\r\n\t\tvar homeResult;\r\n\t\tif (value.HomeGoals === value.AwayGoals){\r\n\t\t\thomeTeam.TeamProgress[value.MatchRound] = {\r\n\t\t\t\tMatchRound:value.MatchRound,\r\n\t\t\t\tHomeTeamName: value.HomeTeamName,\r\n\t\t\t\tHomeTeamGoals: value.HomeGoals,\r\n\t\t\t\tAwayTeamName: value.AwayTeamName,\r\n\t\t\t\tAwayTeamGoals: value.AwayGoals,\r\n\t\t\t\tResult:'d'\r\n\t\t\t};\r\n\t\t\tawayTeam.TeamProgress[value.MatchRound] = {\r\n\t\t\t\tMatchRound:value.MatchRound,\r\n\t\t\t\tHomeTeamName: value.HomeTeamName,\r\n\t\t\t\tHomeTeamGoals: value.HomeGoals,\r\n\t\t\t\tAwayTeamName: value.AwayTeamName,\r\n\t\t\t\tAwayTeamGoals: value.AwayGoals,\r\n\t\t\t\tResult:'D'\r\n\t\t\t};\t\t\t\t\r\n\t\t}\r\n\t\t\t\r\n\t\tif (value.HomeGoals > value.AwayGoals){\r\n\t\t\thomeTeam.TeamProgress[value.MatchRound] = {\r\n\t\t\t\tMatchRound:value.MatchRound,\r\n\t\t\t\tHomeTeamName: value.HomeTeamName,\r\n\t\t\t\tHomeTeamGoals: value.HomeGoals,\r\n\t\t\t\tAwayTeamName: value.AwayTeamName,\r\n\t\t\t\tAwayTeamGoals: value.AwayGoals,\r\n\t\t\t\tResult:'w'\r\n\t\t\t};\r\n\t\t\tawayTeam.TeamProgress[value.MatchRound] = {\r\n\t\t\t\tMatchRound:value.MatchRound,\r\n\t\t\t\tHomeTeamName: value.HomeTeamName,\r\n\t\t\t\tHomeTeamGoals: value.HomeGoals,\r\n\t\t\t\tAwayTeamName: value.AwayTeamName,\r\n\t\t\t\tAwayTeamGoals: value.AwayGoals,\r\n\t\t\t\tResult:'L'\r\n\t\t\t};\r\n\t\t}\r\n\t\t\r\n\t\tif (value.HomeGoals < value.AwayGoals){\r\n\t\t\thomeTeam.TeamProgress[value.MatchRound] = {\r\n\t\t\t\tMatchRound:value.MatchRound,\r\n\t\t\t\tHomeTeamName: value.HomeTeamName,\r\n\t\t\t\tHomeTeamGoals: value.HomeGoals,\r\n\t\t\t\tAwayTeamName: value.AwayTeamName,\r\n\t\t\t\tAwayTeamGoals: value.AwayGoals,\r\n\t\t\t\tResult:'l'\r\n\t\t\t};\r\n\t\t\tawayTeam.TeamProgress[value.MatchRound] = {\r\n\t\t\t\tMatchRound:value.MatchRound,\r\n\t\t\t\tHomeTeamName: value.HomeTeamName,\r\n\t\t\t\tHomeTeamGoals: value.HomeGoals,\r\n\t\t\t\tAwayTeamName: value.AwayTeamName,\r\n\t\t\t\tAwayTeamGoals: value.AwayGoals,\r\n\t\t\t\tResult:'W'\r\n\t\t\t};\r\n\t\t}\t\t\r\n\t});\r\n\t\r\n\tvar reduced = {teams :[]};\r\n\treduced.teams = teamForm;\r\n\t\r\n\treturn reduced;\r\n}",
    "finalizeCode" : "function Finalize(key, reduced) {\r\n\treturn reduced;\r\n}",
    "mapCode" : "function Map() {\r\n\temit({SeriesId : this.LeagueLevelUnitID, Season: this.MatchSeason},\r\n\t{\t\r\n\t\t'Season': this.MatchSeason,\r\n\t\t'HomeTeamID': this.MatchHomeTeam.HomeTeamID,\r\n\t\t'HomeTeamName': this.MatchHomeTeam.HomeTeamName,\r\n\t\t'HomeGoals': this.MatchHomeTeam.HomeGoals,\r\n\t\t'AwayTeamID': this.MatchAwayTeam.AwayTeamID,\r\n\t\t'AwayTeamName': this.MatchAwayTeam.AwayTeamName,\r\n\t\t'AwayGoals': this.MatchAwayTeam.AwayGoals,\r\n\t\t'MatchRound': this.MatchRound\r\n\t});\r\n}",
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
    "query" : ""
  }
}