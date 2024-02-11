using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Common;

public class AddressSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public string SourceFile { private get; set; } = "philippine-barangays.xlsx";

    public void Seed()
    {
        var batchSize = 500;
        var itemsToImport = AddressExcelImporter.Import(this.SourceFile);

        var (regionImportTime, regionDictionary) = SaveRegion(
            sessionFactory: this._sessionFactory,
            itemsToImport: itemsToImport,
            batchSize: batchSize
        );

        Console.WriteLine("Time it took to import regions: {0}", regionImportTime);

        var (provinceImportTime, provinceDictionary) = SaveProvince(
            sessionFactory: this._sessionFactory,
            itemsToImport: itemsToImport,
            regionDictionary: regionDictionary,
            batchSize: batchSize
        );

        Console.WriteLine("Time it took to import provinces: {0}", provinceImportTime);

        var (cityImportTime, cityDictionary) = SaveCity(
            sessionFactory: this._sessionFactory,
            itemsToImport: itemsToImport,
            provinceDictionary: provinceDictionary,
            batchSize: batchSize
        );

        Console.WriteLine("Time it took to import cities: {0}", cityImportTime);

        var (barangayImportTime, _) = SaveBarangay(
            sessionFactory: this._sessionFactory,
            itemsToImport: itemsToImport,
            cityDictionary: cityDictionary,
            batchSize: batchSize
        );

        Console.WriteLine("Time it took to import barangays: {0}", barangayImportTime);
    }

    private static (TimeSpan, IDictionary<string, Region>) SaveRegion(
        ISessionFactory sessionFactory,
        IEnumerable<AddressExcelImporter.Model> itemsToImport,
        int batchSize)
    {
        var start = DateTime.Now;

        using var session = sessionFactory.OpenStatelessSession();
        using var transaction = session.BeginTransaction();

        session.SetBatchSize(batchSize);

        var regionDictionary = session.Query<Region>()
            .ToDictionary(x => x.Key(), x => x);

        var regionsToImport = itemsToImport
            .GroupBy(x => new { x.Region })
            .Where(x => !regionDictionary.ContainsKey(x.First().RegionKey()))
            .Select(x => x.Key)
            .Distinct();

        foreach (var regionToImport in regionsToImport)
        {
            var region = new Region()
            {
                Name = regionToImport.Region
            };
            regionDictionary.Add(region.Key(), region);
            session.Insert(region);
        }

        transaction.Commit();

        var end = DateTime.Now;
        var elapsed = end - start;

        return (elapsed, regionDictionary);
    }

    private static (TimeSpan, IDictionary<string, Province>) SaveProvince(
        ISessionFactory sessionFactory,
        IEnumerable<AddressExcelImporter.Model> itemsToImport,
        IDictionary<string, Region> regionDictionary,
        int batchSize)
    {
        var start = DateTime.Now;

        using var session = sessionFactory.OpenStatelessSession();
        using var transaction = session.BeginTransaction();

        session.SetBatchSize(batchSize);

        var provinceDictionary = session.Query<Province>()
            .Fetch(x => x.Region)
            .ToDictionary(x => x.Key(), x => x);

        var provincesToImport = itemsToImport
            .GroupBy(x => new { x.Region, x.Province })
            .Where(x => !provinceDictionary.ContainsKey(x.First().ProvinceKey()))
            .Select(x => x.Key)
            .Distinct();

        foreach (var provinceToImport in provincesToImport)
        {
            var province = new Province()
            {
                Name = provinceToImport.Province,
                Region = regionDictionary[$"{provinceToImport.Region}"]
            };
            provinceDictionary.Add(province.Key(), province);
            session.Insert(province);
        }

        transaction.Commit();

        var end = DateTime.Now;
        var elapsed = end - start;

        return (elapsed, provinceDictionary);
    }

    private static (TimeSpan, IDictionary<string, City>) SaveCity(
        ISessionFactory sessionFactory,
        IEnumerable<AddressExcelImporter.Model> itemsToImport,
        IDictionary<string, Province> provinceDictionary,
        int batchSize)
    {
        var start = DateTime.Now;

        using var session = sessionFactory.OpenStatelessSession();
        using var transaction = session.BeginTransaction();

        session.SetBatchSize(batchSize);

        var cityDictionary = session.Query<City>()
            .Fetch(x => x.Province)
            .ThenFetch(x => x.Region)
            .ToDictionary(x => x.Key(), x => x);

        var citiesToImport = itemsToImport
            .GroupBy(x => new { x.Region, x.Province, x.City })
            .Where(x => !cityDictionary.ContainsKey(x.First().CityKey()))
            .Select(x => x.Key)
            .Distinct();

        foreach (var cityToImport in citiesToImport)
        {
            var city = new City()
            {
                Name = cityToImport.City,
                Province = provinceDictionary[$"{cityToImport.Region}{cityToImport.Province}"]
            };
            cityDictionary.Add(city.Key(), city);
            session.Insert(city);
        }

        transaction.Commit();

        var end = DateTime.Now;
        var elapsed = end - start;

        return (elapsed, cityDictionary);
    }

    private static (TimeSpan, IDictionary<string, Barangay>) SaveBarangay(
        ISessionFactory sessionFactory,
        IEnumerable<AddressExcelImporter.Model> itemsToImport,
        IDictionary<string, City> cityDictionary,
        int batchSize)
    {
        var start = DateTime.Now;

        using var session = sessionFactory.OpenStatelessSession();
        using var transaction = session.BeginTransaction();

        session.SetBatchSize(batchSize);

        var barangayDictionary = session.Query<Barangay>()
            .Fetch(x => x.City)
            .ThenFetch(x => x.Province)
            .ThenFetch(x => x.Region)
            .ToList()
            .GroupBy(x => x.Key())
            .ToDictionary(x => x.Key, x => x.First());

        var barangaysToImport = itemsToImport
            .Where(x => !barangayDictionary.ContainsKey(x.BarangayKey()))
            .Distinct();

        foreach (var barangayToImport in barangaysToImport)
        {
            var barangay = new Barangay()
            {
                Name = barangayToImport.Barangay,
                City = cityDictionary[$"{barangayToImport.Region}{barangayToImport.Province}{barangayToImport.City}"],
                AreaClass = barangayToImport.AreaClass.AsNullableEnum<AreaClass>(),
                Population = Convert.ToInt64(barangayToImport.Population)
            };
            barangayDictionary.Add(barangay.Key(), barangay);
            session.Insert(barangay);
        }

        transaction.Commit();

        var end = DateTime.Now;
        var elapsed = end - start;

        return (elapsed, barangayDictionary);
    }

}

