using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;

namespace CIS.Core.Domain.Membership;

public class Suspect : Entity<Guid>
{
    private long? _dataStoreId;
    private Guid? _dataStoreChildKey;
    //private int _version;
    private Audit _audit;
    private Warrant _warrant;
    private ArrestStatus? _arrestStatus;
    private DateTime? _arrestDate;
    private string _disposition;
    private Person _person;
    private Address _address;
    private PhysicalAttribute _physicalAttributes;
    private ICollection<string> _aliases = [];
    private ICollection<string> _occupations = [];

    public virtual long? DataStoreId
    {
        get => _dataStoreId;
        set => _dataStoreId = value;
    }

    public virtual Guid? DataStoreChildKey
    {
        get => _dataStoreChildKey;
        set => _dataStoreChildKey = value;
    }

    //public virtual int Version
    //{
    //    get => _version;
    //    protected set => _version = value;
    //}

    public virtual Audit Audit
    {
        get => _audit;
        protected set => _audit = value;
    }

    public virtual Warrant Warrant
    {
        get => _warrant;
        set => _warrant = value;
    }

    public virtual ArrestStatus? ArrestStatus
    {
        get => _arrestStatus;
        set => _arrestStatus = value;
    }

    public virtual DateTime? ArrestDate
    {
        get => _arrestDate;
        set => _arrestDate = value;
    }

    public virtual string Disposition
    {
        get => _disposition;
        set => _disposition = value;
    }

    public virtual Person Person
    {
        get => _person;
        set => _person = value;
    }

    public virtual Address Address
    {
        get => _address;
        set => _address = value;
    }

    public virtual PhysicalAttribute PhysicalAttributes
    {
        get => _physicalAttributes;
        set => _physicalAttributes = value;
    }

    public virtual IEnumerable<string> Aliases
    {
        get => _aliases;
        set => SyncAliases(value);
    }

    public virtual IEnumerable<string> Occupations
    {
        get => _occupations;
        set => SyncOccupations(value);
    }

    public virtual void SerializeWith(Suspect value)
    {
        this.Warrant = value.Warrant;
        this.ArrestStatus = value.ArrestStatus;
        this.Person = value.Person;
        this.Address = value.Address;
        this.PhysicalAttributes = value.PhysicalAttributes;
        this.Aliases = value.Aliases;
        this.Occupations = value.Occupations;
    }

    private void SyncAliases(IEnumerable<string> items)
    {
        items = items.Where(x => !string.IsNullOrWhiteSpace(x));

        var itemsToInsert = items.Except(_aliases).ToList();
        var itemsToRemove = _aliases.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
            _aliases.Add(item);

        // delete
        foreach (var item in itemsToRemove)
            _aliases.Remove(item);
    }

    private void SyncOccupations(IEnumerable<string> items)
    {
        items = items.Where(x => !string.IsNullOrWhiteSpace(x));

        var itemsToInsert = items.Except(_occupations).ToList();
        var itemsToRemove = _occupations.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
            _occupations.Add(item);

        // delete
        foreach (var item in itemsToRemove)
            _occupations.Remove(item);
    }
}
