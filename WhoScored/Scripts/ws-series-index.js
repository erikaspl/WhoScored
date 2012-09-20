/// <reference path="/Scripts/knockout-2.1.0.debug.js" />
/// <reference path="/Scripts/jquery-ui-1.8.19.js" />
/// <reference path="/Scripts/linq.js" />
var viewModel;
var navUp = "up";
var navDown = "down";
var nonNav = "none";
var defaultRound = 14;

$(function () {

    var seriesPage = function () {
        var self = this;
        self.selectedCountry = ko.observable();
        self.countries = [];
        self.selectedSeries = ko.observable();
        self.series = [];
        self.seasons = ko.observableArray();

        self.selectedSeries.subscribe(function (value) {
            if (value != undefined) {
                var allSeasons = viewModel.seasons();
                var maxSeason = GetMaxSeason(allSeasons);

                if (maxSeason != null) {
                    self.selectedSeason(parseInt(maxSeason.Text));
                }
            }
        });

        self.selectedCountry.subscribe(function (country) {
            self.seasons([]);
            self.selectedSeries(undefined);
            self.series = [];
            if (country != null) {
                SetSeriesSeasonViewModel(country.LeagueID);
            }

        } .bind(self));

        self.resetButtons = false;

        self.seasons.subscribe(function () {
            var allSeasons = viewModel.seasons();
            var maxSeason = GetMaxSeason(allSeasons);
            self.resetButtons = true;
            if (maxSeason != null) {
                self.selectedSeason(parseInt(maxSeason.Text));
            }
        } .bind());

        self.visibleSeasons = ko.observableArray();

        self.selectedButton = ko.observable();
        self.selectButton = function (visibleSeasons) {

            self.selectedButton(visibleSeasons);
            var season = self.selectedButton().name();
            if (self.selectedButton().isNavButton()) {
                SetVisibleSeasons(self.selectedButton().navButton());
                season = GetFirstAvailableButtonVal();
            }
            self.currentSeason(season);
        };


        self.currentSeason = ko.observable();

        self.selectedSeason = ko.computed({
            read: function () {
                return this.currentSeason();
            },
            write: function (value) {
                this.currentSeason(value);
            },
            owner: self
        });

        self.selectedSeason.subscribe(function (value) {
            if (value != undefined) {
                ActivateSeasonButton(value);
                self.resetButtons = false;
                UnselectMatchRounds();
                SetAllMatchResultsViewModel();
            }
        });

        self.maxRound = ko.observable();

        self.selectedRound = ko.observable();
        self.rounds = ko.observableArray();
        self.selectRound = function (round) {
            self.selectedRound(round.title());
            this.select(true);
            SetMatchResultsViewModel();
        };

        self.standings = ko.observableArray([]);
        self.hasStandings = function () {
            return (self.standings().length > 0);
        };

        self.allMatchResults = ko.observableArray([]);
        self.allMatchResults.subscribe(function (value) {
            var maxRound = 0;
            if (value.length > 0) {
                maxRound = Enumerable.from(value).max(function (m) {

                    return m.MatchRound;
                });
            }
            self.maxRound(maxRound);
        });

        self.matchResults = ko.observableArray([]);
        self.hasMatchResults = function () {
            return (self.matchResults().length > 0);
        };

        self.MatchDayText = ko.computed(function () {
            var round = self.selectedRound() === undefined ? "" : self.selectedRound();
            return "Matchday " + round;
        });

        self.selectedSeriesHeader = ko.computed(function () {
            var header = "League";
            if (this.selectedCountry()) {
                header = this.selectedCountry().EnglishName;
                if (this.selectedSeries()) {
                    header += ' / ' + $('#seriesSelect option:selected').text();
                }
            }

            return header;

        }, self);
    };

    viewModel = new seriesPage();

    //    $("#selectedSeriesHeaderClass").dataBind({
    //        text: "'selectedSeriesHeader'"
    //    });

    $("#countries").dataBind({
        options: "countries",
        optionsText: "'EnglishName'",
        value: "selectedCountry"
    });

    $("#seriesSelect").dataBind({
        options: "series",
        optionsText: "'Text'",
        optionsValue: "'Value'",
        value: "selectedSeries"
    });

    $("#seasonSelect").dataBind({
        options: "seasons",
        optionsText: "'Text'",
        optionsValue: "'Value'",
        value: "selectedSeason"
    });

    $('body').on('click', '.btn.navbtn', function (e) {
        e.stopImmediatePropagation();
        $(".navbtn").removeClass('active');
    });

    SetCountryViewModel(viewModel);

    InitVisibleSeasons();
    InitMatchRounds();
    ko.applyBindings(viewModel);
});

