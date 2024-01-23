using CIS.Core.Entities.Barangays;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Cameras;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Features.Commons.Signatures;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace CIS.UI.Features.Barangays.Citizens;

public class CitizenController : ControllerBase<CitizenViewModel>
{
    public CitizenController(CitizenViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.CapturePicture = new ReactiveCommand();
        this.ViewModel.CapturePicture.Subscribe(x => CapturePicture((CitizenViewModel)this.ViewModel));
        this.ViewModel.CapturePicture.ThrownExceptions.Handle(this);

        this.ViewModel.CaptureSignature = new ReactiveCommand();
        this.ViewModel.CaptureSignature.Subscribe(x => CaptureSignature((CitizenViewModel)this.ViewModel));
        this.ViewModel.CaptureSignature.ThrownExceptions.Handle(this);

        this.ViewModel.CaptureFingerPrint = new ReactiveCommand();
        this.ViewModel.CaptureFingerPrint.Subscribe(x => CaptureFingerPrint((CitizenViewModel)this.ViewModel));
        this.ViewModel.CaptureFingerPrint.ThrownExceptions.Handle(this);

        this.ViewModel.Load = new ReactiveCommand();
        this.ViewModel.Load.Subscribe(x => Load((Guid)x));
        this.ViewModel.Load.ThrownExceptions.Handle(this);

        this.ViewModel.Save = new ReactiveCommand(this.ViewModel.IsValidObservable());
        this.ViewModel.Save.Subscribe(x => Save());
        this.ViewModel.Save.ThrownExceptions.Handle(this);

        this.ViewModel
          .WhenAnyValue(
              x => x.Person.Prefix,
              x => x.Person.FirstName,
              x => x.Person.MiddleName,
              x => x.Person.LastName,
              x => x.Person.Suffix,
              (
                  prefix,
                  firstName,
                  middleName,
                  lastName,
                  suffix
              ) => new PersonBasicViewModel()
              {
                  Prefix = prefix,
                  FirstName = firstName,
                  MiddleName = middleName,
                  LastName = lastName,
                  Suffix = suffix
              }
          )
          .Throttle(TimeSpan.FromSeconds(1.5))
          .Where(x =>
              !string.IsNullOrWhiteSpace(x.FirstName) &&
              !string.IsNullOrWhiteSpace(x.MiddleName) &&
              !string.IsNullOrWhiteSpace(x.LastName)
          )
          .Subscribe(x => CheckIfExistingApplicant(x));
    }

    private void CheckIfExistingApplicant(PersonBasicViewModel person)
    {
        using var session = this.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var query = session.Query<Citizen>()
            .Where(x =>
                x.Person.FirstName == this.ViewModel.Person.FirstName &&
                x.Person.MiddleName == this.ViewModel.Person.MiddleName &&
                x.Person.LastName == this.ViewModel.Person.LastName &&
                x.Person.Suffix == this.ViewModel.Person.Suffix
            )
            .Fetch(x => x.FingerPrint)
            .Fetch(x => x.Signatures)
            .ToFutureValue();

        session.Query<Citizen>()
            .Where(x =>
                x.Person.FirstName == this.ViewModel.Person.FirstName &&
                x.Person.MiddleName == this.ViewModel.Person.MiddleName &&
                x.Person.LastName == this.ViewModel.Person.LastName &&
                x.Person.Suffix == this.ViewModel.Person.Suffix
            )
            .Fetch(x => x.Pictures)
            .ToFutureValue();

        var citizen = query.Value;
        if (citizen != null)
            this.ViewModel.SerializeWith(citizen);

        transaction.Commit();
    }

    public virtual void CapturePicture(CitizenViewModel viewModel)
    {
        var dialog = new DialogService<CameraDialogViewModel>();
        dialog.ViewModel.Camera.Picture = viewModel.PictureImage;

        var result = dialog.ShowModal(this, "Camera", null);
        if (result == null)
            return;
        
        viewModel.PictureImage = result.Camera.Picture;
        viewModel.IsPictureImageChanged = true;
    }

    public virtual void CaptureSignature(CitizenViewModel viewModel)
    {
        var dialog = new DialogService<SignatureDialogViewModel>();
        dialog.ViewModel.Signature.SignatureImage = viewModel.SignatureImage;

        var result = dialog.ShowModal(this, "Signature", null);
        if (result == null)
            return;
        
        viewModel.SignatureImage = result.Signature.SignatureImage;
        viewModel.IsSignatureImageChanged = true;
    }

    public virtual void CaptureFingerPrint(CitizenViewModel viewModel)
    {
        var dialog = new DialogService<FingerScannerDialogViewModel>();
        dialog.ViewModel.FingerScanner.FingerImages = viewModel.FingerImages;

        var result = dialog.ShowModal(this, "Finger", null);
        if (result == null)
            return;
        
        viewModel.FingerImages = result.FingerScanner.FingerImages;
        viewModel.FingerImage = viewModel.FingerImages.First().Value;
        viewModel.IsFingerImageChanged = true;
    }

    public virtual void Load(Guid id)
    {
        using var session = this.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var query = session.Query<Citizen>()
            .Where(x => x.Id == id)
            .Fetch(x => x.FingerPrint)
            .Fetch(x => x.Signatures)
            .ToFutureValue();

        session.Query<Citizen>()
            .Where(x => x.Id == id)
            .Fetch(x => x.Pictures)
            .ToFutureValue();

        var citizen = query.Value;

        this.ViewModel.SerializeWith(citizen);

        transaction.Commit();
    }

    public virtual void Save()
    {
        var confirmed = this.MessageBox.Confirm("Do you want to save changes?.", "Save");
        if (confirmed == false)
            return;

        using var session = this.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var query = session.Query<Citizen>()
            .Where(x =>
                x.Person.FirstName == this.ViewModel.Person.FirstName &&
                x.Person.MiddleName == this.ViewModel.Person.MiddleName &&
                x.Person.LastName == this.ViewModel.Person.LastName &&
                x.Person.Suffix == this.ViewModel.Person.Suffix
            )
            .Fetch(x => x.FingerPrint)
            .Fetch(x => x.Signatures)
            .ToFutureValue();

        session.Query<Citizen>()
            .Where(x =>
                x.Person.FirstName == this.ViewModel.Person.FirstName &&
                x.Person.MiddleName == this.ViewModel.Person.MiddleName &&
                x.Person.LastName == this.ViewModel.Person.LastName &&
                x.Person.Suffix == this.ViewModel.Person.Suffix
            )
            .Fetch(x => x.Pictures)
            .ToFutureValue();

        var citizen = query.Value ?? new Citizen();
        this.ViewModel.DeserializeInto(citizen);

        session.SaveOrUpdate(citizen);
        transaction.Commit();

        this.ViewModel.Id = citizen.Id;

        //this.MessageBox.Inform("Save has been successfully completed.");
    }
}
