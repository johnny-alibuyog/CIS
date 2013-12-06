using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;
using CIS.Core.Entities.Memberships;
using CIS.Core.Entities.Polices;
using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;

namespace CIS.Data.Configurations
{
    internal static class CacheConfiguration
    {
        public static void Configure(this Configuration config)
        {
            config
                .SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, "true")
                .SetProperty(NHibernate.Cfg.Environment.UseQueryCache, "true")
                .Cache(c => c.Provider<SysCacheProvider>())
                .EntityCache<Barangay>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Barangay";
                })
                .EntityCache<City>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "City";
                })
                .EntityCache<Province>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Province";
                })
                .EntityCache<Region>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Region";
                })
                //.EntityCache<Role>(x => 
                //{
                //    x.Strategy = EntityCacheUsage.Readonly;
                //    x.RegionName = "Role";
                //})
                .EntityCache<User>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "User";
                })
                .EntityCache<Make>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Make";
                })
                .EntityCache<Kind>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Kind";
                })
                .EntityCache<Purpose>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Purpose";
                })
                .EntityCache<Rank>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Rank";
                })
                .EntityCache<Officer>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Officer";
                })
                .EntityCache<Station>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Station";
                })
                .EntityCache<Setting>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Setting";
                })
                .EntityCache<Finger>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Finger";
                });
        }
    }
}