function SetCountryViewModel(viewModel) {

    $.ajax({
        type: "POST",
        url: "Series/WorldDetails/",
        traditional: true,
        success: function (data) {
            var activeCountry;
            $.each(data['WorldDetails'], function (i, v) {
                if (v.EnglishName == data['SelectedCountry']) {
                    activeCountry = v;
                    return;
                }
            });

            viewModel.countries = data['WorldDetails'];
            viewModel.selectedCountry(activeCountry);
        }
    });
}

function SetSeriesSeasonViewModel(countryId) {

    $.ajax({
        type: "POST",
        url: "/Series/SeasonAndSeriesListForCountry/",
        data: {
            countryId: countryId
        },
        dataType: 'json',
        traditional: true,
        success: function (data) {
            viewModel.seasons([]);
            viewModel.series = data.Series;
            if (data.Series.length > 0) {
                viewModel.selectedSeries(data.Series[0]);
            }

            viewModel.seasons(data.Seasons);
        }
    });
}
var CalcStandingEntity = function(teamId) {
    var self = this;
    self.TeamId = teamId;
    self.Position = 0;
    self.TeamName = '';
    self.Played = 0;
    self.Won = 0;
    self.Drawn = 0;
    self.Lost = 0;
    self.GoalsScored = 0;
    self.GoalsConceded = 0;
    self.GoalDifference = 0;
    self.TotalPoints = 0;
};

function SetStandingsViewModel() {

    var round = viewModel.selectedRound();
    if (round == null) {
        round = viewModel.maxRound();
    }

    var allResults = viewModel.allMatchResults();
    allResults = Enumerable.from(allResults).where(function (r) { return r.MatchRound <= round; }).toArray();

    var allHomeTeams = Enumerable.from(allResults)
        .distinct(function (s) { return s.HomeTeamId; })    
        .select(function (s) { return s.HomeTeamId; })
        .toArray();

    var allAwayTeams = Enumerable.from(allResults)
        .distinct(function (s) { return s.AwayTeamId; })
        .select(function (s) { return s.AwayTeamId; })
        .toArray();

    var allTeams = Enumerable.from(allHomeTeams).union(allAwayTeams).distinct(function (t) { return t; }).toArray();
    var calcStandings = [];
    $.each(allTeams, function () {
        var teamId = parseInt(this);
        var teamResults = Enumerable.from(allResults)
            .where(function (r) { return r.HomeTeamId === teamId || r.AwayTeamId === teamId; })
            .toArray();
        var teamStandings = new CalcStandingEntity(teamId);
        $.each(teamResults, function () {

            teamStandings.Played += 1;
            var homeTeamGoals = parseInt(this.HomeTeamGoals);
            var awayTeamGoals = parseInt(this.AwayTeamGoals);
            if (this.HomeTeamId === teamId) {
                teamStandings.TeamName = this.HomeTeamName;
                teamStandings.GoalsScored += homeTeamGoals;
                teamStandings.GoalsConceded += awayTeamGoals;
                teamStandings.GoalDifference += homeTeamGoals - awayTeamGoals;
                if (homeTeamGoals > awayTeamGoals) {
                    teamStandings.TotalPoints += 3;
                    teamStandings.Won += 1;
                }
                if (homeTeamGoals === awayTeamGoals) {
                    teamStandings.TotalPoints += 1;
                    teamStandings.Drawn += 1;
                }
                if (homeTeamGoals < awayTeamGoals) {
                    teamStandings.Lost += 1;
                }
            }

            if (this.AwayTeamId === teamId) {
                teamStandings.TeamName = this.AwayTeamName;
                teamStandings.GoalsScored += awayTeamGoals;
                teamStandings.GoalsConceded += homeTeamGoals;
                teamStandings.GoalDifference += awayTeamGoals - homeTeamGoals;
                if (homeTeamGoals < awayTeamGoals) {
                    teamStandings.TotalPoints += 3;
                    teamStandings.Won += 1;
                }
                if (homeTeamGoals === awayTeamGoals) {
                    teamStandings.TotalPoints += 1;
                    teamStandings.Drawn += 1;
                }
                if (homeTeamGoals > awayTeamGoals) {
                    teamStandings.Lost += 1;
                }
            }
        });
        calcStandings.push(teamStandings);
    });

    calcStandings = Enumerable.from(calcStandings).orderByDescending(function (s) { return s.TotalPoints; })
        .thenByDescending(function (s) { return s.GoalDifference; })
        .thenByDescending(function (s) { return s.GoalsScored; })
        .toArray();

    var position = 1;
    var standingsArray = [];
    allResults = viewModel.allMatchResults();
    $.each(calcStandings, function () {
        this.Position = position;
        standingsArray.push(new StandingEntity(this, allResults, round));
        position += 1;
    });
    viewModel.standings(standingsArray);   
}

