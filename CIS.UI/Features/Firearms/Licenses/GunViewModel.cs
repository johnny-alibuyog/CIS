using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class GunViewModel : ViewModelBase
    {
        private GunController _controller;

        public virtual string Model { get; set; }

        public virtual string Caliber { get; set; }

        public virtual string SerialNumber { get; set; }

        public virtual Lookup<Guid> Kind { get; set; }

        public virtual Lookup<Guid> Make { get; set; }

        public virtual IReactiveCollection<Lookup<Guid>> Kinds { get; set; }

        public virtual IReactiveCollection<Lookup<Guid>> Makes { get; set; }

        public GunViewModel()
        {
            _controller = new GunController(this);
        }
    }
}
