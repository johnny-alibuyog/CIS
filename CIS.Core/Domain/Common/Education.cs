using System;

namespace CIS.Core.Domain.Common;

public class Education : Entity<Guid>
{
    private EducationLevel _Level;
    private string _schoolName;
    private int? _yearGraduated;

    public virtual EducationLevel Level
    {
        get => _Level;
        set => _Level = value;
    }

    public virtual string SchoolName
    {
        get => _schoolName;
        set => _schoolName = value;
    }

    public virtual int? YearGraduated
    {
        get => _yearGraduated;
        set => _yearGraduated = value;
    }

    public virtual void SerializeWith(Education value)
    {
        this.Level = value.Level;
        this.SchoolName = value.SchoolName;
        this.YearGraduated = value.YearGraduated;
    }
}
