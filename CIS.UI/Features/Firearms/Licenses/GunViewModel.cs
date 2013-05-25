using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;
using CIS.Data;
using CIS.UI.Bootstraps.DependencyInjection;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class GunViewModel : ViewModelBase
    {
        private readonly GunController _controller;

        [NotNullNotEmpty]
        public virtual string Model { get; set; }

        [NotNullNotEmpty]
        public virtual string Caliber { get; set; }

        [NotNullNotEmpty]
        public virtual string SerialNumber { get; set; }

        [NotNull]
        public virtual Lookup<Guid> Kind { get; set; }

        [NotNull]
        public virtual Lookup<Guid> Make { get; set; }

        public virtual IReactiveCollection<Lookup<Guid>> Kinds { get; set; }

        public virtual IReactiveCollection<Lookup<Guid>> Makes { get; set; }

        public GunViewModel()
        {
            _controller = new GunController(this);
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is GunViewModel)
            {
                var source = instance as GunViewModel;
                var target = this;

                target.Model = source.Model;
                target.Caliber = source.Caliber;
                target.SerialNumber = source.SerialNumber;
                target.Kind = source.Kind;
                target.Make = source.Make;

                return target;
            }
            else if (instance is Gun)
            {
                var source = instance as Gun;
                var target = this;

                target.Model = source.Model;
                target.Caliber = source.Caliber;
                target.SerialNumber = source.SerialNumber;
                target.Kind = target.Kinds.FirstOrDefault(x => x.Id == source.Kind.Id);
                target.Make = target.Makes.FirstOrDefault(x => x.Id == source.Make.Id);

                return target;
            }

            return null;
        }

        public override object SerializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is GunViewModel)
            {
                var source = this;
                var destination = instance as GunViewModel;

                destination.SerializeWith(source);
                return destination;
            }
            else if (instance is Gun)
            {
                var source = this;
                var target = instance as Gun;

                var session = IoC.Container.Resolve<ISessionProvider>().GetSharedSession();

                target.Model = source.Model;
                target.Caliber = source.Caliber;
                target.SerialNumber = source.SerialNumber;
                target.Kind = session.Load<Kind>(source.Kind.Id);
                target.Make = session.Load<Make>(source.Make.Id);

                return target;
            }

            return null;
        }
    }
}
