using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features.Common.Address;

[HandleError]
public class AddressController : ControllerBase<AddressViewModel>
{
    public AddressController(AddressViewModel viewModel)
        : base(viewModel)
    {
        this.PopulateProvinces();

        this.ViewModel.ObservableForProperty(x => x.SelectedProvince)
            .Subscribe(x => PopulateCities(x.Value));

        this.ViewModel.ObservableForProperty(x => x.SelectedCity)
            .Subscribe(x => PopulateBarangays(x.Value));
    }

    public virtual void PopulateProvinces()
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
            .Select(x => new Lookup<Guid>(x.Id, x.Name))
            .OrderBy(x => x.Name)
            .ToReactiveList();
    }

    public virtual void PopulateBarangays(Lookup<Guid> cityLookup)
    {
        this.ViewModel.Barangay = null;

        if (cityLookup == null)
            return;

        var barangays = default(IList<Barangay>);

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
            .Select(x => new Lookup<Guid>(x.Id, x.Name))
            .OrderBy(x => x.Name)
            .ToReactiveList();
    }
}
