using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace CIS.Data.Configurations;

internal static class SchemaConfiguration
{
    public static void Configure(this Configuration config)
    {
        /// WARNING: this line will recreate all your 
        ///     database object removing all the data,
        ///     not to be used in production
        //new SchemaExport(config).Create(true, true);

        /// NOTE: this line will update your database
        ///     schema based on the changes you made
        ///     on your Entities(business models) if
        ///     there is any
        new SchemaUpdate(config).Execute(true, true);
    }
}
