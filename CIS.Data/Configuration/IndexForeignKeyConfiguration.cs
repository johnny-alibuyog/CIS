using NHibernate.Cfg;
using NHibernate.Mapping;
using System.Collections.Generic;
using System.Reflection;

namespace CIS.Data.Configurations;

internal static class IndexForeignKeyConfiguration
{
    private static readonly PropertyInfo TableMappingsProperty = typeof(Configuration)
             .GetProperty("TableMappings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    public static void Configure(Configuration config)
    {
        //FIXME: Make this object query
        //var createIndex = (ForeignKey fk) =>
        //{
        //    var index = new Index();
        //    index.AddColumns(fk.ColumnIterator);
        //    index.Name = "IDX_" + fk.Name;
        //    index.Table = fk.Table;
        //    fk.Table.AddIndex(index);
        //};

        var tables = (ICollection<Table>)TableMappingsProperty.GetValue(config, null);

        foreach (var table in tables)
        {
            foreach (var foreignKey in table.ForeignKeyIterator)
            {
                var index = new Index();
                index.AddColumns(foreignKey.ColumnIterator);
                index.Name = "IDX_" + foreignKey.Name;
                index.Table = table;
                table.AddIndex(index);
            }
        }
    }
}
