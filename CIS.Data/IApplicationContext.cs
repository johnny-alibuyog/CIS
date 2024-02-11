using CIS.Core.Domain.Common;
using CIS.Core.Domain.Security;

namespace CIS.Core;

public interface IContext
{
    User User { get; set; }
    City City { get; set; }
    Terminal Terminal { get; set; }
    ProductConfiguration Product { get; set; }
    DataStoreConfiguration DataStore { get; set; }
    ImageScaleFactorConfiguration Image { get; set; }
}
