using System;
using System.Collections.Generic;
using System.Linq;

namespace CIS.Core.Domain.Common;

public class Region : Entity<Guid>
{
    private string _name;
    private ICollection<Province> _provinces = [];

    public virtual string Name
    {
        get => _name;
        set => _name = value.ToUpper();
    }

    public virtual ICollection<Province> Provinces
    {
        get => _provinces;
        set => SyncProvinces(value);
    }

    public override string ToString()
    {
        return this.Name;
    }

    #region Methods

    private void SyncProvinces(ICollection<Province> items)
    {
        foreach (var item in items)
            item.Region = this;

        var itemsToInsert = items.Except(_provinces).ToList();
        var itemsToUpdate = _provinces.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _provinces.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            item.Region = this;
            _provinces.Add(item);
        }

        // update
        foreach (var item in itemsToUpdate)
        {
            var value = items.Single(x => x == item);
            item.SerializeWith(value);
        }

        // delete
        foreach (var item in itemsToRemove)
        {
            item.Region = null;
            _provinces.Remove(item);
        }
    }

    public virtual void AddProvice(Province item)
    {
        item.Region = this;
        _provinces.Add(item);
    }

    #endregion
}

public static class AddressExtention
{
    public static string Key(this Region region) => $"{region}";
    public static string Key(this Province province) => $"{province.Region}{province}";
    public static string Key(this City city) => $"{city.Province.Region}{city.Province}{city}";
    public static string Key(this Barangay barangay) => $"{barangay.City.Province.Region}{barangay.City.Province}{barangay.City}{barangay}";
}
