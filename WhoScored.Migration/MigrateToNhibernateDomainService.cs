using System;
using System.Collections.Generic;
using System.Linq;
using WhoScored.CHPP.MatchDetails.Serializer;
using WhoScored.Db.Postgres.Repositories;

namespace WhoScored.Migration
{
    using Db.Postgres;
    using Model;

    using NHibernate;

    public class MigrateToNhibernateDomainService : MigrationDomainServiceBase
    {
        protected readonly ISession Session;
        public MigrateToNhibernateDomainService(ISession session)
        {
            Session = session;
        }


        public override void MigrateWorldDetails()
        {
            var repository = new CountryRepository(Session);
            var wsCountries = repository.GetAll().ToList();

            var countryList = ReadWorldDetails();

            foreach (var htCountry in countryList)
            {
                var country = wsCountries.FirstOrDefault(c => c.HtCountryId == htCountry.LeagueID);
                if (country == null)
                {
                    country = new Country { IsNewValue = true };
                    wsCountries.Add(country);
                }
                country.HtCountryId = htCountry.LeagueID;
                country.CountryName = htCountry.LeagueName;
                country.EnglishName = htCountry.EnglishName;
                country.SeasonOffset = htCountry.SeasonOffset;
                country.NumberOfLevels = htCountry.NumberOfLevels;
                country.CountryInWhoScored = htCountry.LeagueInWhoScored;
                country.SeriesMatchTime = DateTime.Parse(htCountry.SeriesMatchDate).TimeOfDay;
                country.SeriesMatchWeekDay = (short)DateTime.Parse(htCountry.SeriesMatchDate).DayOfWeek;
                if (htCountry.SeriesIdList != null)
                    country.AddSeriesIdRange(htCountry.SeriesIdList);
            }

            repository.SaveUpdateCountries(wsCountries);
        }

        public override void MigrateLeagueDetails(int countryId)
        {
            var countryRepository = new CountryRepository(Session);
            var country = countryRepository.GetCountryByHtId(countryId);

            foreach (var series in country.SupportedSeriesId)
            {
                if (country.Series.FirstOrDefault(s => s.HtSeriesId == series.HtSeriesId) == null)
                {
                    var htseries = ReadSeriesDetails(series.HtSeriesId);
                    country.AddSeries(new Series
                                          {
                                              HtSeriesId = htseries.LeagueLevelUnitID,
                                              LeagueLevel = (short)htseries.LeagueLevel,
                                              LeagueLevelUnitName = htseries.LeagueLevelUnitName
                                          });
                }
            }
            countryRepository.SaveUpdate(country);
            Session.Flush();
        }

        public override void MigrateFixtures(int htSeriesId, int season)
        {
            var seriesFixturesResult = GetSeriesFixtures(season, htSeriesId);
            Session.Clear();
            var repository = new SeriesRepository(Session);
            var series = repository.GetAll().First(s => s.HtSeriesId == htSeriesId);
            SetFixtures(series, season, seriesFixturesResult);
            using (var transaction = Session.BeginTransaction())
            {
                repository.SaveUpdate(series);
                transaction.Commit();
            }
        }

        public override void MigrateMatchDetails(int matchId, int matchRound, int season, int htSeriesId)
        {
            var matchDetails = GetMatchDetails(matchId);
            Session.Clear();
            var repository = new SeriesRepository(Session);
            var series = repository.GetAll().First(s => s.HtSeriesId == htSeriesId);

            var match = series.Matches.FirstOrDefault(m => m.MatchId == matchId);
            var htMatch = matchDetails.Match.First();
            DateTime matchFinishDate;
            if (DateTime.TryParse(htMatch.FinishedDate, out matchFinishDate))
            {
                var htAwayTeam = htMatch.AwayTeam.First();
                var htHomeTeam = htMatch.HomeTeam.First();
                if (match == null)
                {
                    match = new Match
                    {
                        HtMatchId = int.Parse(htMatch.MatchID),
                        MatchDate = DateTime.Parse(htMatch.MatchDate),
                        FinishedDate = DateTime.Parse(htMatch.FinishedDate),
                        MatchArena = new MatchArena
                        {
                            ArenaName = htMatch.MatchArena.ArenaName,
                            HtArenaId = int.Parse(htMatch.MatchArena.ArenaID),
                            WeatherId = short.Parse(htMatch.MatchArena.WeatherID)
                        },
                        MatchType = short.Parse(htMatch.MatchType),
                        MatchSeason = (short)season,
                        MatchRound = (short)matchRound
                    };
                    match.SetMatchHomeTeam(GetMatchTeam(series.Country, htHomeTeam, htMatch.PossessionFirstHalfHome,
                                                        htMatch.PossessionSecondHalfHome));
                    match.SetMatchAwayTeam(GetMatchTeam(series.Country, htAwayTeam, htMatch.PossessionFirstHalfAway,
                                                        htMatch.PossessionSecondHalfAway));
                    ProcessScorers(match, htMatch);
                    ProcessInjuries(match, htMatch);
                    ProcessBookings(match, htMatch);
                    ProcessEvents(match, htMatch);
                    series.AddMatch(match);
                }
            }
            repository.SaveUpdate(series);
            Session.Flush();
        }

