using CIS.Core.Entities.Barangays;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using System;
using System.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents;

public class IncumbentViewModel : ViewModelBase
{
    private readonly IncumbentController _controller;

    public virtual Guid Id { get; set; }

    public virtual DateTime? Date { get; set; }

    public virtual OfficialViewModel SelectedOfficial { get; set; }

    public virtual IReactiveList<OfficialViewModel> Officials { get; set; }

    public virtual IReactiveDerivedList<OfficialViewModel> Captains { get; set; }

    public virtual IReactiveDerivedList<OfficialViewModel> Councilors { get; set; }

    public virtual IReactiveDerivedList<OfficialViewModel> Secretaries { get; set; }

    public virtual IReactiveDerivedList<OfficialViewModel> Treasurers { get; set; }

    public virtual IReactiveDerivedList<OfficialViewModel> Kagawads { get; set; }

    public virtual IReactiveDerivedList<OfficialViewModel> SKChairmans { get; set; }

    public virtual IReactiveCommand Load { get; set; }

    public virtual IReactiveCommand CreateOfficial { get; set; }

    public virtual IReactiveCommand EditOfficial { get; set; }

    public virtual IReactiveCommand DeleteOfficial { get; set; }

    public virtual IReactiveCommand BatchSave { get; set; }

    public IncumbentViewModel()
    {
        this.WhenAnyValue(x => x.Officials)
            .Subscribe(x => InitializeDereivedList());

        this.Officials = new ReactiveList<OfficialViewModel>();

        _controller = IoC.Container.Resolve<IncumbentController>(new ViewModelDependency(this));
    }

    private void InitializeDereivedList()
    {
        if (this.Officials == null)
            return;

        this.Officials.ChangeTrackingEnabled = true;

        this.Captains = this.Officials.CreateDerivedCollection(x => x, x => x.Position.Id == Position.BarangayCaptain.Id);
        this.Councilors = this.Officials.CreateDerivedCollection(x => x, x => x.Position.Id == Position.BarangayCouncilor.Id);
        this.Secretaries = this.Officials.CreateDerivedCollection(x => x, x => x.Position.Id == Position.BarangaySecretary.Id);
        this.Treasurers = this.Officials.CreateDerivedCollection(x => x, x => x.Position.Id == Position.BarangayTreasurer.Id);
        this.Kagawads = this.Officials.CreateDerivedCollection(x => x, x => x.Position.Id == Position.Kagawad.Id);
        this.SKChairmans = this.Officials.CreateDerivedCollection(x => x, x => x.Position.Id == Position.SKChairman.Id);
    }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is IncumbentViewModel)
        {
            var source = instance as IncumbentViewModel;
            var target = this;

            target.Id = source.Id;
            target.Date = source.Date;
            target.Officials = new ReactiveList<OfficialViewModel>(source.Officials);

            return target;
        }
        else if (instance is Incumbent)
        {
            var source = instance as Incumbent;
            var target = this;

            target.Id = source.Id;
            target.Date = source.Date;
            target.Officials = source.Officials
                .Select(x => new OfficialViewModel().SerializeWith(x) as OfficialViewModel)
                .ToReactiveList();

            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (instance is IncumbentViewModel)
        {
            var source = this;
            var destination = instance as IncumbentViewModel;

            destination.SerializeWith(source);
            return destination;
        }
        else if (instance is Incumbent)
        {
            var source = this;
            var target = instance as Incumbent;

            target.Date = source.Date;
            target.Officials = source.Officials
                .Select(x => (Official)x.DeserializeInto(new Official()))
                .ToList();

            return target;
        }

        return null;
    }
}
