using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.Data;
using CIS.UI.Bootstraps.DependencyInjection;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Context;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class OfficerViewModel : ViewModelBase
    {
        private readonly OfficerController _controller;

        public virtual Guid Id { get; set; }

        public virtual PersonViewModel Person { get; set; }

        [NotNull(Message = "Rank is mandatory.")]
        public virtual Lookup<string> Rank { get; set; }

        public virtual IReactiveList<Lookup<string>> Ranks { get; set; }

        [NotNullNotEmpty(Message = "Positon is mandatory.")]
        public virtual string Position { get; set; }

        public virtual BitmapSource Signature { get; set; }

        public virtual IReactiveCommand CaptureSignature { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is OfficerViewModel)
            {
                var source = instance as OfficerViewModel;
                var target = this;

                target.Id = source.Id;
                target.Person.SerializeWith(source.Person);
                target.Rank = source.Rank;
                target.Position = source.Position;
                target.Signature = source.Signature;
                return target;
            }
            else if (instance is Officer)
            {
                var source = instance as Officer;
                var target = this;

                target.Id = source.Id;
                target.Person.SerializeWith(source.Person);
                target.Rank = new Lookup<string>()
                {
                    Id = source.Rank.Id,
                    Name = source.Rank.Name
                };
                target.Position = source.Position;
                target.Signature = source.Signature.Image.ToBitmapSource();

                return target;
            }

            return null;
        }

        public override object SerializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is StationViewModel)
            {
                var source = this;
                var target = instance as StationViewModel;

                source.SerializeWith(target);
                return target;
            }
            else if (instance is Officer)
            {
                var source = this;
                var target = instance as Officer;

                target.Id = source.Id;
                target.Person = (Person)source.Person.SerializeInto(new Person());
                target.Rank = IoC.Container.Resolve<ISessionProvider>().GetSharedSession().Load<Rank>(source.Rank.Id);
                target.Position = source.Position;
                target.Signature.Image = source.Signature.ToImage();

                return target;
            }

            return null;
        }

        public OfficerViewModel()
        {
            _controller = new OfficerController(this);
        }
    }
}
