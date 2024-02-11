using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;

namespace CIS.Core.Domain.Membership;

public class MemberDependentInformation: Entity<Guid>
{
    private Member _member;
    private readonly ICollection<Dependent> _dependents = [];

    public virtual Member Member
    {
        get => _member;
        protected set => _member = value;
    }

    public virtual IEnumerable<Dependent> Dependents
    {
        get => _dependents;
        protected set => SyncDependents(value);
    }

    private void SyncDependents(IEnumerable<Dependent> items)
    {
        var itemsToInsert = items.Except(_dependents).ToList();
        var itemsToUpdate = _dependents.Where(items.Contains).ToList();
        var itemsToRemove = _dependents.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _dependents.Add(item);
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
            _dependents.Remove(item);
        }
    }

    internal void WithMember(Member member)
    {
        this._member = member;
    }
}
