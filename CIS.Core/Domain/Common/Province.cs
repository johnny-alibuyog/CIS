using System;
using System.Collections.Generic;
using System.Linq;

namespace CIS.Core.Domain.Common;

public class Province : Entity<Guid>
{
    private string _name;
    private Region _region;
    private ICollection<City> _cities = [];

    public virtual string Name
    {
        get { return _name; }
        set { _name = value.ToUpper(); }
    }

    public virtual Region Region
    {
        get { return _region; }
        set { _region = value; }
    }

    public virtual IEnumerable<City> Cities
    {
        get { return _cities; }
        set { SyncCities(value); }
    }

    public override string ToString()
    {
        return this.Name;
    }

    #region Methods

    private void SyncCities(IEnumerable<City> items)
    {
        foreach (var item in items)
            item.Province = this;
        
        var itemsToInsert = items.Except(_cities).ToList();
        var itemsToUpdate = _cities.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _cities.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            item.Province = this;
            _cities.Add(item);
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
            item.Province = null;
            _cities.Remove(item);
        }
    }

    public virtual void SerializeWith(Province value)
    {
        this.Name = value.Name;
        this.Region = value.Region;
    }

    public virtual void AddCity(City item)
    {
        item.Province = this;
        _cities.Add(item);
    }

    #endregion
}
