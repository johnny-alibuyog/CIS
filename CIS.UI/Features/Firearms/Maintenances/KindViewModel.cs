using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Firearms
{
    public class KindViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public KindViewModel(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
