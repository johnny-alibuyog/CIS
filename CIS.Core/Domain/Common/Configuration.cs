using System;
using System.Collections.Generic;
using System.Linq;

namespace CIS.Core.Domain.Common;

public class Configuration : Entity<Guid>
{
    private IDictionary<string, string> _properties = new Dictionary<string, string>();

    public virtual IDictionary<string, string> Properties
    {
        get => _properties;
        set => _properties = value;
    }

    protected virtual string GetPropertyValue(string key)
    {
        return this.Properties.ContainsKey(key) ? this.Properties[key] : null;
    }

    protected virtual IEnumerable<string> GetListPropertyValue(string key)
    {
        return this.Properties.Where(x => x.Key.StartsWith(key)).Select(x => x.Value);
    }

    protected virtual void SetPropertyValue(string key, string value)
    {
        if (this.Properties.ContainsKey(key))
            this.Properties[key] = value;
        else
            this.Properties.Add(key, value);
    }

    protected virtual void SetListPropertyValue(string key, IEnumerable<string> items)
    {
        var originalItems = this.GetListPropertyValue(key);
        var itemsToInsert = items.Except(originalItems).ToList();
        var itemsToRemove = originalItems.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
            this.Properties.Add(key + item, item);

        // remove
        foreach (var item in itemsToRemove)
            this.Properties.Remove(key + item);
    }
}
