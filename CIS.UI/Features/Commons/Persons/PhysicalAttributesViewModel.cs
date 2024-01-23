using CIS.Core.Entities.Commons;

namespace CIS.UI.Features.Commons.Persons;

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
        else if (instance is PhysicalAttributes)
        {
            var source = instance as PhysicalAttributes;
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
        else if (instance is PhysicalAttributes)
        {
            var source = this;
            var target = instance as PhysicalAttributes;

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