        #region Private helpers

        private static void SetFixtures(Series series, int season, IEnumerable<SeriesFixturesSummaryEntity> seriesFixturesResult)
        {
            var teams = new List<Team>();
            var seriesFixture = series.SeriesFixtures;
            foreach (var match in seriesFixturesResult.First(s => s.LeagueLevelUnitID == series.HtSeriesId).Matches)
            {
                var fixture = seriesFixture.FirstOrDefault(f => f.HtMatchId == match.MatchID);
                if (fixture == null)
                {
                    fixture = new SeriesFixture();
                    var homeTeam = teams.FirstOrDefault(t => t.TeamId == match.HomeTeamID);
                    if (homeTeam == null)
                    {
                        homeTeam = new Team
                                       {
                                           Country = series.Country,
                                           TeamId = match.HomeTeamID,
                                           TeamName = match.HomeTeamName
                                       };
                        teams.Add(homeTeam);
                    }

                    var awayTeam = teams.FirstOrDefault(t => t.TeamId == match.AwayTeamID);
                    if (awayTeam == null)
                    {
                        awayTeam = new Team
                                       {
                                           Country = series.Country,
                                           TeamId = match.AwayTeamID,
                                           TeamName = match.AwayTeamName
                                       };
                        teams.Add(awayTeam);
                    }
                    fixture.HtMatchId = match.MatchID;
                    fixture.AwayTeam = awayTeam;
                    fixture.HomeTeam = homeTeam;
                    fixture.MatchDate = match.MatchDate;
                    fixture.MatchRound = match.MatchRound;
                    fixture.Season = (short)season;
                    series.AddSeriesFixture(fixture);
                }
                fixture.AwayGoals = (short?)match.AwayGoals;
                fixture.HomeGoals = (short?)match.HomeGoals;
            }
        }

        private static void ProcessEvents(Match match, HattrickDataMatch htMatch)
        {
            foreach (var htMatchEvent in htMatch.EventList)
            {
                MatchEvent matchEvent = new MatchEvent();
                matchEvent.EventIndex = short.Parse(htMatchEvent.Index);
                matchEvent.EventText = htMatchEvent.EventText;
                matchEvent.EventTypeID = short.Parse(htMatchEvent.EventTypeID);
                matchEvent.EventVariation = short.Parse(htMatchEvent.EventVariation);
                matchEvent.Minute = short.Parse(htMatchEvent.Minute);
                matchEvent.ObjectPlayerID = string.IsNullOrEmpty(htMatchEvent.ObjectPlayerID) ? (int?)null : int.Parse(htMatchEvent.ObjectPlayerID);
                matchEvent.SubjectPlayerID = string.IsNullOrEmpty(htMatchEvent.SubjectPlayerID) ? (int?)null : int.Parse(htMatchEvent.SubjectPlayerID);
                matchEvent.SubjectTeamID = string.IsNullOrEmpty(htMatchEvent.SubjectTeamID) ? (int?)null : int.Parse(htMatchEvent.SubjectTeamID);
                match.AddMatchEvent(matchEvent);
            }
        }

        private static void ProcessBookings(Match match, HattrickDataMatch htMatch)
        {
            foreach (var htBooking in htMatch.Bookings)
            {
                int bookingTeamId = int.Parse(htBooking.BookingTeamID);
                var booking = new MatchBooking
                                  {
                                      PlayerId = int.Parse(htBooking.BookingPlayerID),
                                      PlayerName = htBooking.BookingPlayerName,
                                      BookingMinute = short.Parse(htBooking.BookingMinute),
                                      BookingType = short.Parse(htBooking.BookingType)
                                  };
                if (match.MatchHomeTeam.Team.TeamId == bookingTeamId)
                    match.AddHomeTeamMatchBooking(booking);
                else if (match.MatchAwayTeam.Team.TeamId == bookingTeamId)
                    match.AddAwayTeamMatchBooking(booking);
            }
        }

