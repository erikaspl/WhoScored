using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.Model.Mappings
{
    
    
    public class SettingMap : ClassMap<Settings> {

        public SettingMap()
        {
			Table("settings");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("id");
			Map(x => x.GlobalSeason).Column("global_season").Not.Nullable();
        }
    }
}
