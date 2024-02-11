using CIS.Core.Domain.Common;

namespace CIS.UI.Features.Common.Person;

public class PhysicalAttributesViewModel : ViewModelBase
{
    public virtual string Hair { get; set; }
    public virtual string Eyes { get; set; }
    public virtual string Build { get; set; }
    public virtual string Complexion { get; set; }
    public virtual string ScarsAndMarks { get; set; }
    public virtual string Race { get; set; }
    public virtual string Nationality { get; set; }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is PhysicalAttributesViewModel)
        {
            var source = instance as PhysicalAttributesViewModel;
            var target = this;

            target.Hair = source.Hair;
            target.Eyes = source.Eyes;
            target.Build = source.Build;
            target.Complexion = source.Complexion;
            target.ScarsAndMarks = source.ScarsAndMarks;
            target.Race = source.Race;
            target.Nationality = source.Nationality;
            return target;
        }
        else if (instance is PhysicalAttribute)
        {
            var source = instance as PhysicalAttribute;
            var target = this;

            target.Hair = source.Hair;
            target.Eyes = source.Eyes;
            target.Build = source.Build;
            target.Complexion = source.Complexion;
            target.ScarsAndMarks = source.ScarsAndMarks;
            target.Race = source.Race;
            target.Nationality = source.Nationality;
            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (instance is PhysicalAttributesViewModel)
        {
            var source = this;
            var target = instance as PhysicalAttributesViewModel;

            target.SerializeWith(source);
            return target;
        }
        else if (instance is PhysicalAttribute)
        {
            var source = this;
            var target = instance as PhysicalAttribute;

            target.Hair = source.Hair;
            target.Eyes = source.Eyes;
            target.Build = source.Build;
            target.Complexion = source.Complexion;
            target.ScarsAndMarks = source.ScarsAndMarks;
            target.Race = source.Race;
            target.Nationality = source.Nationality;
            return target;
        }

        return null;
    }
}
