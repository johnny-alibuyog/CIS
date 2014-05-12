using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;
using Common = CIS.Core.Entities.Commons;
using Barangays = CIS.Core.Entities.Barangays;
using Firearms = CIS.Core.Entities.Firearms;
using Memberships = CIS.Core.Entities.Memberships;
using Polices = CIS.Core.Entities.Polices;

namespace CIS.Data.Configurations
{
    internal static class CacheConfiguration
    {
        public static void Configure(this NHibernate.Cfg.Configuration config)
        {
            config
                .SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, "true")
                .SetProperty(NHibernate.Cfg.Environment.UseQueryCache, "true")
                .Cache(c => c.Provider<SysCacheProvider>())
                .EntityCache<Common.Configuration>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Common.Configuration";
                })
                .EntityCache<Common.Barangay>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Common.Barangay";
                })
                .EntityCache<Common.City>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Common.City";
                })
                .EntityCache<Common.Province>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Common.Province";
                })
                .EntityCache<Common.Region>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Common.Region";
                })
                .EntityCache<Common.Finger>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Common.Finger";
                })
                .EntityCache<Memberships.User>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Memberships.User";
                })
                .EntityCache<Firearms.Make>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Firearms.Make";
                })
                .EntityCache<Firearms.Kind>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Firearms.Kind";
                })
                .EntityCache<Polices.Purpose>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Polices.Purpose";
                })
                .EntityCache<Polices.Rank>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Polices.Rank";
                })
                .EntityCache<Polices.Officer>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Polices.Officer";
                })
                .EntityCache<Polices.Station>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Polices.Station";
                })
                .EntityCache<Polices.Setting>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Polices.Setting";
                })
                .EntityCache<Barangays.Office>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Barangays.Office";
                })
                .EntityCache<Barangays.Official>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Barangays.Official";
                })
                .EntityCache<Barangays.Purpose>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Barangays.Purpose";
                })
                .EntityCache<Barangays.Position>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Barangays.Position";
                })
                .EntityCache<Barangays.Committee>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Barangays.Committee";
                })
                .EntityCache<Barangays.Setting>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Barangays.Setting";
                });
        }
    }
}
