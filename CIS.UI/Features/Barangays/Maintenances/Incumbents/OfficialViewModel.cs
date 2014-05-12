using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.Data;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents
{
    public class OfficialViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        [Valid]
        public virtual PersonViewModel Person { get; set; }

        [NotNull(Message = "Position is mandatory.")]
        public virtual PositionViewModel Position { get; set; }

        public virtual Lookup<string> Committee { get; set; }

        public virtual BitmapSource Picture { get; set; }

        public virtual BitmapSource Signature { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string Display
        {
            get
            {
                if (this.Person == null)
                    return string.Empty;

                return string.Format("{0} {1}",
                    this.Person.FullName,
                    this.IsActive ? "(Active)" : string.Empty
                );
            }
        }

        public virtual IReactiveList<PositionViewModel> Positions { get; set; }

        public virtual IReactiveCommand CapturePicture { get; set; }

        public virtual IReactiveCommand CaptureSignature { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is OfficialViewModel)
            {
                var source = instance as OfficialViewModel;
                var target = this;

                target.Positions = new ReactiveList<PositionViewModel>(source.Positions);

                target.Id = source.Id;
                //target.Person.SerializeWith(source.Person);
                target.Person = (PersonViewModel)source.Person.DeserializeInto(new PersonViewModel());
                target.Position = target.Positions.FirstOrDefault(x => x.Id == source.Position.Id);
                target.Committee = source.Committee; //target.Position.Committees.FirstOrDefault(x => x.Id  == source.Committee.Id);
                target.Picture = source.Picture;
                target.Signature = source.Signature;
                target.IsActive = source.IsActive;
                return target;
            }
            else if (instance is Official)
            {
                var source = instance as Official;
                var target = this;

                target.Id = source.Id;
                target.Person.SerializeWith(source.Person);
                target.Position = source.Position != null ? new PositionViewModel(source.Position.Id, source.Position.Name) : null;
                target.Committee = source.Committee != null ? new Lookup<string>(source.Committee.Id, source.Committee.Name) : null;
                target.Picture = source.Picture.Image.ToBitmapSource();
                target.Signature = source.Signature.Image.ToBitmapSource();
                target.IsActive = source.IsActive;

                return target;
            }

            return null;
        }

        public override object DeserializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is OfficialViewModel)
            {
                var source = this;
                var target = instance as OfficialViewModel;

                source.SerializeWith(target);
                return target;
            }
            else if (instance is Official)
            {
                var source = this;
                var target = instance as Official;

                var session = IoC.Container.Resolve<ISessionProvider>().GetSharedSession();

                target.Id = source.Id;
                target.Person = (Person)source.Person.DeserializeInto(new Person());
                target.Position = source.Position != null ? session.Load<Position>(source.Position.Id) : null;
                target.Committee = source.Committee != null ? session.Load<Committee>(source.Committee.Id) : null;
                target.Picture.Image = source.Picture.ToImage();
                target.Signature.Image = source.Signature.ToImage();
                target.IsActive = source.IsActive;

                return target;
            }

            return null;
        }

        public OfficialViewModel()
        {
            this.Person = new PersonViewModel();
            this.Positions = new ReactiveList<PositionViewModel>();

            this.WhenAnyValue(x => x.Person.IsValid)
                .Subscribe(x => this.Revalidate());
        }
    }
}