        private static void ProcessInjuries(Match match, HattrickDataMatch htMatch)
        {
            foreach (var htInjury in htMatch.MatchInjuries)
            {
                int injuryTeamId = int.Parse(htInjury.InjuryTeamID);
                var injury = new MatchInjury
                                 {
                                     PlayerId = int.Parse(htInjury.InjuryPlayerID),
                                     PlayerName = htInjury.InjuryPlayerName,
                                     InjuryMinute = short.Parse(htInjury.InjuryMinute),
                                     InjuryType = short.Parse(htInjury.InjuryType),
                                 };
                if (match.MatchHomeTeam.Team.TeamId == injuryTeamId)
                    match.AddHomeTeamMatchInjury(injury);
                else if (match.MatchAwayTeam.Team.TeamId == injuryTeamId)
                    match.AddAwayTeamMatchInjury(injury);
            }
        }

        private static void ProcessScorers(Match match, HattrickDataMatch htMatch)
        {
            foreach (var matchScorer in htMatch.MatchScorers)
            {
                int scorerTeamId = int.Parse(matchScorer.ScorerTeamID);
                bool isHomeScorer = match.MatchHomeTeam.Team.TeamId == scorerTeamId;
                var scorer = new MatchScorer
                                 {
                                     OppositionGoals =
                                         isHomeScorer
                                             ? short.Parse(matchScorer.ScorerAwayGoals)
                                             : short.Parse(matchScorer.ScorerHomeGoals),
                                     TeamGoals =
                                         isHomeScorer
                                             ? short.Parse(matchScorer.ScorerHomeGoals)
                                             : short.Parse(matchScorer.ScorerAwayGoals),
                                     PlayerId = int.Parse(matchScorer.ScorerPlayerID),
                                     PlayerName = matchScorer.ScorerPlayerName,
                                     ScorerMinute = short.Parse(matchScorer.ScorerMinute)
                                 };

                if (isHomeScorer)
                    match.AddHomeMatchScorer(scorer);
                else
                    match.AddAwayMatchScorer(scorer);
            }
        }

        private MatchTeam GetMatchTeam(Country country, IMatchTeam htAwayTeam, string possessionFirstHalf, string possessionSecondHalf)
        {
            return new MatchTeam
                       {
                           Team = new Team
                                      {
                                          Country = country,
                                          TeamId = int.Parse(htAwayTeam.TeamID),
                                          TeamName = htAwayTeam.TeamName
                                      },
                           DressURI = htAwayTeam.DressURI,
                           Formation = htAwayTeam.Formation,
                           Goals = short.Parse(htAwayTeam.Goals),

                           RatingIndirectSetPiecesAtt = string.IsNullOrEmpty(htAwayTeam.RatingIndirectSetPiecesAtt) ? (short?)null
                                            : short.Parse(htAwayTeam.RatingIndirectSetPiecesAtt),
                           RatingIndirectSetPiecesDef = string.IsNullOrEmpty(htAwayTeam.RatingIndirectSetPiecesDef) ? (short?)null
                                            : short.Parse(htAwayTeam.RatingIndirectSetPiecesDef),

                           RatingLeftAtt = short.Parse(htAwayTeam.RatingLeftAtt),
                           RatingLeftDef = short.Parse(htAwayTeam.RatingLeftDef),
                           RatingMidAtt = short.Parse(htAwayTeam.RatingMidAtt),
                           RatingMidDef = short.Parse(htAwayTeam.RatingMidDef),
                           RatingMidfield = short.Parse(htAwayTeam.RatingMidfield),
                           RatingRightAtt = short.Parse(htAwayTeam.RatingRightAtt),
                           RatingRightDef = short.Parse(htAwayTeam.RatingRightDef),
                           TacticSkill = short.Parse(htAwayTeam.TacticSkill),
                           TacticType = short.Parse(htAwayTeam.TacticType),
                           PossessionFirstHalf = short.Parse(possessionFirstHalf),
                           PossessionSecondHalf = short.Parse(possessionSecondHalf)
                       };
        }

        #endregion
    }
}
