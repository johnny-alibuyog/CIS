using CIS.Core.Utilities.Extentions;
using System.Collections.Generic;

namespace CIS.Core.Entities.Commons;

public class PhysicalAttributes : ValueObject
{
    private string _hair;
    private string _eyes;
    private string _build;
    private string _complexion;
    private string _scarsAndMarks;
    private string _race;
    private string _nationality;

    public virtual string Hair
    {
        get => _hair;
        set => _hair = value.ToProperCase();
    }

    public virtual string Eyes
    {
        get => _eyes;
        set => _eyes = value.ToProperCase();
    }

    public virtual string Build
    {
        get => _build;
        set => _build = value.ToProperCase();
    }

    public virtual string Complexion
    {
        get => _complexion;
        set => _complexion = value.ToProperCase();
    }

    public virtual string ScarsAndMarks
    {
        get => _scarsAndMarks;
        set => _scarsAndMarks = value.ToProperCase();
    }

    public virtual string Race
    {
        get => _race;
        set => _race = value.ToProperCase();
    }

    public virtual string Nationality
    {
        get => _nationality;
        set => _nationality = value.ToProperCase();
    }

    protected override IEnumerable<object> GetEqualityValues()
    {
        yield return Hair;
        yield return Eyes;
        yield return Build;
        yield return Complexion;
        yield return ScarsAndMarks;
        yield return Race;
        yield return Nationality;
    }
}
