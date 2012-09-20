using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using WhoScored.Model;

namespace WhoScored.Db.NHibernate
{
    public class CountrySeriesIdMap : ClassMap<CountrySeriesId>
    {
        public CountrySeriesIdMap()
        {
            Table("country_series_id");
            LazyLoad();
            Id(x => x.SeriesId).GeneratedBy.Identity().Column("series_id");
            Map(x => x.HtSeriesId).Column("ht_series_id").Not.Nullable().Unique();
            References(x => x.Country).Column("country_id").Not.Nullable();
        }
    }
}