function SetMatchResultsViewModel() {
    var round = viewModel.selectedRound();
    if (round == null) {
        round = viewModel.maxRound();
    }

    var allResults = viewModel.allMatchResults();
    
    var matchResultArray = Enumerable.from(allResults).where(function (s) {
        return s.MatchRound == round;
    }).orderBy(function (s) {
        return s.MatchId;
    }).select(function (s) { return s; }).toArray();

    viewModel.matchResults(matchResultArray);   
    SetStandingsViewModel();    
}

function SetAllMatchResultsViewModel() {
    viewModel.selectedRound(undefined);
    $.ajax({
        type: "POST",
        url: "/Series/AllSeriesResults/",
        data: {
            seriesId: viewModel.selectedSeries(),
            season: viewModel.currentSeason()
        },
        dataType: 'json',
        traditional: true,
        success: function (matchResults) {
            var matchResultArray = [];
            $.each(matchResults, function () {
                matchResultArray.push(new MatchResultsEntity(this));
            });
            viewModel.allMatchResults(matchResultArray);
            SetMatchResultsViewModel();
            SetMatchRoundButtons();
        }
    });
}

var MatchResultsEntity = function (matchResult) {
    var self = this;
    self.MatchId = matchResult.MatchId;
    self.MatchRound = matchResult.MatchRound;
    self.HomeTeamName = matchResult.HomeTeamName;
    self.HomeTeamId = matchResult.HomeTeamID;
    self.HomeTeamGoals = matchResult.HomeTeamGoals;
    self.AwayTeamName = matchResult.AwayTeamName;
    self.AwayTeamGoals = matchResult.AwayTeamGoals;
    self.AwayTeamId = matchResult.AwayTeamID;
    self.Score = ko.computed(function () {
        return self.HomeTeamGoals + " : " + self.AwayTeamGoals;
    });
};


var maxResults = 10;



var StandingEntity = function (standing, allResults, selectedRound) {
    var self = this;
    self.TeamId = standing.TeamId;
    self.Position = standing.Position;
    self.TeamName = standing.TeamName;
    self.Played = standing.Played;
    self.Won = standing.Won;
    self.Drawn = standing.Drawn;
    self.Lost = standing.Lost;
    self.GoalsScored = standing.GoalsScored;
    self.GoalsConceded = standing.GoalsConceded;
    self.GoalDifference = standing.GoalDifference;
    self.TotalPoints = standing.TotalPoints;
    self.Results = [];

    var teamResults = Enumerable.from(allResults).where(function (s) {
        return s.HomeTeamId == self.TeamId || s.AwayTeamId == self.TeamId;
    }).orderBy(function (s) {
        return s.MatchRound;
    }).select(function (s) { return s; }).toArray();

    var firstIndex = selectedRound > maxResults ? selectedRound - maxResults : 0;
    var lastIndex = selectedRound;

    for (var i = firstIndex; i < lastIndex; i++) {
        self.Results.push(new ResultEntity(teamResults[i], self.TeamId));
    }
};

