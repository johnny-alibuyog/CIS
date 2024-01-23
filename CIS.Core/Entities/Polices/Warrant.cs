using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Polices;

public class Warrant : Entity<Guid>
{
    private Guid? _dataStoreParentKey;
    private Audit _audit;
    private string _warrantCode;
    private string _caseNumber;
    private string _crime;
    private string _description;
    private string _remarks;
    private decimal _bailAmount;
    private DateTime? _issuedOn;
    private string _issuedBy;
    private Address _issuedAt;
    private ICollection<Suspect> _suspects = new Collection<Suspect>();

    public virtual Guid? DataStoreParentKey
    {
        get => _dataStoreParentKey;
        set => _dataStoreParentKey = value;
    }

    public virtual Audit Audit
    {
        get => _audit;
        protected set => _audit = value;
    }

    public virtual string WarrantCode
    {
        get => _warrantCode;
        set => _warrantCode = value;
    }

    public virtual string CaseNumber
    {
        get => _caseNumber;
        set => _caseNumber = value;
    }

    public virtual string Crime
    {
        get => _crime;
        set => _crime = value.ToProperCase();
    }

    public virtual string Description
    {
        get => _description;
        set => _description = value;
    }

    public virtual string Remarks
    {
        get => _remarks;
        set => _remarks = value;
    }

    public virtual decimal BailAmount
    {
        get => _bailAmount;
        set => _bailAmount = value;
    }

    public virtual DateTime? IssuedOn
    {
        get => _issuedOn;
        set => _issuedOn = value;
    }

    public virtual string IssuedBy
    {
        get => _issuedBy;
        set => _issuedBy = value;
    }

    public virtual Address IssuedAt
    {
        get => _issuedAt;
        set => _issuedAt = value;
    }

    public virtual IEnumerable<Suspect> Suspects
    {
        get => _suspects;
        set => SyncSuspects(value);
    }

    private void SyncSuspects(IEnumerable<Suspect> items)
    {
        foreach (var item in items)
            item.Warrant = this;

        var itemsToInsert = items.Except(_suspects).ToList();
        var itemsToUpdate = _suspects.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _suspects.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            item.Warrant = this;
            _suspects.Add(item);
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
            item.Warrant = null;
            _suspects.Remove(item);
        }
    }

    public virtual void AddSuspect(Suspect item)
    {
        item.Warrant = this;
        _suspects.Add(item);
    }

    public virtual void DeleteSuspect(Suspect item)
    {
        item.Warrant = null;
        _suspects.Remove(item);
    }
}
