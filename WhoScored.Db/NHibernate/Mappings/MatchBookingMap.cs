using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class MatchBookingMap : ClassMap<MatchBooking> {

        public MatchBookingMap()
        {
			Table("match_bookings");
			LazyLoad();
			Id(x => x.MatchBookingId).GeneratedBy.Identity().Column("match_booking_id");
			References(x => x.Match).Column("match_id");
			References(x => x.MatchTeam).Column("match_team_id");
			Map(x => x.PlayerId).Column("player_id").Not.Nullable();
			Map(x => x.EventIndex).Column("event_index").Not.Nullable();
			Map(x => x.BookingMinute).Column("booking_minute").Not.Nullable();
			Map(x => x.PlayerName).Column("player_name").Not.Nullable();
			Map(x => x.BookingType).Column("booking_type").Not.Nullable();
        }
    }
}
