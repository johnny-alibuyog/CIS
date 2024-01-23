using CIS.Core.Entities.Memberships;
using System;

namespace CIS.Core.Entities.Polices;

public class Amendment : Entity<Guid>
{
    private User _approver;
    private string _documentNumber;
    private string _reason;
    private string _remarks;

    public virtual User Approver
    {
        get => _approver;
        set => _approver = value;
    }

    public virtual string DocumentNumber
    {
        get => _documentNumber;
        set => _documentNumber = value;
    }

    public virtual string Reason
    {
        get => _reason;
        set => _reason = value;
    }

    public virtual string Remarks
    {
        get => _remarks;
        set => _remarks = value;
    }
}
