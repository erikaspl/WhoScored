function Reduce(key, values) {
    var teamForm = new Object();

    values.forEach(function (value) {
        var homeTeam;
        if (value.HomeTeamID in teamForm) {
            homeTeam = teamForm[value.HomeTeamID];
        } else {
            homeTeam = {
                TeamId: value.HomeTeamID,
                TeamName: value.HomeTeamName,
                TeamProgress: {}
            }
            teamForm[value.HomeTeamID] = homeTeam;
        }

        var awayTeam;
        if (value.AwayTeamID in teamForm) {
            awayTeam = teamForm[value.AwayTeamID];
        } else {
            awayTeam = {
                TeamId: value.AwayTeamID,
                TeamName: value.AwayTeamName,
                TeamProgress: {}
            }
            teamForm[value.AwayTeamID] = awayTeam;
        }

        if (value.HomeGoals === value.AwayGoals) {
            homeTeam.TeamProgress[value.MatchRound] = {
                MatchRound: value.MatchRound,
                HomeTeamID: value.HomeTeamID,
                HomeTeamName: value.HomeTeamName,
                HomeTeamGoals: value.HomeGoals,
                AwayTeamID: value.AwayTeamID,
                AwayTeamName: value.AwayTeamName,
                AwayTeamGoals: value.AwayGoals,
                Result: 'd'
            };
            awayTeam.TeamProgress[value.MatchRound] = {
                MatchRound: value.MatchRound,
                HomeTeamID: value.HomeTeamID,
                HomeTeamName: value.HomeTeamName,
                HomeTeamGoals: value.HomeGoals,
                AwayTeamID: value.AwayTeamID,
                AwayTeamName: value.AwayTeamName,
                AwayTeamGoals: value.AwayGoals,
                Result: 'D'
            };
        }

        if (value.HomeGoals > value.AwayGoals) {
            homeTeam.TeamProgress[value.MatchRound] = {
                MatchRound: value.MatchRound,
                HomeTeamID: value.HomeTeamID,
                HomeTeamName: value.HomeTeamName,
                HomeTeamGoals: value.HomeGoals,
                AwayTeamID: value.AwayTeamID,
                AwayTeamName: value.AwayTeamName,
                AwayTeamGoals: value.AwayGoals,
                Result: 'w'
            };
            awayTeam.TeamProgress[value.MatchRound] = {
                MatchRound: value.MatchRound,
                HomeTeamID: value.HomeTeamID,
                HomeTeamName: value.HomeTeamName,
                HomeTeamGoals: value.HomeGoals,
                AwayTeamID: value.AwayTeamID,
                AwayTeamName: value.AwayTeamName,
                AwayTeamGoals: value.AwayGoals,
                Result: 'L'
            };
        }

        if (value.HomeGoals < value.AwayGoals) {
            homeTeam.TeamProgress[value.MatchRound] = {
                MatchRound: value.MatchRound,
                HomeTeamID: value.HomeTeamID,
                HomeTeamName: value.HomeTeamName,
                HomeTeamGoals: value.HomeGoals,
                AwayTeamID: value.AwayTeamID,
                AwayTeamName: value.AwayTeamName,
                AwayTeamGoals: value.AwayGoals,
                Result: 'l'
            };
            awayTeam.TeamProgress[value.MatchRound] = {
                MatchRound: value.MatchRound,
                HomeTeamID: value.HomeTeamID,
                HomeTeamName: value.HomeTeamName,
                HomeTeamGoals: value.HomeGoals,
                AwayTeamID: value.AwayTeamID,
                AwayTeamName: value.AwayTeamName,
                AwayTeamGoals: value.AwayGoals,
                Result: 'W'
            };
        }
    });

    var reduced = { teams: [] };
    reduced.teams = teamForm;

    return reduced;
}