/*
PGSQL Backup
Source Server Version: 9.1.4
Source Database: WhoScored
Date: 19/08/2012 21:20:26
*/


-- ----------------------------
--  Table structure for "public"."countries"
-- ----------------------------
DROP TABLE "public"."countries";
CREATE TABLE "public"."countries" (
"id" int4 DEFAULT nextval('countries_id_seq'::regclass) NOT NULL,
"LeagueName" varchar NOT NULL,
"EnglishName" varchar NOT NULL,
"NumberOfLevels" int2 NOT NULL,
"SeasonOffset" int2 DEFAULT 0 NOT NULL,
"LeagueInWhoScored" bool DEFAULT false NOT NULL,
PRIMARY KEY ("id")
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."match_arena"
-- ----------------------------
DROP TABLE "public"."match_arena";
CREATE TABLE "public"."match_arena" (
"MatchArenaId" int4 DEFAULT nextval('"match_arena_MatchArenaId_seq"'::regclass) NOT NULL,
"ArenaId" int4 NOT NULL,
"ArenaName" varchar NOT NULL,
"WeatherId" int2 NOT NULL,
"SoldTotal" int2,
PRIMARY KEY ("MatchArenaId")
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."match_bookings"
-- ----------------------------
DROP TABLE "public"."match_bookings";
CREATE TABLE "public"."match_bookings" (
"MatchBookingId" int4 DEFAULT nextval('"match_bookings_MatchBookingId_seq"'::regclass) NOT NULL,
"PlayerId" int4 NOT NULL,
"PlayerName" varchar NOT NULL,
"MatchTeamId" int4 NOT NULL,
"MatchId" int4 NOT NULL,
"BookingType" int2 NOT NULL,
"BookingMinute" int2 NOT NULL,
"EventIndex" int2 NOT NULL,
PRIMARY KEY ("MatchBookingId"),
FOREIGN KEY ("MatchTeamId") REFERENCES "public"."match_team" ("MatchTeamId") ON DELETE NO ACTION ON UPDATE NO ACTION,
FOREIGN KEY ("MatchId") REFERENCES "public"."matches" ("MatchId") ON DELETE NO ACTION ON UPDATE NO ACTION
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."match_events"
-- ----------------------------
DROP TABLE "public"."match_events";
CREATE TABLE "public"."match_events" (
"MatchEventId" int8 DEFAULT nextval('"match_events_MatchEventId_seq"'::regclass) NOT NULL,
"MatchId" int4 NOT NULL,
"Minute" int2 NOT NULL,
"SubjectPlayerID" int4 NOT NULL,
"SubjectTeamID" int4 NOT NULL,
"ObjectPlayerID" int4 NOT NULL,
"EventTypeID" int2 NOT NULL,
"EventVariation" int2,
"EventText" varchar NOT NULL,
"EventIndex" int2 NOT NULL,
PRIMARY KEY ("MatchEventId"),
FOREIGN KEY ("MatchId") REFERENCES "public"."matches" ("MatchId") ON DELETE NO ACTION ON UPDATE NO ACTION
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."match_injuries"
-- ----------------------------
DROP TABLE "public"."match_injuries";
CREATE TABLE "public"."match_injuries" (
"MatchInjuryId" int4 DEFAULT nextval('"match_injuries_MatchInjuryId_seq"'::regclass) NOT NULL,
"PlayerId" int4 NOT NULL,
"PlayerName" varchar NOT NULL,
"MatchTeamId" int4 NOT NULL,
"MatchId" int4 NOT NULL,
"InjuryType" int2 NOT NULL,
"InjuryMinute" int2 NOT NULL,
"EventIndex" int2 NOT NULL,
PRIMARY KEY ("MatchInjuryId")
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."match_scorers"
-- ----------------------------
DROP TABLE "public"."match_scorers";
CREATE TABLE "public"."match_scorers" (
"MatchScorerId" int4 DEFAULT nextval('"match_scorers_MatchScoredId_seq"'::regclass) NOT NULL,
"PlayerId" int4 NOT NULL,
"PlayerName" varchar NOT NULL,
"MatchTeamId" int4 NOT NULL,
"TeamGoals" int2 NOT NULL,
"ScorerMinute" int2 NOT NULL,
"MatchId" int4 NOT NULL,
"OppositionGoals" int2 NOT NULL,
"EventIndex" int2 NOT NULL,
PRIMARY KEY ("MatchScorerId"),
FOREIGN KEY ("MatchId") REFERENCES "public"."matches" ("MatchId") ON DELETE NO ACTION ON UPDATE NO ACTION,
FOREIGN KEY ("MatchTeamId") REFERENCES "public"."match_team" ("MatchTeamId") ON DELETE NO ACTION ON UPDATE NO ACTION
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."match_team"
-- ----------------------------
DROP TABLE "public"."match_team";
CREATE TABLE "public"."match_team" (
"MatchTeamId" int4 DEFAULT nextval('"MatchTeam_id_seq"'::regclass) NOT NULL,
"TeamId" int4 NOT NULL,
"Goals" int2 DEFAULT 0,
"DressURI" varchar,
"Formation" varchar,
"TacticType" int2,
"TacticSkill" int2,
"RatingMidfield" int2,
"RatingRightDef" int2,
"RatingMidDef" int2,
"RatingLeftDef" int2,
"RatingRightAtt" int2,
"RatingMidAtt" int2,
"RatingLeftAtt" int2,
"RatingIndirectSetPiecesDef" int2,
"RatingIndirectSetPiecesAtt" int2,
"PossessionFirstHalf" int2,
"PossessionSecondHalf" int2,
PRIMARY KEY ("MatchTeamId"),
FOREIGN KEY ("TeamId") REFERENCES "public"."teams" ("TeamId") ON DELETE NO ACTION ON UPDATE NO ACTION
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."matches"
-- ----------------------------
DROP TABLE "public"."matches";
CREATE TABLE "public"."matches" (
"MatchId" int4 NOT NULL,
"MatchSeason" int2 NOT NULL,
"SeriesId" int4 NOT NULL,
"MatchRound" int2 NOT NULL,
"MatchType" int2 NOT NULL,
"MatchDate" date NOT NULL,
"FinishedDate" date,
"MatchTeamIdHome" int4 NOT NULL,
"MatchTeamIdAway" int4 NOT NULL,
"MatchArenaId" int4,
PRIMARY KEY ("MatchId"),
FOREIGN KEY ("MatchTeamIdAway") REFERENCES "public"."match_team" ("MatchTeamId") ON DELETE NO ACTION ON UPDATE NO ACTION,
FOREIGN KEY ("MatchTeamIdHome") REFERENCES "public"."match_team" ("MatchTeamId") ON DELETE NO ACTION ON UPDATE NO ACTION,
FOREIGN KEY ("MatchArenaId") REFERENCES "public"."match_arena" ("MatchArenaId") ON DELETE NO ACTION ON UPDATE NO ACTION
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."series"
-- ----------------------------
DROP TABLE "public"."series";
CREATE TABLE "public"."series" (
"id" int4 NOT NULL,
"CountryId" int4 NOT NULL,
"LeagueLevel" int2 NOT NULL,
"LeagueLevelUnitName" varchar NOT NULL,
PRIMARY KEY ("id"),
FOREIGN KEY ("CountryId") REFERENCES "public"."countries" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."settings"
-- ----------------------------
DROP TABLE "public"."settings";
CREATE TABLE "public"."settings" (
"GlobalSeason" int4 NOT NULL
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Table structure for "public"."teams"
-- ----------------------------
DROP TABLE "public"."teams";
CREATE TABLE "public"."teams" (
"TeamId" int4 NOT NULL,
"CountryId" int2 NOT NULL,
"TeamName" varchar NOT NULL,
PRIMARY KEY ("TeamId"),
FOREIGN KEY ("CountryId") REFERENCES "public"."countries" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION
)
WITH (OIDS=FALSE)
;;

-- ----------------------------
--  Records 
-- ----------------------------