public static class AddressExcelImporter
{
    public class Model
    {
        public double Id { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string AreaClass { get; set; }
        public double Population { get; set; }

        internal static Model OfDataRow(DataRow row)
        {
            return new()
            {
                Id = row.Field<double>("id").As<double>(),
                Barangay = row.Field<string>("name").ToUpper(),
                City = row.Field<string>("city").ToUpper(),
                Province = row.Field<string>("province").ToUpper(),
                Region = row.Field<string>("region").ToUpper(),
                AreaClass = row.Field<string>("urban_rural"),
                Population = row.Field<double>("population"),
            };
        }
    }

    public static string RegionKey(this Model value) => $"{value.Region}";

    public static string ProvinceKey(this Model value) => $"{value.Region}{value.Province}";

    public static string CityKey(this Model value) => $"{value.Region}{value.Province}{value.City}";

    public static string BarangayKey(this Model value) => $"{value.Region}{value.Province}{value.City}{value.Barangay}";

    public static List<Model> Import(string sourceFile)
    {
        var list = Importer.ImportExcel(
            path: sourceFile,
            sheet: "philippine-barangays",
            map: Model.OfDataRow
        );

        return list
            .OrderBy(x => x.Region)
            .ThenBy(x => x.Province)
            .ThenBy(x => x.City)
            .ThenBy(x => x.Barangay)
            .ToList();
    }
}

//public class AddressSeeder(ISessionFactory sessionFactory) : ISeeder
//{
//    private readonly ISessionFactory _sessionFactory = sessionFactory;

//    public string SourceFile { private get; set; } = "philippine-barangays.xlsx";

//    public void Seed()
//    {
//        var start = DateTime.Now;
//        var provinces = AddressExcelImporter.Import2(this.SourceFile);

//        using var session = this._sessionFactory.OpenSession();
//        using var transaction = session.BeginTransaction();

//        provinces.ForEach(x => session.Save(x));

//        transaction.Commit();

//        var end = DateTime.Now;
//        var elapsed = end - start;
//        Console.WriteLine("Time it took to import addresses: {0}", elapsed);

//    }

//    public void Seed()
//    {
//        var start = DateTime.Now;

//        var itemsToImport = AddressExcelImporter.Import(this.SourceFile);

//        var batchSize = 100;

//        var regionDictionary = default(Dictionary<string, Region>);

//        using (var session = _sessionFactory.OpenStatelessSession())
//        using (var transaction = session.BeginTransaction())
//        {
//            session.SetBatchSize(batchSize);

//            var regions = session.Query<Region>().ToList();

//            regionDictionary = regions.ToDictionary(x => x.Key(), x => x);

//            var regionsToImport = itemsToImport
//                .GroupBy(x => new { x.Region })
//                .Where(x => !regionDictionary.ContainsKey(x.First().RegionKey()))
//                .Select(x => x.Key)
//                .Distinct();

//            foreach (var regionToImport in regionsToImport)
//            {
//                var region = new Region()
//                {
//                    Name = regionToImport.Region
//                };
//                regionDictionary.Add(region.Key(), region);
//                session.Insert(region);
//            }

//            transaction.Commit();
//        }

//        var provinceDictionary = default(Dictionary<string, Province>);

//        using (var session = _sessionFactory.OpenStatelessSession())
//        using (var transaction = session.BeginTransaction())
//        {
//            session.SetBatchSize(batchSize);

//            var provinces = session.Query<Province>()
//                .Fetch(x => x.Region)
//                .ToList();

//            provinceDictionary = provinces.ToDictionary(x => x.Key(), x => x);

//            var provincesToImport = itemsToImport
//                .GroupBy(x => new { x.Region, x.Province })
//                .Where(x => !provinceDictionary.ContainsKey(x.First().ProvinceKey()))
//                .Select(x => x.Key)
//                .Distinct();

//            foreach (var provinceToImport in provincesToImport)
//            {
//                var province = new Province()
//                {
//                    Name = provinceToImport.Province,
//                    Region = regionDictionary[$"{provinceToImport.Region}"]
//                };
//                provinceDictionary.Add(province.Key(), province);
//                session.Insert(province);
//            }

//            transaction.Commit();
//        }

//        var cityDictionary = default(Dictionary<string, City>);

//        using (var session = _sessionFactory.OpenStatelessSession())
//        using (var transaction = session.BeginTransaction())
//        {
//            session.SetBatchSize(batchSize);

//            var cities = session.Query<City>()
//                .Fetch(x => x.Province)
//                .ThenFetch(x => x.Region)
//                .ToList();

//            cityDictionary = cities.ToDictionary(x => x.Key(), x => x);

//            var citiesToImport = itemsToImport
//                .GroupBy(x => new { x.Region, x.Province, x.City })
//                .Where(x => !cityDictionary.ContainsKey(x.First().CityKey()))
//                .Select(x => x.Key)
//                .Distinct();

//            foreach (var cityToImport in citiesToImport)
//            {
//                var city = new City()
//                {
//                    Name = cityToImport.City,
//                    Province = provinceDictionary[$"{cityToImport.Region}{cityToImport.Province}"]
//                };
//                cityDictionary.Add(city.Key(), city);
//                session.Insert(city);
//            }

//            transaction.Commit();
//        }

//        using (var session = _sessionFactory.OpenStatelessSession())
//        using (var transaction = session.BeginTransaction())
//        {
//            session.SetBatchSize(batchSize);

//            var barangays = session.Query<Barangay>()
//                .Fetch(x => x.City)
//                .ThenFetch(x => x.Province)
//                .ThenFetch(x => x.Region)
//                .ToList();

//            var barangayDictionary = barangays
//                .GroupBy(x => x.Key())
//                .ToDictionary(x => x.Key, x => x.First());

//            var barangaysToImport = itemsToImport
//                .Where(x => !barangayDictionary.ContainsKey(x.BarangayKey()))
//                .Distinct();

//            foreach (var barangayToImport in barangaysToImport)
//            {
//                var barangay = new Barangay()
//                {
//                    Name = barangayToImport.Barangay,
//                    City = cityDictionary[$"{barangayToImport.Region}{barangayToImport.Province}{barangayToImport.City}"],
//                    AreaClass = barangayToImport.AreaClass.AsNullableEnum<AreaClass>(),
//                    Population = Convert.ToInt64(barangayToImport.Population)
//                };
//                session.Insert(barangay);
//            }

//            transaction.Commit();
//        }

//        var end = DateTime.Now;
//        var lapse = end - start;

//        Console.WriteLine("Time it took to import addresses: {0}", lapse);

//        start = DateTime.Now;

//        using (var session = _sessionFactory.OpenStatelessSession())
//        using (var transaction = session.BeginTransaction())
//        {
//            var regions = session.Query<Region>()
//                .FetchMany(x => x.Provinces)
//                .ThenFetchMany(x => x.Cities)
//                .ThenFetchMany(x => x.Barangays)
//                .ToList();

//            transaction.Commit();
//        }

//        end = DateTime.Now;
//        lapse = end - start;

//        Console.WriteLine("Time it took query addresses: {0}", lapse);


//        using (var session = this.SessionFactory.OpenStatelessSession())
//        using (var transaction = session.BeginTransaction())
//        {
//            session.SetBatchSize(batchSize);

//            var regions = session.Query<Region>()
//                .FetchMany(x => x.Provinces)
//                .ThenFetchMany(x => x.Cities)
//                .ThenFetchMany(x => x.Barangays)
//                .ToList();

//            foreach (var item in itemsToImport)
//            {
//                var region = regions.FirstOrDefault(x => IsEqual(x.Name, item.Region));
//                if (region == null)
//                {
//                    region = new Region();
//                    region.Name = item.Region;
//                    //session.Save(region);
//                    session.Insert(region);
//                }

//                var province = region.Provinces.FirstOrDefault(x => IsEqual(x.Name, item.Province));
//                if (province == null)
//                {
//                    province = new Province();
//                    province.Name = item.Province;
//                    region.AddProvice(province);
//                    session.Insert(province);
//                }

//                var city = province.Cities.FirstOrDefault(x => IsEqual(x.Name, item.City));
//                if (city == null)
//                {
//                    city = new City();
//                    city.Name = item.City;
//                    province.AddCity(city);
//                    session.Insert(city);
//                }

//                var barangay = city.Barangays.FirstOrDefault(x => IsEqual(x.Name, item.Barangay));
//                if (barangay == null)
//                {
//                    barangay = new Barangay();
//                    barangay.Name = item.Barangay;
//                    barangay.AreaClass = item.AreaClass.As<AreaClass>();
//                    barangay.Population = item.Population;
//                    city.AddBarangay(barangay);
//                    session.Insert(barangay);
//                }
//            }

//            transaction.Commit();
//        }

//        var batchSize = 1000;
//        var remainer = (itemsToImport.Count % batchSize);
//        var totalBatch = remainer > 0
//            ? (itemsToImport.Count / batchSize) + 1
//            : (itemsToImport.Count / batchSize);

//        for (var batch = 1; batch <= totalBatch; batch++)
//        {
//            var itemsToImportBatch = itemsToImport
//                .Skip(batchSize * batch)
//                .Take(batchSize);

//            using (var session = this.SessionFactory.OpenSession())
//            using (var transaction = session.BeginTransaction())
//            {
//                session.SetBatchSize(1000);

//                var regions = session.Query<Region>()
//                    .FetchMany(x => x.Provinces)
//                    .ThenFetchMany(x => x.Cities)
//                    .ThenFetchMany(x => x.Barangays)
//                    .ToList();

//                foreach (var item in itemsToImportBatch)
//                {
//                    var region = regions.FirstOrDefault(x => IsEqual(x.Name, item.Region));
//                    if (region == null)
//                    {
//                        region = new Region();
//                        region.Name = item.Region;
//                        session.Save(region);
//                        //session.Insert(region);
//                    }

//                    var province = region.Provinces.FirstOrDefault(x => IsEqual(x.Name, item.Province));
//                    if (province == null)
//                    {
//                        province = new Province();
//                        province.Name = item.Province;
//                        region.AddProvice(province);
//                        //session.Insert(province);
//                    }

//                    var city = province.Cities.FirstOrDefault(x => IsEqual(x.Name, item.City));
//                    if (city == null)
//                    {
//                        city = new City();
//                        city.Name = item.City;
//                        province.AddCity(city);
//                        //session.Insert(city);
//                    }

//                    var barangay = city.Barangays.FirstOrDefault(x => IsEqual(x.Name, item.Barangay));
//                    if (barangay == null)
//                    {
//                        barangay = new Barangay();
//                        barangay.Name = item.Barangay;
//                        barangay.AreaClass = item.AreaClass.As<AreaClass>();
//                        barangay.Population = item.Population;
//                        city.AddBarangay(barangay);
//                        //session.Insert(barangay);
//                    }
//                }

//                transaction.Commit();
//            }
//        }
//    }
//}

//public static class AddressExcelImporter
//{
//    public class Model
//    {
//        public double Id { get; set; }
//        public string Barangay { get; set; }
//        public string City { get; set; }
//        public string Province { get; set; }
//        public string Region { get; set; }
//        public string AreaClass { get; set; }
//        public double Population { get; set; }

//    }

//    public static string RegionKey(this Model value) => $"{value.Region}";

//    public static string ProvinceKey(this Model value) => $"{value.Region}{value.Province}";

//    public static string CityKey(this Model value) => $"{value.Region}{value.Province}{value.City}";

//    public static string BarangayKey(this Model value) => $"{value.Region}{value.Province}{value.City}{value.Barangay}";

//    public static List<Model> Import(string sourceFile)
//    {
//        if (!File.Exists(sourceFile))
//            throw new FileNotFoundException("File not found.", sourceFile);

//        var excel = new ExcelQueryFactory(sourceFile);

//        //// Needed to bypass the check for the presence of the BIFF12 format in .NET Core
//        //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

//        using var stream = File.Open(sourceFile, FileMode.Open, FileAccess.Read);
//        using var reader = ExcelReaderFactory.CreateReader(stream);

//        var result = reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = _ => new ExcelDataTableConfiguration() { UseHeaderRow = true } });

//        var table = result.Tables["philippine-barangays"];

//        return table.Rows.Cast<DataRow>()
//            .Select(x => new Model()
//            {
//                Id = x["id"].As<double>(),
//                Barangay = x["name"].As<string>().ToUpper(),
//                City = x["city"].As<string>().ToUpper(),
//                Province = x["province"].As<string>().ToUpper(),
//                Region = x["region"].As<string>().ToUpper(),
//                AreaClass = x["urban_rural"].As<string>(),
//                Population = x["population"].As<double>(),
//            })
//            .OrderBy(x => x.Region)
//            .ThenBy(x => x.Province)
//            .ThenBy(x => x.City)
//            .ThenBy(x => x.Barangay)
//            .ToList();
//    }

//    //public static List<Region> Import2(string sourceFile)
//    //{
//    //    if (!File.Exists(sourceFile))
//    //        throw new FileNotFoundException("File not found.", sourceFile);

//    //    var excel = new ExcelQueryFactory(sourceFile);

//    //    //// Needed to bypass the check for the presence of the BIFF12 format in .NET Core
//    //    //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

//    //    using var stream = File.Open(sourceFile, FileMode.Open, FileAccess.Read);
//    //    using var reader = ExcelReaderFactory.CreateReader(stream);

//    //    var result = reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = _ => new ExcelDataTableConfiguration() { UseHeaderRow = true } });

//    //    var table = result.Tables["philippine-barangays"];

//    //    var dataRow = table.Rows.Cast<DataRow>()
//    //        .Select(x => new Model()
//    //        {
//    //            Id = x["id"].As<double>(),
//    //            Barangay = x["name"].As<string>().ToUpper(),
//    //            City = x["city"].As<string>().ToUpper(),
//    //            Province = x["province"].As<string>().ToUpper(),
//    //            Region = x["region"].As<string>().ToUpper(),
//    //            AreaClass = x["urban_rural"].As<string>(),
//    //            Population = x["population"].As<double>(),
//    //        })
//    //        .OrderBy(x => x.Region)
//    //        .ThenBy(x => x.Province)
//    //        .ThenBy(x => x.City)
//    //        .ThenBy(x => x.Barangay)
//    //        .ToList();

//    //    return dataRow
//    //        .GroupBy(x => new { x.Region })
//    //        .Select(address => new Region()
//    //        {
//    //            Name = address.Key.Region,
//    //            Provinces = dataRow
//    //                .Where(x1 => x1.Region == address.Key.Region)
//    //                .GroupBy(x2 => new { x2.Province })
//    //                .Select(addres2 => new Province()
//    //                {
//    //                    Name = addres2.Key.Province,
//    //                    Cities = dataRow
//    //                        .Where(x2 => 
//    //                            x2.Region == address.Key.Region && 
//    //                            x2.Province == addres2.Key.Province
//    //                        )
//    //                        .GroupBy(x3 => new { x3.City })
//    //                        .Select(address3 => new City()
//    //                        {
//    //                            Name = address3.Key.City,
//    //                            Barangays = address
//    //                                .Where(x4 => 
//    //                                    x4.Region == address.Key.Region && 
//    //                                    x4.Province == addres2.Key.Province && 
//    //                                    x4.City == address3.Key.City
//    //                                )
//    //                                .Select(address4 => new Barangay()
//    //                                {
//    //                                    Name = address4.Barangay,
//    //                                    AreaClass = address4.AreaClass.AsNullableEnum<AreaClass>(),
//    //                                    Population = Convert.ToInt64(address4.Population)
//    //                                })
//    //                                .ToList()
//    //                        })
//    //                        .ToList()
//    //                })
//    //                .ToList()
//    //        })
//    //        .ToList();
//    //}
//}

