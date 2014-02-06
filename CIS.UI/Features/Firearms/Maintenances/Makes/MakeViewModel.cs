using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Firearms.Maintenances.Makes
{
    public class MakeViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public MakeViewModel(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
