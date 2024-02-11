using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;

namespace CIS.Core.Domain.Membership;

public class Station : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private ImageBlob _logo = new();
    private string _name;
    private string _office;
    private string _location;
    private decimal? _clearanceFee;
    private int? _clearanceValidityInDays;
    private Address _address;
    private ICollection<Officer> _officers = [];

    public virtual int Version
    {
        get => _version;
        protected set => _version = value;
    }

    public virtual Audit Audit
    {
        get => _audit;
        set => _audit = value;
    }

    public virtual ImageBlob Logo
    {
        get => _logo;
        set => _logo = value;
    }

    public virtual string Name
    {
        get => _name;
        set => _name = value.ToProperCase();
    }

    public virtual string Office
    {
        get => _office;
        set => _office = value.ToProperCase();
    }

    public virtual string Location
    {
        get => _location;
        set => _location = value.ToProperCase();
    }

    public virtual decimal? ClearanceFee
    {
        get => _clearanceFee;
        set => _clearanceFee = value;
    }

    public virtual int? ClearanceValidityInDays
    {
        get => _clearanceValidityInDays;
        set => _clearanceValidityInDays = value;
    }

    public virtual Address Address
    {
        get => _address;
        set => _address = value;
    }

    public virtual IEnumerable<Officer> Officers
    {
        get => _officers;
        set => SyncOfficers(value);
    }

    private void SyncOfficers(IEnumerable<Officer> items)
    {
        foreach (var item in items)
            item.Station = this;

        var itemsToInsert = items.Except(_officers).ToList();
        var itemsToUpdate = _officers.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _officers.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            item.Station = this;
            _officers.Add(item);
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
            item.Station = null;
            _officers.Remove(item);
        }
    }

    public virtual void AddOfficer(Officer item)
    {
        item.Station = this;
        _officers.Add(item);
    }
}