var ResultEntity = function (result, teamId) {
    var self = this;
    //self.PositionChange = result.PositionChange;
    self.MatchRound = result.MatchRound;
    self.HomeTeamId = result.HomeTeamId;
    self.HomeTeamName = result.HomeTeamName;
    self.HomeTeamGoals = result.HomeTeamGoals;
    self.AwayTeamId = result.AwayTeamID;
    self.AwayTeamName = result.AwayTeamName;
    self.AwayTeamGoals = result.AwayTeamGoals;

    self.Title = ko.computed(function () {
        return self.HomeTeamName + " " + self.HomeTeamGoals + "-" + self.AwayTeamGoals + " " + self.AwayTeamName;
    });

    var intTeamId = parseInt(teamId);

    self.ResultSymbol = "d";
    if (result.HomeTeamId === intTeamId) {
        if (self.HomeTeamGoals > self.AwayTeamGoals) {
            self.ResultSymbol = "w";
        }
        if (self.HomeTeamGoals < self.AwayTeamGoals) {
            self.ResultSymbol = "L";
        }
        if (self.HomeTeamGoals === self.AwayTeamGoals) {
            self.ResultSymbol = "d";
        }
    }

    if (result.AwayTeamId === intTeamId) {
        if (self.HomeTeamGoals > self.AwayTeamGoals) {
            self.ResultSymbol = "l";
        }
        if (self.HomeTeamGoals < self.AwayTeamGoals) {
            self.ResultSymbol = "W";
        }
        if (self.HomeTeamGoals === self.AwayTeamGoals) {
            self.ResultSymbol = "D";
        }
    }
};

var seasonButtonCount = 8;
var lastButtonIndex = seasonButtonCount - 1;

var SeasonButton = function (name) {
    var self = this;
    self.name = ko.observable(name);
    self.selected = ko.observable(false);
    self.hidden = ko.observable(false);

    self.navButton = ko.observable(nonNav);
    self.isNavButton = ko.observable(false);

    self.navButton.subscribe(function (navDirection) {
        if (navDirection == navUp || navDirection == navDown) {
            self.navButton(navDirection);
            self.name("...");
            self.isNavButton(true);
        } else {
            self.navButton(nonNav);
            self.name("");
            self.isNavButton(false);
        }
    } .bind());
};

function GetMaxSeason(seasons) {
    return ko.utils.arrayFirst(seasons, function (season) {
        return parseInt(season.Value) === Math.max.apply(null, ko.utils.arrayMap(seasons, function (e) {
            return parseInt(e.Value);
        }));
    });
}

function InitVisibleSeasons() {
    viewModel.visibleSeasons([]);

    for (var i = 1; i < seasonButtonCount + 1; i++) {
        viewModel.visibleSeasons().push(new SeasonButton(""));
    }
}

function UnselectAllSeasons() {
    for (var i = 0; i < lastButtonIndex; i++) {
        viewModel.visibleSeasons()[i].selected(false);
    }
}

function SelectFirstAvailableButton() {
    for (var i = 0; i < lastButtonIndex; i++) {
        if (!viewModel.visibleSeasons()[i].isNavButton()) {
            viewModel.visibleSeasons()[i].selected(true);
            break;
        }
    }
}

function GetFirstAvailableButtonVal() {
    var rezValue;
    for (var i = 0; i < lastButtonIndex; i++) {
        if (!viewModel.visibleSeasons()[i].isNavButton()) {
            rezValue = viewModel.visibleSeasons()[i].name();
            break;
        }
    }

    return rezValue;
}

