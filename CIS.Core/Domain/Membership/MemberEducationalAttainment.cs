using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;

namespace CIS.Core.Domain.Membership;

public class MemberEducationalAttainment : Entity<Guid>
{
    private Member _member;
    private readonly ICollection<Education> _educations = [];

    private void SyncDependents(IEnumerable<Education> items)
    {
        var itemsToInsert = items.Except(_educations).ToList();
        var itemsToUpdate = _educations.Where(items.Contains).ToList();
        var itemsToRemove = _educations.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _educations.Add(item);
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
            _educations.Remove(item);
        }
    }

    internal virtual void WithMember(Member member)
    {
        this._member = member;
    }
}
