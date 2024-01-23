namespace CIS.Core.Entities.Commons;

public class Finger : Entity<string>
{
    private string _name;
    private string _imageUri;

    public virtual string Name
    {
        get => _name;
        protected set => _name = value;
    }

    public virtual string ImageUri
    {
        get => _imageUri;
        protected set => _imageUri = value;
    }

    public static readonly Finger RightThumb = new()
    {
        Id = "RT",
        Name = "Right Thumb",
        ImageUri = "/Assets/Images/FF-R1.png"
        //ImageUri = "/Assets/Images/FF-R1.jpg"
    };

    public static readonly Finger RightIndex = new()
    {
        Id = "RI",
        Name = "Right Index",
        ImageUri = "/Assets/Images/FF-R2.png"
        //ImageUri = "/Assets/Images/FF-R2.jpg"
    };

    public static readonly Finger RightMiddle = new()
    {
        Id = "RM",
        Name = "Right Middle",
        ImageUri = "/Assets/Images/FF-R3.png"
        //ImageUri = "/Assets/Images/FF-R3.jpg"
    };

    public static readonly Finger RightRing = new()
    {
        Id = "RR",
        Name = "Right Ring",
        ImageUri = "/Assets/Images/FF-R4.png"
        //ImageUri = "/Assets/Images/FF-R4.jpg"
    };

    public static readonly Finger RightPinky = new()
    {
        Id = "RP",
        Name = "Right Pinky",
        ImageUri = "/Assets/Images/FF-R5.png"
        //ImageUri = "/Assets/Images/FF-R5.jpg"
    };

    public static readonly Finger LeftThumb = new()
    {
        Id = "LT",
        Name = "Left Thumb",
        ImageUri = "/Assets/Images/FF-L1.png"
        //ImageUri = "/Assets/Images/FF-L1.jpg"
    };

    public static readonly Finger LeftIndex = new()
    {
        Id = "LI",
        Name = "Left Index",
        ImageUri = "/Assets/Images/FF-L2.png"
        //ImageUri = "/Assets/Images/FF-L2.jpg"
    };

    public static readonly Finger LeftMiddle = new()
    {
        Id = "LM",
        Name = "Left Middle",
        ImageUri = "/Assets/Images/FF-L3.png"
        //ImageUri = "/Assets/Images/FF-L3.jpg"
    };

    public static readonly Finger LeftRing = new()
    {
        Id = "LR",
        Name = "Left Ring",
        ImageUri = "/Assets/Images/FF-L4.png"
        //ImageUri = "/Assets/Images/FF-L4.jpg"
    };

    public static readonly Finger LeftPinky = new()
    {
        Id = "LP",
        Name = "Left Pinky",
        ImageUri = "/Assets/Images/FF-L5.png"
        //ImageUri = "/Assets/Images/FF-L5.jpg"
    };

    public static readonly Finger[] All =
    [
        Finger.RightThumb,
        Finger.RightIndex,
        Finger.RightMiddle,
        Finger.RightRing,
        Finger.RightPinky,
        Finger.LeftThumb,
        Finger.LeftIndex,
        Finger.LeftMiddle,
        Finger.LeftRing,
        Finger.LeftPinky
    ];
}
