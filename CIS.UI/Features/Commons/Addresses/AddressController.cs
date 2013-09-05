using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using CIS.UI.Utilities.Extentions;
using LinqToExcel;
using LinqToExcel.Domain;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using ReactiveUI;

namespace CIS.UI.Features.Commons.Addresses
{
    public class AddressController : ControllerBase<AddressViewModel>
    {
        public AddressController(AddressViewModel viewModel) : base(viewModel)
        {
            //Import();
            PopulateProvinces();

            this.ViewModel.ObservableForProperty(x => x.SelectedProvince)
                .Subscribe(x => PopulateCities(x.Value));

            this.ViewModel.ObservableForProperty(x => x.SelectedCity)
                .Subscribe(x => PopulateBarangays(x.Value));
        }

        private void PopulateProvinces()
        {
            this.ViewModel.Provinces = null;
            this.ViewModel.Cities = null;
            this.ViewModel.Barangays = null;

            var provinces = (IList<Province>)null;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                provinces = session.Query<Province>().Cacheable().ToList();

                transaction.Commit();
            }

            this.ViewModel.Provinces = provinces
                .Select(x => new Lookup<Guid>()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToReactiveList();
        }

        public virtual void PopulateCities(Lookup<Guid> provinceLookup)
        {
            this.ViewModel.Cities = null;
            this.ViewModel.Barangays = null;

            if (provinceLookup == null)
                return;

            var cities = (IList<City>)null;
            
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                cities = session.Query<City>()
                    .Where(x => x.Province.Id == provinceLookup.Id)
                    .Cacheable()
                    .ToList();

                transaction.Commit();
            }

            this.ViewModel.Cities = cities
                .Select(x => new Lookup<Guid>()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToReactiveList();
        }

        public virtual void PopulateBarangays(Lookup<Guid> cityLookup)
        {
            this.ViewModel.Barangay = null;

            if (cityLookup == null)
                return;

            var barangays = (IList<Barangay>)null;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                barangays = session.Query<Barangay>()
                    .Where(x => x.City.Id == cityLookup.Id)
                    .Cacheable()
                    .ToList();

                transaction.Commit();
            }

            this.ViewModel.Barangays = barangays
                .Select(x => new Lookup<Guid>()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToReactiveList();
        }

