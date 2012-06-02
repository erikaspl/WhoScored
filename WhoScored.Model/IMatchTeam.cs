namespace WhoScored.Model
{
    public interface IMatchTeam
    {
        string TeamID { get; set; }

        string TeamName { get; set; }

        string DressURI { get; set; }

        string Formation { get; set; }

        string Goals { get; set; }

        string TacticType { get; set; }

        string TacticSkill { get; set; }

        string RatingMidfield { get; set; }

        string RatingRightDef { get; set; }

        string RatingMidDef { get; set; }

        string RatingLeftDef { get; set; }

        string RatingRightAtt { get; set; }

        string RatingMidAtt { get; set; }

        string RatingLeftAtt { get; set; }

        string RatingIndirectSetPiecesDef { get; set; }

        string RatingIndirectSetPiecesAtt { get; set; }
    }
}