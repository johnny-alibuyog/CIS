using CIS.Core.Entities.Commons;
using System;

namespace CIS.Core.Entities.Barangays;

public abstract class Hit : Entity<Guid>
{
    private Finding _finding;
    private HitScore _hitScore;
    private bool _isIdentical;

    public virtual Finding Finding
    {
        get => _finding;
        set => _finding = value;
    }

    public virtual HitScore HitScore
    {
        get => _hitScore;
        set => _hitScore = value;
    }

    public virtual bool IsIdentical
    {
        get => _isIdentical;
        set => _isIdentical = value;
    }
}
