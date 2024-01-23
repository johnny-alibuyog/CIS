using CIS.Core.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Mayors;

public class Incumbent : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private ICollection<Official> _officials = new Collection<Official>();
    private DateTime? _date;

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

    public virtual IEnumerable<Official> Officials
    {
        get => _officials;
        set => SyncSecretaries(value);
    }

    public virtual DateTime? Date
    {
        get => _date;
        set => _date = value;
    }

    #region Methods

    private void SyncSecretaries(IEnumerable<Official> items)
    {
        var itemsToInsert = items.Except(_officials).ToList();
        var itemsToUpdate = _officials.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _officials.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
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
            _officials.Remove(item);
        }
    }

    #endregion
}
