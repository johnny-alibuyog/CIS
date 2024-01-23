using NHibernate.Cfg;
using NHibernate.Context;

namespace CIS.Data.Configurations;

public static class SessionContextConfiguration
{
    public static void Configure(this Configuration config)
    {
        //config.SetProperty(Environment.CurrentSessionContextClass, "thread_static");
        var context = typeof(ThreadStaticSessionContext).AssemblyQualifiedName;
        config.SetProperty(Environment.CurrentSessionContextClass, context);
    }
}