function ActivateSeasonButton(seasonValue) {
    var buttonSelected = false;
    UnselectAllSeasons();

    if (!viewModel.resetButtons) {
        for (var i = 0; i < lastButtonIndex; i++) {
            if (!viewModel.visibleSeasons()[i].hidden() && viewModel.visibleSeasons()[i].name() == seasonValue) {
                viewModel.visibleSeasons()[i].selected(true);
                buttonSelected = true;
                break;
            }
        }
    }

    if (!buttonSelected) {
        var allSeasons = viewModel.seasons();
        if (allSeasons.length > 0) {
            var lastSeason = allSeasons[0].Text;
            SetSeasons(seasonValue, lastSeason, false);
            SelectFirstAvailableButton();
        }
    }
}

function SetVisibleSeasons(navDirection) {
    var allSeasons = viewModel.seasons();
    var lastSeason = allSeasons[0].Text;
    var currentSeason = lastSeason;
    var buttonValue;

    if (navDirection == navUp) {
        for (var j = 0; j < lastButtonIndex; j++) {
            buttonValue = parseInt(viewModel.visibleSeasons()[j].name());
            if (!viewModel.visibleSeasons()[j].hidden() && !isNaN(buttonValue)) {
                currentSeason = buttonValue + seasonButtonCount - 2;
                if ((currentSeason - lastSeason) > -2) {
                    currentSeason = lastSeason;
                }
                break;
            }
        }
    } else if (navDirection == navDown) {
        for (var j = lastButtonIndex; j > 0; j--) {
            buttonValue = parseInt(viewModel.visibleSeasons()[j].name());
            if (!viewModel.visibleSeasons()[j].hidden() && !isNaN(buttonValue)) {
                currentSeason = buttonValue - 1;
                break;
            }
        }
    }

    SetSeasons(currentSeason, lastSeason, true);
}


function SetSeasons(straringSeason, lastSeason) {
    var startingIndex = 0;
    if (straringSeason != lastSeason) {
        viewModel.visibleSeasons()[0].navButton(navUp);
        viewModel.visibleSeasons()[0].hidden(false);
        startingIndex = 1;
    }

    var currentSeason = straringSeason;
    for (var i = startingIndex; i < lastButtonIndex; i++) {
        if (currentSeason > 0) {
            viewModel.visibleSeasons()[i].navButton(nonNav);
            viewModel.visibleSeasons()[i].name(currentSeason);
            viewModel.visibleSeasons()[i].hidden(false);
            currentSeason--;
        } else {
            viewModel.visibleSeasons()[i].hidden(true);
        }
    }

    if (currentSeason > 1) {
        viewModel.visibleSeasons()[lastButtonIndex].navButton(navDown);
        viewModel.visibleSeasons()[lastButtonIndex].hidden(false);
    } else {
        viewModel.visibleSeasons()[lastButtonIndex].hidden(true);
    }
}

var numberOfRounds = 14;
var lastRoundIndex = numberOfRounds - 1;

var MatchRoundButton = function (title) {
    var self = this;
    self.title = ko.observable(title);
    self.selected = ko.observable(false);
    self.select = ko.computed({
        read: function () {
            return this.selected();
        },
        write: function (value) {
            UnselectMatchRounds();
            this.selected(value);
        },
        owner: this
    });
    self.enabled = ko.observable(true);
};

function InitMatchRounds() {
    viewModel.rounds([]);

    for (var i = 1; i < numberOfRounds + 1; i++) {
        viewModel.rounds().push(new MatchRoundButton(i));
    }
}

function UnselectMatchRounds() {
    for (var i = 0; i < numberOfRounds; i++) {
        if (viewModel.rounds()[i] != undefined) {
            viewModel.rounds()[i].selected(false);
        }
    }
}

function SetMatchRoundButtons() {
    var round = viewModel.selectedRound();
    if (round == null) {
        round = viewModel.maxRound();
    }

    for (var i = 0; i < numberOfRounds; i++) {
        if (viewModel.rounds()[i] != undefined) {
            if (viewModel.rounds()[i].title() === round) {
                viewModel.rounds()[i].selected(true);
                viewModel.selectedRound(round);
            }
            if (viewModel.rounds()[i].title() > round) {
                viewModel.rounds()[i].enabled(false);
            }

            if (viewModel.rounds()[i].title() <= round) {
                viewModel.rounds()[i].enabled(true);
            }
        }
    }
}


