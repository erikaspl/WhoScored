using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{

    public class MatchEvent
    {
        public MatchEvent() { }
        public virtual long MatchEventId { get; set; }
        public virtual Match Match { get; set; }
        public virtual short Minute { get; set; }
        public virtual short EventIndex { get; set; }
        public virtual int? ObjectPlayerID { get; set; }
        public virtual int? SubjectPlayerID { get; set; }
        public virtual short EventTypeID { get; set; }
        public virtual string EventText { get; set; }
        public virtual System.Nullable<short> EventVariation { get; set; }
        public virtual int? SubjectTeamID { get; set; }
    }
}
