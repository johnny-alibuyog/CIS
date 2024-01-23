using CIS.Core.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Barangays;

public class Blotter : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private string _complaint;
    private string _details;
    private string _remarks;
    private BlotterStatus? _status;
    private DateTime? _filedOn;
    private DateTime? _occuredOn;
    private Address _address;
    private Incumbent _incumbent;
    private ICollection<Official> _officials;
    private ICollection<Citizen> _complainants;
    private ICollection<Citizen> _respondents;
    private ICollection<Citizen> _witnesses;

    public virtual int Version
    {
        get => _version;
        set => _version = value;
    }

    public virtual Audit Audit
    {
        get => _audit;
        set => _audit = value;
    }

    public virtual string Complaint
    {
        get => _complaint;
        set => _complaint = value;
    }

    public virtual string Details
    {
        get => _details;
        set => _details = value;
    }

    public virtual string Remarks
    {
        get => _remarks;
        set => _remarks = value;
    }

    public virtual BlotterStatus? Status
    {
        get => _status;
        set => _status = value;
    }

    public virtual DateTime? FiledOn
    {
        get => _filedOn;
        set => _filedOn = value;
    }

    public virtual DateTime? OccuredOn
    {
        get => _occuredOn;
        set => _occuredOn = value;
    }

    public virtual Address Address
    {
        get => _address;
        set => _address = value;
    }

    public virtual Incumbent Incumbent
    {
        get => _incumbent;
        set => _incumbent = value;
    }

    public virtual IEnumerable<Official> Officials
    {
        get => _officials;
        set => SyncOfficials(value);
    }

    public virtual IEnumerable<Citizen> Complainants
    {
        get => _complainants;
        set => SyncComplainants(value);
    }

    public virtual IEnumerable<Citizen> Respondents
    {
        get => _respondents;
        set => SyncRespondents(value);
    }

    public virtual IEnumerable<Citizen> Witnesses
    {
        get => _witnesses;
        set => SyncWitnesses(value);
    }

    #region Constructors

    public Blotter()
    {
        _status = BlotterStatus.Open;
        _officials = new Collection<Official>();
        _complainants = new Collection<Citizen>();
        _respondents = new Collection<Citizen>();
        _witnesses = new Collection<Citizen>();
    }

    #endregion

    #region Methods

    private void SyncOfficials(IEnumerable<Official> items)
    {
        var itemsToInsert = items.Except(_officials).ToList();
        var itemsToRemove = _officials.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
            _officials.Add(item);

        // delete
        foreach (var item in itemsToRemove)
            _officials.Remove(item);
    }

    private void SyncComplainants(IEnumerable<Citizen> items)
    {
        var itemsToInsert = items.Except(_complainants).ToList();
        var itemsToUpdate = _complainants.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _complainants.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _complainants.Add(item);
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
            _complainants.Remove(item);
        }
    }

    private void SyncRespondents(IEnumerable<Citizen> items)
    {
        var itemsToInsert = items.Except(_respondents).ToList();
        var itemsToUpdate = _respondents.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _respondents.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _respondents.Add(item);
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
            _respondents.Remove(item);
        }
    }

    private void SyncWitnesses(IEnumerable<Citizen> items)
    {
        var itemsToInsert = items.Except(_witnesses).ToList();
        var itemsToUpdate = _witnesses.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _witnesses.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _witnesses.Add(item);
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
            _witnesses.Remove(item);
        }
    }

    #endregion
}
