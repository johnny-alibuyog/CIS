using System.ComponentModel;

namespace CIS.Core.Domain.Common;

public enum BloodType
{
    [Description("A+")]
    APositive,

    [Description("A-")]
    ANegative,

    [Description("B+")]
    BPositive,

    [Description("B-")]
    BNegative,

    [Description("AB+")]
    ABPositive,

    [Description("AB-")]
    ABNegative,

    [Description("O+")]
    OPositive,

    [Description("O-")]
    ONegative
}
