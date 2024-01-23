using CIS.Core.Entities.Barangays;
using CIS.UI.Features.Commons.Cameras;
using CIS.UI.Features.Commons.Signatures;
using CIS.UI.Utilities.Extentions;
using NHibernate.Exceptions;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents;

public class IncumbentController : ControllerBase<IncumbentViewModel>
{
    public IncumbentController(IncumbentViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.Load = new ReactiveCommand();
        this.ViewModel.Load.Subscribe(x => Load((Guid)x));
        this.ViewModel.Load.ThrownExceptions.Handle(this);

        this.ViewModel.CreateOfficial = new ReactiveCommand();
        this.ViewModel.CreateOfficial.Subscribe(x => CreateOfficial());
        this.ViewModel.CreateOfficial.ThrownExceptions.Handle(this);

        this.ViewModel.EditOfficial = new ReactiveCommand();
        this.ViewModel.EditOfficial.Subscribe(x => EditOfficial((OfficialViewModel)x));
        this.ViewModel.EditOfficial.ThrownExceptions.Handle(this);

        this.ViewModel.DeleteOfficial = new ReactiveCommand();
        this.ViewModel.DeleteOfficial.Subscribe(x => DeleteOfficial((OfficialViewModel)x));
        this.ViewModel.DeleteOfficial.ThrownExceptions.Handle(this);

        this.ViewModel.BatchSave = new ReactiveCommand(this.ViewModel.IsValidObservable());
        this.ViewModel.BatchSave.Subscribe(x => BatchSave());
        this.ViewModel.BatchSave.ThrownExceptions.Handle(this);
    }

    private IReactiveList<PositionViewModel> GetPositions()
    {
        var result = (IReactiveList<PositionViewModel>)null;

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var positions = session.Query<Position>()
                .FetchMany(x => x.Committees)
                .Cacheable()
                .ToList();

            result = positions
                .Select(x => new PositionViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Committees = x.Committees
                        .Select(o => new Lookup<string>()
                        {
                            Id = o.Id,
                            Name = o.Name
                        })
                        .ToReactiveList()
                })
                .ToReactiveList();

            transaction.Commit();
        }

        return result;
    }

    public virtual void CapturePicture(OfficialViewModel viewModel)
    {
        var dialog = new DialogService<CameraDialogViewModel>();
        dialog.ViewModel.Camera.Picture = viewModel.Picture;

        var result = dialog.ShowModal(this, "Camera", null);
        if (result == null)
            return;
        
        viewModel.Picture = result.Camera.Picture;
    }

    public virtual void CaptureSignature(OfficialViewModel viewModel)
    {
        var dialog = new DialogService<SignatureDialogViewModel>();
        dialog.ViewModel.Signature.SignatureImage = viewModel.Signature;

        var result = dialog.ShowModal(this, "Signature", null);
        if (result == null)
            return;

        viewModel.Signature = result.Signature.SignatureImage;
    }

    public virtual void Load(Guid id)
    {
        using var session = this.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var incumbentAlias = default(Incumbent);
        var officalAlias = default(Official);

        var query = session.QueryOver(() => incumbentAlias)
            .Left.JoinAlias(() => incumbentAlias.Officials, () => officalAlias)
            .Left.JoinQueryOver(() => officalAlias.Position)
            .Left.JoinQueryOver(() => officalAlias.Committee)
            .Where(() => incumbentAlias.Id == id)
            .FutureValue();

        var incumbent = query.Value;

        this.ViewModel.SerializeWith(incumbent);

        transaction.Commit();
    }

    public virtual void CreateOfficial()
    {
        var dialog = new DialogService<OfficialViewModel>();
        dialog.ViewModel.Positions = this.GetPositions();

        dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
        dialog.ViewModel.Save.Subscribe(x => InsertOfficial(dialog.ViewModel));
        dialog.ViewModel.Save.ThrownExceptions.Handle(this);

        dialog.ViewModel.CapturePicture = new ReactiveCommand();
        dialog.ViewModel.CapturePicture.Subscribe(x => CapturePicture(dialog.ViewModel));
        dialog.ViewModel.CapturePicture.ThrownExceptions.Handle(this);

        dialog.ViewModel.CaptureSignature = new ReactiveCommand();
        dialog.ViewModel.CaptureSignature.Subscribe(x => CaptureSignature(dialog.ViewModel));
        dialog.ViewModel.CaptureSignature.ThrownExceptions.Handle(this);

        dialog.ShowModal(this, "Create Official");
    }

    public virtual void InsertOfficial(OfficialViewModel item)
    {
        this.ViewModel.Officials.Add(item);
        item.Close();
    }

    public virtual void EditOfficial(OfficialViewModel item)
    {
        this.ViewModel.SelectedOfficial = item;
        this.ViewModel.SelectedOfficial.Positions = this.GetPositions();

        var dialog = new DialogService<OfficialViewModel>();
        dialog.ViewModel.Positions = this.GetPositions();

        dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
        dialog.ViewModel.Save.Subscribe(x => UpdateOfficial(dialog.ViewModel));
        dialog.ViewModel.Save.ThrownExceptions.Handle(this);

        dialog.ViewModel.CapturePicture = new ReactiveCommand();
        dialog.ViewModel.CapturePicture.Subscribe(x => CapturePicture(dialog.ViewModel));
        dialog.ViewModel.CapturePicture.ThrownExceptions.Handle(this);

        dialog.ViewModel.CaptureSignature = new ReactiveCommand();
        dialog.ViewModel.CaptureSignature.Subscribe(x => CaptureSignature(dialog.ViewModel));
        dialog.ViewModel.CaptureSignature.ThrownExceptions.Handle(this);

        dialog.ShowModal(this, "Edit Official", this.ViewModel.SelectedOfficial);
    }

    public virtual void UpdateOfficial(OfficialViewModel item)
    {
        this.ViewModel.SelectedOfficial.SerializeWith(item);
        item.Close();
    }

    public virtual void DeleteOfficial(OfficialViewModel item)
    {
        this.ViewModel.Officials.Remove(item);
        this.ViewModel.SelectedOfficial = null;
    }

    public virtual void BatchSave()
    {
        try
        {
            var message = string.Format("Do you want to save incubent?");
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var incumbent = default(Incumbent);

                if (this.ViewModel.Id == Guid.Empty)
                {
                    incumbent = new Incumbent();
                    session.Save(incumbent);
                }
                else
                {
                    var query = session.QueryOver<Incumbent>()
                        .Where(x => x.Id == this.ViewModel.Id)
                        .Fetch(x => x.Officials).Eager
                        .FutureValue();

                    incumbent = query.Value;
                }

                this.ViewModel.DeserializeInto(incumbent);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBox.Inform("Save has been successfully completed.");

            this.ViewModel.Close();
        }
        catch (GenericADOException)
        {
            throw new InvalidOperationException(string.Format("Unable to batch save. You may have deleted official that has already been referenced(data in use)."));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