        public virtual void Import()
        {
            Func<string, string, bool> IsEqual = (item1, item2) => string.Compare(item1, item2, true) == 0;

            var start = DateTime.Now;

            var excel = new ExcelQueryFactory("philippine-barangays.xlsx");
            var itemsToImport = excel.Worksheet("philippine-barangays")
                .Select(x => new
                {
                    Id = x["id"].Cast<long>(),
                    Barangay = x["name"].Cast<string>().ToProperCase(),
                    City = x["city"].Cast<string>().ToProperCase(),
                    Province = x["province"].Cast<string>().ToProperCase(),
                    Region = x["region"].Cast<string>().ToProperCase(),
                    AreaClass = x["urban_rural"].Cast<string>(),
                    Population = x["population"].Cast<int>(),
                })
                .ToList();

            var batchSize = 1000;

            var regions = new List<Region>();
            
            using (var session = this.SessionFactory.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                regions = session.Query<Region>().ToList();

                var regionsToImport = itemsToImport
                    .GroupBy(x => new { RegionName = x.Region })
                    .Distinct();

                foreach (var regionToImport in regionsToImport)
                {
                    var region = regions.FirstOrDefault(x => IsEqual(x.Name, regionToImport.Key.RegionName));
                    if (region == null)
                    {
                        region = new Region();
                        region.Name = regionToImport.Key.RegionName;
                        regions.Add(region);
                        session.Insert(region);
                    }
                }

                transaction.Commit();
            }

            var provinces = new List<Province>();

            using (var session = this.SessionFactory.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                provinces = session.Query<Province>()
                    .Fetch(x => x.Region)
                    .ToList();

                var provincesToImport = itemsToImport
                    .GroupBy(x => new 
                    {
                        Region = x.Region,
                        Province = x.Province,
                    })
                    .Distinct();

                foreach (var provinceToImport in provincesToImport)
                {
                    var province = provinces
                        .FirstOrDefault(x => 
                            IsEqual(x.Name, provinceToImport.Key.Province) &&
                            IsEqual(x.Region.Name, provinceToImport.Key.Region)
                        );

                    if (province == null)
                    {
                        province = new Province();
                        province.Name = provinceToImport.Key.Province;
                        province.Region = regions.FirstOrDefault(x => x.Name == provinceToImport.Key.Region);
                        provinces.Add(province);
                        session.Insert(province);
                    }
                }

                transaction.Commit();
            }

            var cities = new List<City>();

            using (var session = this.SessionFactory.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                cities = session.Query<City>()
                    .Fetch(x => x.Province)
                    .ThenFetch(x => x.Region)
                    .ToList();

                var citiesToImport = itemsToImport
                    .GroupBy(x => new
                    {
                        Region = x.Region,
                        Province = x.Province,
                        City = x.City
                    })
                    .Distinct();

                foreach (var cityToImport in citiesToImport)
                {
                    var city = cities
                        .FirstOrDefault(x =>
                            IsEqual(x.Name, cityToImport.Key.City) &&
                            IsEqual(x.Province.Name, cityToImport.Key.Province) &&
                            IsEqual(x.Province.Region.Name, cityToImport.Key.Region)
                        );

                    if (city == null)
                    {
                        city = new City();
                        city.Name = cityToImport.Key.City;
                        city.Province = provinces.FirstOrDefault(x => 
                            x.Name == cityToImport.Key.Province &&
                            x.Region.Name == cityToImport.Key.Region
                        );
                        cities.Add(city);
                        session.Insert(city);
                    }
                }

                transaction.Commit();
            }

            var barangays = new List<Barangay>();

            using (var session = this.SessionFactory.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                barangays = session.Query<Barangay>()
                    .Fetch(x => x.City)
                    .ThenFetch(x => x.Province)
                    .ThenFetch(x => x.Region)
                    .ToList();

                var barangaysToImport = itemsToImport;
                    //.GroupBy(x => new
                    //{
                    //    Region = x.Region,
                    //    Province = x.Province,
                    //    City = x.City,
                    //    Barangay = x.Barangay,
                    //})
                    //.Distinct();

                foreach (var barangayToImport in barangaysToImport)
                {
                    var barangay = barangays
                        .FirstOrDefault(x =>
                            IsEqual(x.Name, barangayToImport.Barangay) &&
                            IsEqual(x.City.Name, barangayToImport.City) &&
                            IsEqual(x.City.Province.Name, barangayToImport.Province) &&
                            IsEqual(x.City.Province.Region.Name, barangayToImport.Region)
                        );

                    if (barangay == null)
                    {
                        barangay = new Barangay();
                        barangay.Name = barangayToImport.Barangay;
                        barangay.City = cities.FirstOrDefault(x => 
                            x.Name == barangayToImport.City &&
                            x.Province.Name == barangayToImport.Province &&
                            x.Province.Region.Name == barangayToImport.Region
                        );
                        barangay.AreaClass = barangayToImport.AreaClass.As<AreaClass>();
                        barangay.Population = barangayToImport.Population;
                        barangays.Add(barangay);
                        session.Insert(barangay);
                    }
                }

                transaction.Commit();
            }

            var end = DateTime.Now;
            var lapse = end - start;

            Console.WriteLine("Time it took to import addresses: {0}", lapse);

            start = DateTime.Now;

            using (var session = this.SessionFactory.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                regions = session.Query<Region>()
                    .FetchMany(x => x.Provinces)
                    .ThenFetchMany(x => x.Cities)
                    .ThenFetchMany(x => x.Barangays)
                    .ToList();

                transaction.Commit();
            }

            end = DateTime.Now;
            lapse = end - start;

            Console.WriteLine("Time it took query addresses: {0}", lapse);


            //using (var session = this.SessionFactory.OpenStatelessSession())
            //using (var transaction = session.BeginTransaction())
            //{
            //    session.SetBatchSize(batchSize);

            //    var regions = session.Query<Region>()
            //        .FetchMany(x => x.Provinces)
            //        .ThenFetchMany(x => x.Cities)
            //        .ThenFetchMany(x => x.Barangays)
            //        .ToList();

            //    foreach (var item in itemsToImport)
            //    {
            //        var region = regions.FirstOrDefault(x => IsEqual(x.Name, item.Region));
            //        if (region == null)
            //        {
            //            region = new Region();
            //            region.Name = item.Region;
            //            //session.Save(region);
            //            session.Insert(region);
            //        }

            //        var province = region.Provinces.FirstOrDefault(x => IsEqual(x.Name, item.Province));
            //        if (province == null)
            //        {
            //            province = new Province();
            //            province.Name = item.Province;
            //            region.AddProvice(province);
            //            session.Insert(province);
            //        }

            //        var city = province.Cities.FirstOrDefault(x => IsEqual(x.Name, item.City));
            //        if (city == null)
            //        {
            //            city = new City();
            //            city.Name = item.City;
            //            province.AddCity(city);
            //            session.Insert(city);
            //        }

            //        var barangay = city.Barangays.FirstOrDefault(x => IsEqual(x.Name, item.Barangay));
            //        if (barangay == null)
            //        {
            //            barangay = new Barangay();
            //            barangay.Name = item.Barangay;
            //            barangay.AreaClass = item.AreaClass.As<AreaClass>();
            //            barangay.Population = item.Population;
            //            city.AddBarangay(barangay);
            //            session.Insert(barangay);
            //        }
            //    }

            //    transaction.Commit();
            //}



            //var batchSize = 1000;
            //var remainer = (itemsToImport.Count % batchSize);
            //var totalBatch = remainer > 0
            //    ? (itemsToImport.Count / batchSize) + 1
            //    : (itemsToImport.Count / batchSize);

            //for (var batch = 1; batch <= totalBatch; batch++)
            //{
            //    var itemsToImportBatch = itemsToImport
            //        .Skip(batchSize * batch)
            //        .Take(batchSize);

            //    using (var session = this.SessionFactory.OpenSession())
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        session.SetBatchSize(1000);

            //        var regions = session.Query<Region>()
            //            .FetchMany(x => x.Provinces)
            //            .ThenFetchMany(x => x.Cities)
            //            .ThenFetchMany(x => x.Barangays)
            //            .ToList();

            //        foreach (var item in itemsToImportBatch)
            //        {
            //            var region = regions.FirstOrDefault(x => IsEqual(x.Name, item.Region));
            //            if (region == null)
            //            {
            //                region = new Region();
            //                region.Name = item.Region;
            //                session.Save(region);
            //                //session.Insert(region);
            //            }

            //            var province = region.Provinces.FirstOrDefault(x => IsEqual(x.Name, item.Province));
            //            if (province == null)
            //            {
            //                province = new Province();
            //                province.Name = item.Province;
            //                region.AddProvice(province);
            //                //session.Insert(province);
            //            }

            //            var city = province.Cities.FirstOrDefault(x => IsEqual(x.Name, item.City));
            //            if (city == null)
            //            {
            //                city = new City();
            //                city.Name = item.City;
            //                province.AddCity(city);
            //                //session.Insert(city);
            //            }

            //            var barangay = city.Barangays.FirstOrDefault(x => IsEqual(x.Name, item.Barangay));
            //            if (barangay == null)
            //            {
            //                barangay = new Barangay();
            //                barangay.Name = item.Barangay;
            //                barangay.AreaClass = item.AreaClass.As<AreaClass>();
            //                barangay.Population = item.Population;
            //                city.AddBarangay(barangay);
            //                //session.Insert(barangay);
            //            }
            //        }

            //        transaction.Commit();
            //    }
            //}



            ////.GroupBy(
            ////    x => x.Region,
            ////    x => new 

            ////.GroupBy(x => new
            ////{
            ////    Region = x.Region,
            ////    Province = x.Province,
            ////    City = x.City,
            ////})
            ////.Select(x => new 
            ////{
            ////    Region = x.Region,
            ////    Province = x.Province,
            ////    City = x.City,
            ////});

        }
    }
}
