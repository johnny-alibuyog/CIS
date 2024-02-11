using System;
using System.Windows.Media.Imaging;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.Data;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Common.Person;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Maintenance.Officer;

public class OfficerViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    [Valid]
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
            target.Ranks = new ReactiveList<Lookup<string>>(source.Ranks);
            target.Position = source.Position;
            target.Signature = source.Signature;
            return target;
        }
        else if (instance is Core.Domain.Membership.Officer)
        {
            var source = instance as Core.Domain.Membership.Officer;
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

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (instance is OfficerViewModel)
        {
            var source = this;
            var target = instance as OfficerViewModel;

            source.SerializeWith(target);
            return target;
        }
        else if (instance is Core.Domain.Membership.Officer)
        {
            var source = this;
            var target = instance as Core.Domain.Membership.Officer;

            var session = IoC.Container.Resolve<ISessionProvider>().GetSharedSession();

            target.WithId(source.Id);
            target.Person = (Person)source.Person.DeserializeInto(new Person());
            target.Rank = session.Load<Core.Domain.Membership.Rank>(source.Rank.Id);
            target.Position = source.Position;
            target.Signature.Image = source.Signature.ToImage();

            return target;
        }

        return null;
    }

    public OfficerViewModel()
    {
        this.Person = new PersonViewModel(); // 
        this.Ranks = new ReactiveList<Lookup<string>>();

        this.WhenAnyValue(x => x.Person.IsValid)
            .Subscribe(x => this.Revalidate());
    }
}
