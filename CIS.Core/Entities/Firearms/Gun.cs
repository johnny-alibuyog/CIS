using System.Collections.Generic;

namespace CIS.Core.Entities.Firearms;

public class Gun : ValueObject
{
    private string _model;
    private string _caliber;
    private string _serialNumber;
    private Kind _kind;
    private Make _make;

    public virtual string Model
    {
        get => _model;
        set => _model = value;
    }

    public virtual string Caliber
    {
        get => _caliber;
        set => _caliber = value;
    }

    public virtual string SerialNumber
    {
        get => _serialNumber;
        set => _serialNumber = value;
    }

    public virtual Kind Kind
    {
        get => _kind;
        set => _kind = value;
    }

    public virtual Make Make
    {
        get => _make;
        set => _make = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Model;
        yield return this.Caliber;
        yield return this.SerialNumber;
        yield return this.Kind;
        yield return this.
Make;
    }
}
