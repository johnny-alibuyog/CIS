namespace CIS.Core.Domain.Membership;

public class SuspectHit : Hit
{
    private Suspect _suspect;

    public virtual Suspect Suspect
    {
        get { return _suspect; }
        set { _suspect = value; }
    }

    public virtual void SerializeWith(SuspectHit value)
    {
        this.HitScore = value.HitScore;
        this.IsIdentical = value.IsIdentical;
        this.Suspect = value.Suspect;
    }
}
