using System;

namespace CIS.Core.Domain.Common;

public class Barangay : Entity<Guid>
{
    private string _name;
    private City _city;
    private AreaClass? _areaClass;
    private long? _population;

    public virtual string Name
    {
        get { return _name; }
        set { _name = value.ToUpper(); }
    }

    public virtual City City
    {
        get { return _city; }
        set { _city = value; }
    }

    public virtual AreaClass? AreaClass
    {
        get { return _areaClass; }
        set { _areaClass = value; }
    }

    public virtual long? Population
    {
        get { return _population; }
        set { _population = value; }
    }

    public virtual void SerializeWith(Barangay value)
    {
        this.Name = value.Name;
        this.City = value.City;
        this.Population = value.Population;
        this.AreaClass = value.AreaClass;
    }

    public override string ToString()
    {
        return this.Name;
    }
}
