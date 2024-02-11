using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelDataReader;
using NHibernate.Util;

namespace CIS.Data.Definition;

public static class Importer
{
    public static List<TModel> ImportExcel<TModel>(string path,  Func<DataRow, TModel> map, string sheet = null)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("File not found.", path);

        //// Needed to bypass the check for the presence of the BIFF12 format in .NET Core
        //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var result = reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = _ => new ExcelDataTableConfiguration() { UseHeaderRow = true } });

        var table = string.IsNullOrWhiteSpace(sheet)  
            ? result.Tables.First() as DataTable
            : result.Tables[sheet];

        return table.Rows.Cast<DataRow>().Select(map).ToList();
    }
}
