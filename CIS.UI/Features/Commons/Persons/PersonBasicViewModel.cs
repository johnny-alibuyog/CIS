using System.Collections.Generic;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using NHibernate.Validator.Constraints;

namespace CIS.UI.Features.Commons.Persons;

public class PersonBasicViewModel : ViewModelBase
{
    public virtual string Prefix { get; set; }

    [NotNullNotEmpty(Message = "First name is mandatory.")]
    public virtual string FirstName { get; set; }

    public virtual string MiddleName { get; set; }

    [NotNullNotEmpty(Message = "Last name is mandatory.")]
    public virtual string LastName { get; set; }

    public virtual string Suffix { get; set; }

    public virtual string FullName
    {
        get { return this.GetFullName(); }
    }

    public PersonBasicViewModel() { }

    public PersonBasicViewModel(PersonBasic value)
    {
        this.SerializeWith(value);
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
        .ToProperCase()
        .Trim();
    }

    public override string ToString()
    {
        return this.GetFullName();
    }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance == Empty)
            return null;

        if (instance is PersonBasicViewModel)
        {
            var source = instance as PersonBasicViewModel;
            var target = this;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            return target;
        }
        else if (instance is PersonBasic)
        {
            var source = instance as PersonBasic;
            var target = this;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (this == PersonBasicViewModel.Empty)
            return null;

        if (instance is PersonBasicViewModel)
        {
            var source = this;
            var target = instance as PersonBasicViewModel;

            target.SerializeWith(source);
            return target;
        }
        else if (instance is PersonBasic)
        {
            var source = this;
            var target = instance as PersonBasic;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;

            return target;
        }

        return null;
    }

    public static readonly PersonBasicViewModel Empty = new();

    protected override IEnumerable<object> GetEqualityValues() 
    {
        yield return this.GetFullName();
    }
}
