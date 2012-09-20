using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings 
{
    
    
    public class MatchArenaMap : ClassMap<MatchArena> {

        public MatchArenaMap()
        {
			Table("match_arena");
			LazyLoad();
            Id(x => x.MatchArenaId).GeneratedBy.Identity().Column("match_arena_id");
			Map(x => x.ArenaName).Column("arena_name").Not.Nullable();
			Map(x => x.WeatherId).Column("weather_id").Not.Nullable();
			Map(x => x.SoldTotal).Column("sold_total");
			Map(x => x.HtArenaId).Column("arena_id").Not.Nullable();
            //HasMany(x => x.Matches).Cascade.SaveUpdate();
        }
    }
}
