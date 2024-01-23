using CIS.Core.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Barangays;

public class Incumbent : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Office _office; //FIXME: what is this for?
    private DateTime? _date;
    private ICollection<Official> _officials;

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

    public virtual DateTime? Date
    {
        get => _date;
        set => _date = value;
    }

    public virtual IEnumerable<Official> Officials
    {
        get => _officials;
        set => SyncOfficials(value);
    }

    #region Methods

    private void SyncOfficials(IEnumerable<Official> items)
    {
        foreach (var item in items)
            item.Incumbent = this;

        var itemsToInsert = items.Except(_officials).ToList();
        var itemsToUpdate = _officials.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _officials.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            item.Incumbent = this;
            _officials.Add(item);
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
            item.Incumbent = null;
            _officials.Remove(item);
        }
    }

    #endregion

    #region Constructors

    public Incumbent()
    {
        _officials = new Collection<Official>();
        _date = DateTime.Today;
    }

    #endregion
}
