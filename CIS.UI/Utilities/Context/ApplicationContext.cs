using CIS.Core.Domain.Common;
using CIS.Core.Domain.Security;
using CIS.Store.Services;

namespace CIS.UI.Utilities.Context;

public class ApplicationContext
{
    public virtual User User { get; set; }
    public virtual City City { get; set; }
    public virtual Terminal Terminal { get; set; }
    public virtual ProductConfiguration Product { get; set; }
    public virtual DataStoreConfiguration DataStore { get; set; }
    public virtual ImageScaleFactorConfiguration Image { get; set; }

    public virtual ClientInfo GetClientInfo()
    {
        return new ClientInfo()
        {
            Username = User.Username,
            Origin = Terminal.MachineName
        };
    }
}
