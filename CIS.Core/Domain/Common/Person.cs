using CIS.Core.Utility.Extention;
using System;
using System.Collections.Generic;

namespace CIS.Core.Domain.Common;

public class Person : ValueObject
{
    private string _prefix;
    private string _firstName;
    private string _middleName;
    private string _lastName;
    private string _suffix;
    private Gender? _gender;
    private DateTime? _birthDate;
    private string _birthPlace;

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

    public virtual Gender? Gender
    {
        get => _gender;
        set => _gender = value;
    }

    public virtual DateTime? BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    public string BirthPlace
    {
        get => _birthPlace;
        set => _birthPlace = value.ToProperCase();
    }

    public virtual int? Age
    {
        get => this.ComputeAge();
    }

    public virtual string Fullname
    {
        get => this.GetFullName();
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

    private int? ComputeAge()
    {
        if (this.BirthDate == null)
            return null;

        var value = this.BirthDate.Value;
        var now = DateTime.Now;
        var age = now.Year - value.Year;

        if (now.Month < value.Month || (now.Month == value.Month && now.Day < value.Day))
            age--;

        return age;
    }

    public override string ToString()
    {
        return this.Fullname;
    }

    protected override IEnumerable<object> GetEqualityValues()
    {
        yield return this.Fullname;
    }
}
