using System;

namespace CIS.Core.Domain.Common;

public class Education : Entity<Guid>
{
    private EducationLevel _educationLevel;
    private string _schoolName;
    private DateTime? _dateGraduated;

    public virtual EducationLevel EducationLevel
    {
        get => _educationLevel;
        set => _educationLevel = value;
    }

    public virtual string SchoolName
    {
        get => _schoolName;
        set => _schoolName = value;
    }

    public virtual DateTime? DateGraduated
    {
        get => _dateGraduated;
        set => _dateGraduated = value;
    }

    internal virtual void SerializeWith(Education value)
    {
        this.EducationLevel = value.EducationLevel;
        this.SchoolName = value.SchoolName;
        this.DateGraduated = value.DateGraduated;
    }
}
