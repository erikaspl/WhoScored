using System;
using System.Collections.Generic;

namespace WhoScored.IntegrationTest
{
    using Model;

    public class TestEntities
    {

        public static Country CreateCountry(int htCountryId, string englishName, string countryName)
        {
            var country = new Country
                {
                    EnglishName = englishName,
                    HtCountryId = htCountryId,
                    CountryInWhoScored = false,
                    NumberOfLevels = 6,
                    CountryName = countryName,
                    SeriesMatchWeekDay = 0,
                    SeriesMatchTime = DateTime.Now.TimeOfDay
                };
            return country;
        }

        public static List<int> CreateSupportedIdList()
        {
            var supportedIds = new List<int>();
            for (int i = 29747; i <= 29767; i++)
            {
                supportedIds.Add(i);
            }
            return supportedIds;
        }

        public static List<Series> CreateSeries(int id, Country country)
        {
            var series1 = CreateSeries(id, country, "A Lyga");
            var seriesFixture1 = CreateSeriesFixture(1000, 30, 1001, 1002, country);
            series1.AddSeriesFixture(seriesFixture1);

            var series2 = new Series { HtSeriesId = 1001, LeagueLevel = 2, LeagueLevelUnitName = "II.1", };
            series2.SetCountry(series1.Country);
            var seriesFixture2 = CreateSeriesFixture(1001, 30, 1003, 1004, country);

            series2.AddSeriesFixture(seriesFixture2);
            return new List<Series> { series1, series2 };
        }

        public static Series CreateSeries(int id, Country country, string seriesName)
        {
            var series = new Series { HtSeriesId = id, LeagueLevel = 1, LeagueLevelUnitName = seriesName, };
            series.SetCountry(country);

            return series;
        }

        public static Team CreateTeam(int id, Country country, string teamName)
        {
            return new Team { TeamId = id, Country = country, TeamName = teamName };
        }

        public static SeriesFixture CreateSeriesFixture(
            int id, short season, int homeTeamId, int awayTeamId, Country country)
        {
            var seriesFixture1 = new SeriesFixture { HtMatchId = id, Season = season, MatchDate = DateTime.Now };
            var homeTeam = CreateTeam(homeTeamId, country, "TeamName1");
            homeTeam.SeriesFixtures.Add(seriesFixture1);
            seriesFixture1.HomeTeam = homeTeam;

            var awayTeam = CreateTeam(awayTeamId, country, "TeamName2");
            awayTeam.SeriesFixtures.Add(seriesFixture1);
            seriesFixture1.AwayTeam = awayTeam;

            return seriesFixture1;
        }

        public static Match CreateMatch(int id, short matchRound)
        {
            return new Match { HtMatchId = id, MatchDate = DateTime.Now, MatchRound = matchRound, MatchType = 2, MatchSeason = 30 };
        }

        public static MatchTeam CreateMatchTeam()
        {
            return new MatchTeam
                {
                    DressURI = "dressUri",
                    Formation = "3-5-2",
                    RatingIndirectSetPiecesAtt = 1,
                    RatingLeftAtt = 2,
                    RatingMidAtt = 3,
                    RatingIndirectSetPiecesDef = 4,
                    RatingLeftDef = 5,
                    RatingMidDef = 6,
                    RatingMidfield = 7,
                    RatingRightAtt = 8,
                    RatingRightDef = 9,
                    TacticType = 2,
                    TacticSkill = 20,
                    Goals = 11,
                    PossessionFirstHalf = 55,
                    PossessionSecondHalf = 51
                };
        }

        public static MatchScorer CreateMatchScorer()
        {
            return new MatchScorer
                {
                    EventIndex = 1,
                    PlayerId = 1050,
                    OppositionGoals = 3,
                    TeamGoals = 4,
                    PlayerName = "Playername",
                    ScorerMinute = 4
                };
        }

        public static MatchInjury CreateMatchInjury()
        {
            return new MatchInjury
                {
                    EventIndex = 1,
                    InjuryMinute = 22,
                    InjuryType = 14,
                    PlayerId = 1100,
                    PlayerName = "InjuredPlayerName"
                };
        }

        public static MatchBooking CreateMatchBooking()
        {
            return new MatchBooking
                {
                    EventIndex = 1,
                    BookingMinute = 55,
                    BookingType = 1,
                    PlayerId = 1200,
                    PlayerName = "BookedPlayerName"
                };
        }

        public static MatchEvent CreateMatchEvent()
        {
            return new MatchEvent
                {
                    EventIndex = 2,
                    EventText = "Event text",
                    EventVariation = 2,
                    EventTypeID = 221,
                    Minute = 25,
                    ObjectPlayerID = null,
                    SubjectPlayerID = null,
                    SubjectTeamID = null
                };
        }

        public static MatchArena CreateMatchArena()
        {
            return new MatchArena
                       {
                           HtArenaId = 2000,
                           ArenaName = "ArenaName",
                           SoldTotal = 2000,
                           WeatherId = 2
                       };
        }

        public static Match CreateMatchFullData(int matchId, Country country, int teamId1, int teamId2, List<int> eventIdList, int matchArenaId, short matchRound)
        {
            var match = CreateMatch(matchId, matchRound);

            var homeMatchTeam = CreateMatchTeam();
            homeMatchTeam.SetTeam(CreateTeam(teamId1, country, "TeamName"));
            match.SetMatchHomeTeam(homeMatchTeam);

            var awayMatchTeam = CreateMatchTeam();
            awayMatchTeam.SetTeam(CreateTeam(teamId2, country, "TeamName"));
            match.SetMatchAwayTeam(awayMatchTeam);

            var matchScorer1 = CreateMatchScorer();
            var matchScorer2 = CreateMatchScorer();
            var matchScorer3 = CreateMatchScorer();

            match.AddAwayMatchScorer(matchScorer1);
            match.AddAwayMatchScorer(matchScorer2);
            match.AddHomeMatchScorer(matchScorer3);

            var matchInjury1 = CreateMatchInjury();
            var matchInjury2 = CreateMatchInjury();
            var matchInjury3 = CreateMatchInjury();

            match.AddHomeTeamMatchInjury(matchInjury1);
            match.AddAwayTeamMatchInjury(matchInjury2);
            match.AddAwayTeamMatchInjury(matchInjury3);

            var matchBooking1 = CreateMatchBooking();
            var matchBooking2 = CreateMatchBooking();
            var matchBooking3 = CreateMatchBooking();

            match.AddHomeTeamMatchBooking(matchBooking1);
            match.AddHomeTeamMatchBooking(matchBooking2);
            match.AddAwayTeamMatchBooking(matchBooking3);

            foreach (var eventId  in eventIdList)
            {
                match.AddMatchEvent(CreateMatchEvent());
            }

            match.MatchArena = CreateMatchArena();

            return match;
        }
    }
}
