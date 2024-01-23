using CIS.Core.Utilities.Extentions;
using System.Collections.Generic;

namespace CIS.Core.Entities.Commons;

public class PersonBasic : ValueObject
{
    private string _prefix;
    private string _firstName;
    private string _middleName;
    private string _lastName;
    private string _suffix;

    public virtual string Prefix
    {
        get => _prefix;
        set => _prefix = value.ToProperCase();
    }

    public virtual string FirstName
    {
        get => _firstName;
        set => _firstName = value.ToProperCase();
    }

    public virtual string MiddleName
    {
        get => _middleName;
        set => _middleName = value.ToProperCase();
    }

    public virtual string LastName
    {
        get => _lastName;
        set => _lastName = value.ToProperCase();
    }

    public virtual string Suffix
    {
        get => _suffix;
        set => _suffix = value.ToProperCase();
    }

    public virtual string Fullname => this.GetFullName();

    public virtual void SerializeWith(PersonBasic value)
    {
        this.Prefix = value.Prefix;
        this.FirstName = value.FirstName;
        this.MiddleName = value.MiddleName;
        this.LastName = value.LastName;
        this.Suffix = value.Suffix;
    }

    private string GetFullName()
    {
        return
        (
            (!string.IsNullOrWhiteSpace(this.Prefix) ? this.Prefix : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.FirstName) ? " " + this.FirstName : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.MiddleName) ? " " + this.MiddleName : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.LastName) ? " " + this.LastName : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.Suffix) ? " " + this.Suffix : string.Empty)
        )
        .Trim();
    }

    public override string ToString()
    {
        return this.GetFullName();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Fullname;
    }
}
