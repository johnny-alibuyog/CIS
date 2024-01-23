using System;
using System.Collections.Generic;

namespace CIS.Core.Entities.Commons;

public class Audit : ValueObject
{
    private string _createdBy;
    private string _updatedBy;
    private DateTime? _createdOn;
    private DateTime? _updatedOn;

    public virtual string CreatedBy
    {
        get => _createdBy;
        protected set => _createdBy = value;
    }

    public virtual string UpdatedBy
    {
        get => _updatedBy;
        protected set => _updatedBy = value;
    }

    public virtual DateTime? CreatedOn
    {
        get => _createdOn;
        protected set => _createdOn = value;
    }

    public virtual DateTime? UpdatedOn
    {
        get => _updatedOn;
        protected set => _updatedOn = value;
    }

    public static Audit Create(string createdBy, DateTime createdOn)
    {
        return new Audit()
        {
            CreatedBy = createdBy,
            CreatedOn = createdOn
        };
    }

    public static Audit Create(string updatedBy, DateTime updatedOn, Audit currentAudit)
    {
        return new Audit()
        {
            CreatedBy = currentAudit != null
                ? currentAudit.CreatedBy : updatedBy,
            CreatedOn = currentAudit != null
                ? currentAudit.CreatedOn : updatedOn,
            UpdatedBy = updatedBy,
            UpdatedOn = updatedOn
        };
    }

    protected override IEnumerable<object> GetEqualityValues()
    {
        yield return CreatedBy;
        yield return CreatedOn;
        yield return UpdatedBy;
        yield return UpdatedOn;
    }
}
