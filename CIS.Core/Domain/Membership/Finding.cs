﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CIS.Core.Domain.Membership;

public class Finding : Entity<Guid>
{
    private string _finalFindings;
    private Amendment _amendment;
    private ICollection<Hit> _hits = [];

    public virtual string FinalFindings
    {
        get => _finalFindings;
        set => _finalFindings = value;
    }

    public virtual Amendment Amendment
    {
        get => _amendment;
        set => _amendment = value;
    }

    public virtual IEnumerable<Hit> Hits
    {
        get => _hits;
        set => SyncHits(value);
    }

    private void SyncHits(IEnumerable<Hit> items)
    {
        foreach (var item in items)
            item.Finding = this;

        var itemsToInsert = items.Except(_hits).ToList();
        var itemsToUpdate = _hits.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _hits.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _hits.Add(item);
        }

        // update
        foreach (var item in itemsToUpdate)
        {
            var value = items.Single(x => x == item);

            if (item is SuspectHit suspectHit)
                suspectHit.SerializeWith((SuspectHit)value);
        }

        // delete
        foreach (var item in itemsToRemove)
        {
            _hits.Remove(item);
        }
    }

    private void SyncSuspectPartialMatches(IEnumerable<Hit> items)
    {
        var itemsToInsert = items.Except(_hits).ToList();
        var itemsToUpdate = _hits.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _hits.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
            _hits.Add(item);

        // update
        foreach (var item in itemsToUpdate)
        {
            var value = items.Single(x => x == item);
            item.IsIdentical = value.IsIdentical;
        }

        // delete
        foreach (var item in itemsToRemove)
            _hits.Remove(item);
    }
}