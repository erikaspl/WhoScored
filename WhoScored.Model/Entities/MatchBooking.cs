using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class MatchBooking {
        public MatchBooking() { }
        public virtual int MatchBookingId { get; set; }
        public virtual Match Match { get; set; }
        public virtual MatchTeam MatchTeam { get; set; }
        public virtual int PlayerId { get; set; }
        public virtual short EventIndex { get; set; }
        public virtual short BookingMinute { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual short BookingType { get; set; }
    }
}
