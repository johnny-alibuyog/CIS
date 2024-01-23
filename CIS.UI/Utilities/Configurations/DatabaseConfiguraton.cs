using System;

namespace CIS.UI.Utilities.Configurations;

[Serializable()]
public class DatabaseConfiguraton
{
    public virtual string ServerName { get; set; }
    public virtual string DatabaseName { get; set; }
    public virtual string Username { get; set; }
    public virtual string Password { get; set; }

    public DatabaseConfiguraton()
    {
        this.ServerName = "(local)";
        this.DatabaseName = "cisdb";
        this.Username = "sa";
        this.Password = "Alpha123$";
    }
}
