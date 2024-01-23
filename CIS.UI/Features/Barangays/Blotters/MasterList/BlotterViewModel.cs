using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.Data;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace CIS.UI.Features.Barangays.Blotters.MasterList;

public class BlotterViewModel : ViewModelBase
{
    private readonly BlotterController _controller;

    public virtual Guid Id { get; set; }

    public virtual string Complaint { get; set; }

    public virtual string Details { get; set; }

    public virtual string Remarks { get; set; }

    public virtual BlotterStatus? Status { get; set; }

    public virtual DateTime? FiledOn { get; set; }

    public virtual DateTime? OccuredOn { get; set; }

    public virtual AddressViewModel Address { get; set; }

    public virtual Guid IncumbentId { get; set; }

    public virtual IReactiveList<BlotterOfficialViewModel> Officials { get; set; }

    public virtual IReactiveList<BlotterCitizenViewModel> Complainants { get; set; }

    public virtual IReactiveList<BlotterCitizenViewModel> Respondents { get; set; }

    public virtual IReactiveList<BlotterCitizenViewModel> Witnesses { get; set; }

    public virtual BlotterOfficialViewModel SelectedOfficial { get; set; }

    public virtual BlotterCitizenViewModel SelectedComplainant { get; set; }

    public virtual BlotterCitizenViewModel SelectedRespondent { get; set; }

    public virtual BlotterCitizenViewModel SelectedWitness { get; set; }

    public virtual IReactiveCommand CreateComplainant { get; set; }

    public virtual IReactiveCommand EditComplainant { get; set; }

    public virtual IReactiveCommand DeleteComplainant { get; set; }

    public virtual IReactiveCommand CreateRespondent { get; set; }

    public virtual IReactiveCommand EditRespondent { get; set; }

    public virtual IReactiveCommand DeleteRespondent { get; set; }

    public virtual IReactiveCommand CreateWitness { get; set; }

    public virtual IReactiveCommand EditWitness { get; set; }

    public virtual IReactiveCommand DeleteWitness { get; set; }

    public virtual IReactiveCommand Load { get; set; }

    public virtual IReactiveCommand Save { get; set; }

    public BlotterViewModel()
    {
        this.Status = BlotterStatus.Open;
        this.FiledOn = DateTime.Today;
        this.Address = new AddressViewModel();
        this.Officials = new ReactiveList<BlotterOfficialViewModel>();
        this.Complainants = new ReactiveList<BlotterCitizenViewModel>();
        this.Respondents = new ReactiveList<BlotterCitizenViewModel>();
        this.Witnesses = new ReactiveList<BlotterCitizenViewModel>();

        this.WhenAnyValue(x => x.SelectedOfficial)
            .Where(x => x != null)
            .Subscribe(x => x.Selected = !x.Selected);

        _controller = IoC.Container.Resolve<BlotterController>(new ViewModelDependency(this));
    }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is BlotterViewModel)
        {
            var source = instance as BlotterViewModel;
            var target = this;

            var officialIds = source.Officials.Where(x => x.Selected).Select(x => x.Id);

            target.Id = source.Id;
            target.Complaint = source.Complaint;
            target.Details = source.Details;
            target.Remarks = source.Remarks;
            target.Remarks = source.Remarks;
            target.Status = source.Status;
            target.FiledOn = source.FiledOn;
            target.OccuredOn = source.OccuredOn;
            target.Address.SerializeWith(source.Address);
            target.IncumbentId = source.IncumbentId;
            target.Officials.ForEach(item => item.Selected = officialIds.Contains(item.Id));
            target.Complainants = new ReactiveList<BlotterCitizenViewModel>(source.Complainants);
            target.Respondents = new ReactiveList<BlotterCitizenViewModel>(source.Respondents);
            target.Witnesses = new ReactiveList<BlotterCitizenViewModel>(source.Witnesses);

            return target;
        }
        else if (instance is Blotter)
        {
            var source = instance as Blotter;
            var target = this;

            var officialIds = source.Officials.Select(x => x.Id);

            target.Id = source.Id;
            target.Complaint = source.Complaint;
            target.Details = source.Details;
            target.Remarks = source.Remarks;
            target.Remarks = source.Remarks;
            target.Status = source.Status;
            target.FiledOn = source.FiledOn;
            target.OccuredOn = source.OccuredOn;
            target.Address.SerializeWith(source.Address);
            target.Officials.ForEach(item => item.Selected = officialIds.Contains(item.Id));
            target.Complainants = source.Complainants
                .Select(x => new BlotterCitizenViewModel()
                {
                    Id = x.Id,
                    Name = x.Person.Fullname,
                    Gender = x.Person.Gender
                })
                .ToReactiveList();
            target.Respondents = source.Respondents
                .Select(x => new BlotterCitizenViewModel()
                {
                    Id = x.Id,
                    Name = x.Person.Fullname,
                    Gender = x.Person.Gender
                })
                .ToReactiveList();
            target.Witnesses = source.Witnesses
                .Select(x => new BlotterCitizenViewModel()
                {
                    Id = x.Id,
                    Name = x.Person.Fullname,
                    Gender = x.Person.Gender
                })
                .ToReactiveList();

            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (instance is BlotterViewModel)
        {
            var source = this;
            var destination = instance as BlotterViewModel;

            destination.SerializeWith(source);
            return destination;
        }
        else if (instance is Blotter)
        {
            var source = this;
            var target = instance as Blotter;

            var session = IoC.Container.Resolve<ISessionProvider>().GetSharedSession();

            var officialIds = source.Officials.Where(x => x.Selected).Select(x => x.Id).ToArray();
            var complainantIds = source.Complainants.Select(x => x.Id).ToArray();
            var respondentIds = source.Respondents.Select(x => x.Id).ToArray();
            var witnessIds = source.Witnesses.Select(x => x.Id).ToArray();

            var officialQuery = session.Query<Official>()
                .Where(x => officialIds.Contains(x.Id))
                .Fetch(x => x.Committee)
                .Fetch(x => x.Position)
                .ToFuture();

            var complainantQuery = session.Query<Citizen>()
                .Where(x => complainantIds.Contains(x.Id))
                .ToFuture();

            var respondentQuery = session.Query<Citizen>()
                .Where(x => respondentIds.Contains(x.Id))
                .ToFuture();

            var witnessQuery = session.Query<Citizen>()
                .Where(x => witnessIds.Contains(x.Id))
                .ToFuture();

            //target.Id = source.Id;
            target.Complaint = source.Complaint;
            target.Details = source.Details;
            target.Remarks = source.Remarks;
            target.Remarks = source.Remarks;
            target.Status = source.Status;
            target.FiledOn = source.FiledOn;
            target.OccuredOn = source.OccuredOn;
            target.Address = source.Address.DeserializeInto(new Address()) as Address;
            target.Officials = officialQuery;
            target.Complainants = complainantQuery;
            target.Respondents = respondentQuery;
            target.Witnesses = witnessQuery;

            return target;
        }

        return null;
    }
}
