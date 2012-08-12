function Reduce(key, values) {

    var teamStandings = new Object();
    values.forEach(function (value) {
        var homeTeam;
        if (value.HomeTeamID in teamStandings) {
            homeTeam = teamStandings[value.HomeTeamID];
        } else {
            homeTeam = {
                TeamId: value.HomeTeamID,
                TeamName: value.HomeTeamName,
                GoalsScored: 0,
                GoalsConceded: 0,
                GoalDifference: 0,
                HomePoints: 0,
                AwayPoints: 0,
                TotalPoints: 0,
                Won: 0,
                Lost: 0,
                Drawn: 0,
                Played: 0
            }
            teamStandings[value.HomeTeamID] = homeTeam;
        }

        var awayTeam;
        if (value.AwayTeamID in teamStandings) {
            awayTeam = teamStandings[value.AwayTeamID];
        } else {
            awayTeam = {
                TeamId: value.AwayTeamID,
                TeamName: value.AwayTeamName,
                GoalsScored: 0,
                GoalsConceded: 0,
                GoalDifference: 0,
                HomePoints: 0,
                AwayPoints: 0,
                TotalPoints: 0,
                Won: 0,
                Lost: 0,
                Drawn: 0,
                Played: 0
            }
            teamStandings[value.AwayTeamID] = awayTeam;
        }

        homeTeam.Played += 1;
        awayTeam.Played += 1;

        homeTeam.GoalsScored += parseInt(value.HomeGoals);
        awayTeam.GoalsScored += parseInt(value.AwayGoals);
        homeTeam.GoalsConceded += parseInt(value.AwayGoals);
        awayTeam.GoalsConceded += parseInt(value.HomeGoals);

        homeTeam.GoalDifference = homeTeam.GoalsScored - homeTeam.GoalsConceded;
        awayTeam.GoalDifference = awayTeam.GoalsScored - awayTeam.GoalsConceded;

        if (value.HomeGoals === value.AwayGoals) {
            homeTeam.TotalPoints += 1;
            homeTeam.HomePoints += 1;
            awayTeam.TotalPoints += 1;
            awayTeam.AwayPoints += 1;

            homeTeam.Drawn += 1;
            awayTeam.Drawn += 1;
        }

        if (value.HomeGoals > value.AwayGoals) {
            homeTeam.TotalPoints += 3;
            homeTeam.HomePoints += 3;
            homeTeam.Won += 1;
            awayTeam.Lost += 1;
        }

        if (value.HomeGoals < value.AwayGoals) {
            awayTeam.TotalPoints += 3;
            awayTeam.AwayPoints += 3;
            awayTeam.Won += 1;
            homeTeam.Lost += 1;
        }

    });

    var reduced = { teams: [] };
    reduced.teams = teamStandings;

    return reduced;
}