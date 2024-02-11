using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;
using CommonEntities = CIS.Core.Domain.Common;
using SecurityEntities = CIS.Core.Domain.Security;
using MembershipEntities = CIS.Core.Domain.Membership;

namespace CIS.Data.Configurations;

internal static class CacheConfiguration
{
    public static void Configure(this Configuration config)
    {
        config
            .SetProperty(Environment.UseSecondLevelCache, "true")
            .SetProperty(Environment.UseQueryCache, "true")
            .Cache(c => c.Provider<SysCacheProvider>())
            .EntityCache<CommonEntities.Configuration>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Common.Configuration";
            })
            .EntityCache<CommonEntities.Barangay>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Common.Barangay";
            })
            .EntityCache<CommonEntities.City>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Common.City";
            })
            .EntityCache<CommonEntities.Province>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Common.Province";
            })
            .EntityCache<CommonEntities.Region>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Common.Region";
            })
            .EntityCache<CommonEntities.Finger>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Common.Finger";
            })
            .EntityCache<SecurityEntities.User>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Security.User";
            })
            .EntityCache<MembershipEntities.Purpose>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Membership.Purpose";
            })
            .EntityCache<MembershipEntities.Rank>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Membership.Rank";
            })
            .EntityCache<MembershipEntities.Officer>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Membership.Officer";
            })
            .EntityCache<MembershipEntities.Station>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Membership.Station";
            })
            .EntityCache<MembershipEntities.Setting>(x =>
            {
                x.Strategy = EntityCacheUsage.ReadWrite;
                x.RegionName = "Membership.Setting";
            });
    }
}
