using CIS.Core.Utilities.Extentions;
using System.Collections.Generic;

namespace CIS.Core.Entities.Commons;

public class Address : ValueObject
{
    private string _address1;
    private string _address2;
    private string _barangay;
    private string _city;
    private string _province;

    public virtual string Address1
    {
        get => _address1;
        set => _address1 = value.ToProperCase();
    }

    public virtual string Address2
    {
        get => _address2;
        set => _address2 = value.ToProperCase();
    }

    public virtual string Barangay
    {
        get => _barangay;
        set => _barangay = value.ToProperCase();
    }

    public virtual string City
    {
        get => _city;
        set => _city = value.ToProperCase();
    }

    public virtual string Province
    {
        get => _province;
        set => _province = value.ToProperCase();
    }

    public override string ToString()
    {
        return
        (
            (!string.IsNullOrWhiteSpace(Address1) ? Address1 : string.Empty) +
            (!string.IsNullOrWhiteSpace(Address2) ? " " + Address2 : string.Empty) +
            (!string.IsNullOrWhiteSpace(Barangay) ? ", " + Barangay : string.Empty) +
            (!string.IsNullOrWhiteSpace(City) ? ", " + City : string.Empty) +
            (!string.IsNullOrWhiteSpace(Province) ? ", " + Province : string.Empty)
        )
        .Trim();
    }

    protected override IEnumerable<object> GetEqualityValues()
    {
        yield return Address1;
        yield return Address2;
        yield return Barangay;
        yield return City;
        yield return Province;
    }
}
