using System;

namespace CIS.UI.Utilities.Configurations;

[Serializable()]
public class LoginConfiguration
{
    public virtual string PowerUser { get; set; }
    public virtual bool UsePowerUser { get; set; }

    public LoginConfiguration()
    {
        this.PowerUser = "power_user";
        this.UsePowerUser = false;
    }
}
