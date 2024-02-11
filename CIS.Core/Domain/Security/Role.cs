using System.ComponentModel;

namespace CIS.Core.Domain.Security;

public enum Role
{
    [Description("System Administrator")]
    SystemAdministrator = 0,

    [Description("Police Administartor")]
    PoliceAdministartor = 101,

    [Description("Police Approver")]
    PoliceApprover = 102,

    [Description("Police Encoder")]
    PoliceEncoder = 103,

    [Description("Barangay Administartor")]
    BarangayAdministartor = 201,

    [Description("Barangay Approver")]
    BarangayApprover = 202,

    [Description("Barangay Encoder")]
    BarangayEncoder = 203,

    [Description("Mayor Administrator")]
    MayorAdministrator = 301,

    [Description("Mayor Approver")]
    MayorApprover = 302,

    [Description("Mayor Encoder")]
    MayorEncoder = 303,
}
