using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Security;
using CIS.Core.Domain.Membership;
using CIS.Data.Common.Exception;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Features.Common.Biometrics;
using CIS.UI.Features.Common.Camera;
using CIS.UI.Features.Common.Person;
using CIS.UI.Features.Common.Signature;
using CIS.UI.Features.Security.Users.Approvals;
using CIS.UI.Features.Membership.Maintenance;
using CIS.UI.Utilities.Extentions;
using CIS.UI.Utilities.Reports;
using Microsoft.Reporting.WinForms;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

[HandleError]
public class ApplicationController : ControllerBase<ApplicationViewModel>
{
    private enum Direction { Previous, Next }

    public ApplicationController(ApplicationViewModel viewModel)
        : base(viewModel)
    {
        this.Reset();

        this.MessageBus.Listen<MaintenanceMessage>().Subscribe(x =>
        {
            if (x.Identifier == "Setting")
                Reset();
            else
                PopulateLookups();
        });

        this.ViewModel.Previous = new ReactiveCommand(
            this.ViewModel.WhenAny(
                x => x.CurrentViewModel,
                x => x.CurrentViewModel.IsValid,
                (current, isValid) =>
                {
                    if (current.Value == this.ViewModel.ViewModels.First())
                        return false;

                    return true;
                }
            )
        );
        this.ViewModel.Previous.Subscribe(x => Previous());
        this.ViewModel.Previous.ThrownExceptions.Handle(this);

        this.ViewModel.Next = new ReactiveCommand(
            this.ViewModel.WhenAny(
                x => x.CurrentViewModel,
                x => x.CurrentViewModel.IsValid,
                (current, isValid) =>
                {
                    if (current.Value == this.ViewModel.ViewModels.Last())
                        return false;

                    if (isValid.Value != true)
                        return false;

                    return true;
                }
            )
        );
        this.ViewModel.Next.Subscribe(x => Next());
        this.ViewModel.Next.ThrownExceptions.Handle(this);

        this.ViewModel.Reset = new ReactiveCommand();
        this.ViewModel.Reset.Subscribe(x => Reset());
        this.ViewModel.Reset.ThrownExceptions.Handle(this);

        this.ViewModel.Release = new ReactiveCommand(
            this.ViewModel.WhenAny(
                x => x.CurrentViewModel,
                x => x.CurrentViewModel.IsValid,
                (current, isValid) =>
                {
                    if (current.Value != this.ViewModel.Summary)
                        return false;

                    if (isValid.Value != true)
                        return false;

                    return true;
                })
            );
        this.ViewModel.Release.Subscribe(x => Release());
        this.ViewModel.Release.ThrownExceptions.Handle(this);

        this.ViewModel
            .WhenAnyValue(
                x => x.PersonalInformation.Person.Prefix,
                x => x.PersonalInformation.Person.FirstName,
                x => x.PersonalInformation.Person.MiddleName,
                x => x.PersonalInformation.Person.LastName,
                x => x.PersonalInformation.Person.Suffix,
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
            .Subscribe(CheckIfExistingApplicant);
    }

    #region Routine Helpers

    private bool ValidateScreen()
    {
        if (this.ViewModel.CurrentViewModel.IsValid == false)
        {
            this.MessageBox.Warn(this.ViewModel.CurrentViewModel.Error, "Application");
            return false;
        }

        if (this.ViewModel.PersonalInformation == this.ViewModel.CurrentViewModel)
        {
            if (this.ViewModel.PersonalInformation.Person.MiddleName == null)
                throw new BusinessException("Middle name is mandatory.");

            if (this.ViewModel.PersonalInformation.Person.BirthDate == null)
                throw new BusinessException("Birth date name is mandatory.");

            if (this.ViewModel.PersonalInformation.Person.Gender == null)
                throw new BusinessException("Gender is mandatory.");
        }

        if (this.ViewModel.Finding == this.ViewModel.CurrentViewModel)
        {
            if (this.ViewModel.Finding.Amendment != null)
            {
                var dialog = new DialogService<ApprovalViewModel>();
                dialog.ViewModel.Roles = [Role.PoliceApprover];
                var result = dialog.ShowModal();
                if (result == null)
                    return false;

                this.ViewModel.Finding.Amendment.ApproverUserId = result.UserId;
                this.ViewModel.Summary.FinalFindings = this.ViewModel.Finding.Evaluate();
            }
        }
        return true;
    }

    private int GetMovementPosition(Direction direction)
    {
        var movement = 1;
        var currentIndex = this.ViewModel.ViewModels.IndexOf(this.ViewModel.CurrentViewModel);
        var targetViewModel = direction == Direction.Next
            ? this.ViewModel.ViewModels[currentIndex + 1]
            : this.ViewModel.ViewModels[currentIndex - 1];

        if (this.ViewModel.Finding == targetViewModel)
        {
            if (direction == Direction.Next)
                this.Evaluate();

            if (this.ViewModel.Finding.Hits.Count() == 0)
                movement = 2;
        }

        if (this.ViewModel.Summary == targetViewModel)
        {
            this.ViewModel.Summary.FinalFindings = this.ViewModel.Finding.Evaluate();
        }

        return direction == Direction.Next
            ? currentIndex + movement
            : currentIndex - movement;
    }

    private void InitializeScreen(Direction direction)
    {
        if (this.ViewModel.Camera == this.ViewModel.CurrentViewModel)
            this.ViewModel.Camera.Start.Execute(null);
        else
            this.ViewModel.Camera.Stop.Execute(null);

        if (this.ViewModel.FingerScanner == this.ViewModel.CurrentViewModel)
            this.ViewModel.FingerScanner.Start.Execute(null);
        else
            this.ViewModel.FingerScanner.Stop.Execute(null);
    }

    private void InitializeViews()
    {
        this.ViewModel.PersonalInformation = new PersonalInformationViewModel();
        this.ViewModel.OtherInformation = new OtherInformationViewModel();
        this.ViewModel.Camera = new CameraViewModel();
        this.ViewModel.FingerScanner = new FingerScannerViewModel();
        this.ViewModel.Signature = new SignatureViewModel();
        this.ViewModel.Finding = new FindingViewModel();
        this.ViewModel.Summary = new SummaryViewModel();
        this.ViewModel.ViewModels = new List<ViewModelBase>();

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var query = session.Query<Setting>()
                .Where(x => x.Terminal.MachineName == Environment.MachineName)
                .Cacheable()
                .FirstOrDefault();

            var setting = query;

            this.ViewModel.ViewModels.Add(this.ViewModel.PersonalInformation);
            this.ViewModel.ViewModels.Add(this.ViewModel.OtherInformation);

            if (setting.WithCameraDevice)
                this.ViewModel.ViewModels.Add(this.ViewModel.Camera);

            if (setting.WithFingerScannerDevice)
                this.ViewModel.ViewModels.Add(this.ViewModel.FingerScanner);

            if (setting.WithDigitalSignatureDevice)
                this.ViewModel.ViewModels.Add(this.ViewModel.Signature);

            this.ViewModel.ViewModels.Add(this.ViewModel.Finding);
            this.ViewModel.ViewModels.Add(this.ViewModel.Summary);

            transaction.Commit();
        }

        this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels.First();
    }

    private void PopulateLookups()
    {
        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var purposeQuery = session.Query<Purpose>().Cacheable().ToList();
            var officerQuery = session.Query<Officer>().Cacheable().ToList();
            var stationQuery = session.Query<Station>().Cacheable().ToList();
            var settingQuery = session.Query<Setting>()
                .Where(x => x.Terminal.MachineName == Environment.MachineName)
                .Fetch(x => x.CurrentVerifier)
                .Fetch(x => x.CurrentCertifier)
                .Cacheable()
                .FirstOrDefault();

            this.ViewModel.PersonalInformation.Purposes = purposeQuery
                .Select(x => new Lookup<Guid>(x.Id, x.Name))
                .ToReactiveList();

            this.ViewModel.PersonalInformation.Verifiers = officerQuery
                .Select(x => new Lookup<Guid>(x.Id, x.Person.Fullname))
                .ToReactiveList();

            this.ViewModel.PersonalInformation.Certifiers = officerQuery
                .Select(x => new Lookup<Guid>(x.Id, x.Person.Fullname))
                .ToReactiveList();

            var setting = settingQuery;
            if (setting != null)
            {
                if (setting.CurrentVerifier != null)
                    this.ViewModel.PersonalInformation.Verifier = this.ViewModel.PersonalInformation.Verifiers
                        .FirstOrDefault(x => x.Id == setting.CurrentVerifier.Id);

                if (setting.CurrentCertifier != null)
                    this.ViewModel.PersonalInformation.Certifier = this.ViewModel.PersonalInformation.Certifiers
                        .FirstOrDefault(x => x.Id == setting.CurrentCertifier.Id);
            }

            var station = stationQuery.FirstOrDefault();
            if (station.ClearanceFee == null || station.ClearanceFee <= 0M)
                station.ClearanceFee = 100.00M;

            this.ViewModel.Summary.ClearanceFee = station.ClearanceFee;
            this.ViewModel.Summary.ClearanceValidityInDays = station.ClearanceValidityInDays;
            this.ViewModel.PersonalInformation.Address.City = station.Address.City;
            this.ViewModel.PersonalInformation.Address.Province = station.Address.Province;
            this.ViewModel.OtherInformation.ProvincialAddress.City = station.Address.City;
            this.ViewModel.OtherInformation.ProvincialAddress.Province = station.Address.Province;

            transaction.Commit();
        }
    }

    private void CheckIfExistingApplicant(PersonBasicViewModel person)
    {
        var applicant = default(Core.Domain.Membership.Application);
        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var applicantAlias = default(Core.Domain.Membership.Application);
            var fingerPrintAlias = default(FingerPrint);

            var applicantQuery = session.QueryOver(() => applicantAlias)
                .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
                .Left.JoinQueryOver(() => applicantAlias.Relatives)
                .Left.JoinQueryOver(() => applicantAlias.Signatures)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightThumb)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightIndex)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightMiddle)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightRing)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightPinky)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftThumb)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftIndex)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftMiddle)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftRing)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftPinky)
                .Where(() =>
                    applicantAlias.Person.FirstName == person.FirstName &&
                    applicantAlias.Person.MiddleName == person.MiddleName &&
                    applicantAlias.Person.LastName == person.LastName &&
                    applicantAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch pictures
            session.QueryOver(() => applicantAlias)
                .Left.JoinQueryOver(() => applicantAlias.Pictures)
                .Where(() =>
                    applicantAlias.Person.FirstName == person.FirstName &&
                    applicantAlias.Person.MiddleName == person.MiddleName &&
                    applicantAlias.Person.LastName == person.LastName &&
                    applicantAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch signatures
            session.QueryOver(() => applicantAlias)
                .Left.JoinQueryOver(() => applicantAlias.Signatures)
                .Where(() =>
                    applicantAlias.Person.FirstName == person.FirstName &&
                    applicantAlias.Person.MiddleName == person.MiddleName &&
                    applicantAlias.Person.LastName == person.LastName &&
                    applicantAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            applicant = applicantQuery.Value;

            transaction.Commit();
        }

        if (applicant != null)
        {
            //this.MessageBox.Inform("Application has an existing record.\nPlease update with latest data.");

            this.ViewModel.PersonalInformation.Person.Gender = applicant.Person.Gender;
            this.ViewModel.PersonalInformation.Person.BirthDate = applicant.Person.BirthDate;
            this.ViewModel.PersonalInformation.Person.BirthPlace = applicant.Person.BirthPlace;
            this.ViewModel.PersonalInformation.Address.SerializeWith(applicant.Address);
            this.ViewModel.PersonalInformation.Height = applicant.Height;
            this.ViewModel.PersonalInformation.Weight = applicant.Weight;
            this.ViewModel.PersonalInformation.Build = applicant.Build;
            this.ViewModel.PersonalInformation.Marks = applicant.Marks;
            this.ViewModel.PersonalInformation.AlsoKnownAs = applicant.AlsoKnownAs;
            this.ViewModel.PersonalInformation.Occupation = applicant.Occupation;
            this.ViewModel.PersonalInformation.Religion = applicant.Religion;
            this.ViewModel.PersonalInformation.Citizenship = applicant.Citizenship;
            this.ViewModel.PersonalInformation.CivilStatus = applicant.CivilStatus;
            this.ViewModel.OtherInformation.Mother.SerializeWith(applicant.Mother);
            this.ViewModel.OtherInformation.Father.SerializeWith(applicant.Father);
            this.ViewModel.OtherInformation.Relatives = applicant.Relatives.Select(x => new PersonBasicViewModel(x)).ToReactiveList();
            this.ViewModel.OtherInformation.ProvincialAddress.SerializeWith(applicant.ProvincialAddress);
            this.ViewModel.OtherInformation.EmailAddress = applicant.EmailAddress;
            this.ViewModel.OtherInformation.TelephoneNumber = applicant.TelephoneNumber;
            this.ViewModel.OtherInformation.CellphoneNumber = applicant.CellphoneNumber;
            this.ViewModel.OtherInformation.PassportNumber = applicant.PassportNumber;
            this.ViewModel.OtherInformation.TaxIdentificationNumber = applicant.TaxIdentificationNumber;
            this.ViewModel.OtherInformation.SocialSecuritySystemNumber = applicant.SocialSecuritySystemNumber;
            this.ViewModel.Camera.Picture = applicant.Pictures.Count() > 0 ? applicant.Pictures.Last().Image.ToBitmapSource() : null;
            this.ViewModel.Signature.SignatureImage = applicant.Signatures.Count() > 0 ? applicant.Signatures.Last().Image.ToBitmapSource() : null;
            this.ViewModel.FingerScanner.CapturedFingerImage = applicant.FingerPrint.RightThumb.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb] = applicant.FingerPrint.RightThumb.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightIndex] = applicant.FingerPrint.RightIndex.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightMiddle] = applicant.FingerPrint.RightMiddle.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightRing] = applicant.FingerPrint.RightRing.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightPinky] = applicant.FingerPrint.RightPinky.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftThumb] = applicant.FingerPrint.LeftThumb.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftIndex] = applicant.FingerPrint.LeftIndex.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftMiddle] = applicant.FingerPrint.LeftMiddle.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftRing] = applicant.FingerPrint.LeftRing.Image.ToBitmapSource();
            this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftPinky] = applicant.FingerPrint.LeftPinky.Image.ToBitmapSource();
        }
    }

    private void Evaluate()
    {
        this.ViewModel.Finding.Amendment = null;
        this.ViewModel.Finding.Hits.Clear();

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var person = this.ViewModel.PersonalInformation.Person;

            var matchingSuspectQuery = session.Query<Suspect>()
                .Where(x =>
                    x.Person.FirstName == person.FirstName &&
                    x.Person.LastName == person.LastName &&
                    (
                        x.Person.MiddleName == null ||
                        x.Person.MiddleName == string.Empty ||
                        (
                            x.Person.MiddleName.Length == 1 || x.Person.MiddleName.Contains(".")
                                ? x.Person.MiddleName.Substring(0, 1) == person.MiddleName.Substring(0, 1)
                                : x.Person.MiddleName == (person.MiddleName ?? string.Empty)
                        )
                    )
                )
                .Fetch(x => x.Warrant)
                .ToFuture();

            foreach (var item in matchingSuspectQuery)
            {
                var hit = new SuspectHitViewModel();
                hit.SuspectId = item.Id;
                hit.Applicant.SerializeWith(this.ViewModel.PersonalInformation.Person);
                hit.Suspect.SerializeWith(item.Person);
                hit.WarrantCode = item.Warrant.WarrantCode;
                hit.CaseNumber = item.Warrant.CaseNumber;
                hit.Crime = item.Warrant.Crime;
                hit.Description = item.Warrant.Description;
                hit.Remarks = item.Warrant.Remarks;
                hit.BailAmount = item.Warrant.BailAmount.ToString("#,##0.00");
                hit.IssuedBy = item.Warrant.IssuedBy;
                hit.IssuedAt.SerializeWith(item.Warrant.IssuedAt);
                hit.IssuedOn = item.Warrant.IssuedOn;

                this.ViewModel.Finding.Hits.Add(hit);
            }

            transaction.Commit();
        }

        this.ViewModel.Finding.SelectedHit = this.ViewModel.Finding.Hits.FirstOrDefault();

        this.ViewModel.Summary.FinalFindings = this.ViewModel.Finding.Evaluate();
        this.ViewModel.Summary.Picture = this.ViewModel.Camera.Picture;
        this.ViewModel.Summary.RightThumb = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb];
        this.ViewModel.Summary.Address = this.ViewModel.PersonalInformation.Address.ToString();
        this.ViewModel.Summary.FullName = this.ViewModel.PersonalInformation.Person.FullName;
        this.ViewModel.Summary.BirthDate = this.ViewModel.PersonalInformation.Person.BirthDate;
        this.ViewModel.Summary.BirthPlace = this.ViewModel.PersonalInformation.BirthPlace;
    }

    private RegistrationIdCardReportViewModel GenerateClearance()
    {
        var result = new RegistrationIdCardReportViewModel();

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var person = this.ViewModel.PersonalInformation.Person;

            var registrationAlias = default(Core.Domain.Membership.Registration);
            var applicantAlias = default(Core.Domain.Membership.Application);
            var fingerPrintAlias = default(FingerPrint);
            var stationAlias = default(Station);
            var barcodeAlais = default(Barcode);

            var isNewApplicant = false;

            var applicantQuery = session.QueryOver(() => applicantAlias)
                .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
                .Left.JoinAlias(() => applicantAlias.Registrations, () => registrationAlias)
                .Left.JoinAlias(() => registrationAlias.Station, () => stationAlias)
                .Left.JoinAlias(() => registrationAlias.Barcode, () => barcodeAlais)
                .Left.JoinQueryOver(() => stationAlias.Logo)
                .Left.JoinQueryOver(() => barcodeAlais.Image)
                .Left.JoinQueryOver(() => registrationAlias.ApplicantPicture)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightThumb)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightIndex)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightMiddle)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightRing)
                .Left.JoinQueryOver(() => fingerPrintAlias.RightPinky)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftThumb)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftIndex)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftMiddle)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftRing)
                .Left.JoinQueryOver(() => fingerPrintAlias.LeftPinky)
                .Where(() =>
                    applicantAlias.Person.FirstName == person.FirstName &&
                    applicantAlias.Person.MiddleName == person.MiddleName &&
                    applicantAlias.Person.LastName == person.LastName &&
                    applicantAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch relatives
            session.QueryOver(() => applicantAlias)
                .Left.JoinQueryOver(x => x.Relatives)
                .Where(() =>
                    applicantAlias.Person.FirstName == person.FirstName &&
                    applicantAlias.Person.MiddleName == person.MiddleName &&
                    applicantAlias.Person.LastName == person.LastName &&
                    applicantAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch pictures
            session.QueryOver(() => applicantAlias)
                .Left.JoinQueryOver(x => x.Pictures)
                .Where(() =>
                    applicantAlias.Person.FirstName == person.FirstName &&
                    applicantAlias.Person.MiddleName == person.MiddleName &&
                    applicantAlias.Person.LastName == person.LastName &&
                    applicantAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch signatures
            session.QueryOver(() => applicantAlias)
                .Left.JoinQueryOver(x => x.Signatures)
                .Where(() =>
                    applicantAlias.Person.FirstName == person.FirstName &&
                    applicantAlias.Person.MiddleName == person.MiddleName &&
                    applicantAlias.Person.LastName == person.LastName &&
                    applicantAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            var stationQuery = session.QueryOver<Station>()
                .Left.JoinQueryOver(x => x.Logo)
                .Cacheable()
                .List();

            var officerAlias = (Officer)null;
            var rankAlias = (Rank)null;

            var certifier = session.QueryOver(() => officerAlias)
                .Left.JoinAlias(() => officerAlias.Rank, () => rankAlias)
                .Where(() => officerAlias.Id == this.ViewModel.PersonalInformation.Certifier.Id)
                .Cacheable()
                .SingleOrDefault();

            var verifier = session.QueryOver(() => officerAlias)
                .Left.JoinAlias(() => officerAlias.Rank, () => rankAlias)
                .Where(() => officerAlias.Id == this.ViewModel.PersonalInformation.Verifier.Id)
                .Cacheable()
                .SingleOrDefault();

            var settings = session.Query<Setting>()
                .Where(x => x.Terminal.MachineName == Environment.MachineName)
                .Fetch(x => x.CurrentVerifier)
                .Fetch(x => x.CurrentCertifier)
                .Cacheable()
                .ToList();


            var setting = settings.FirstOrDefault();
            if (setting != null)
            {
                setting.CurrentVerifier = verifier;
                setting.CurrentCertifier = certifier;
            }

            var applicant = applicantQuery.Value;
            if (applicant == null)
            {
                isNewApplicant = true;
                applicant = new Core.Domain.Membership.Application();
            }

            var clearance = applicant.Registrations.FirstOrDefault(x => x.IssueDate == DateTime.Today);
            if (clearance == null)
            {
                clearance = new Core.Domain.Membership.Registration();
                applicant.AddClearance(clearance);
            }

            var picture = new ImageBlob(this.ViewModel.Camera.Picture.ToImage());
            var signature = new ImageBlob(this.ViewModel.Signature.SignatureImage.ToImage());

            applicant.Person = (Person)this.ViewModel.PersonalInformation.Person.DeserializeInto(new Person());
            applicant.Address = (Address)this.ViewModel.PersonalInformation.Address.DeserializeInto(new Address());
            applicant.Height = this.ViewModel.PersonalInformation.Height;
            applicant.Weight = this.ViewModel.PersonalInformation.Weight;
            applicant.Build = this.ViewModel.PersonalInformation.Build;
            applicant.Marks = this.ViewModel.PersonalInformation.Marks;
            applicant.AlsoKnownAs = this.ViewModel.PersonalInformation.AlsoKnownAs;
            applicant.Occupation = this.ViewModel.PersonalInformation.Occupation;
            applicant.Religion = this.ViewModel.PersonalInformation.Religion;
            applicant.Citizenship = this.ViewModel.PersonalInformation.Citizenship;
            applicant.CivilStatus = this.ViewModel.PersonalInformation.CivilStatus;
            applicant.Mother = (PersonBasic)this.ViewModel.OtherInformation.Mother.DeserializeInto(new PersonBasic());
            applicant.Father = (PersonBasic)this.ViewModel.OtherInformation.Father.DeserializeInto(new PersonBasic());
            applicant.Relatives = this.ViewModel.OtherInformation.Relatives.Select(x => x.DeserializeInto(new PersonBasic()) as PersonBasic);
            applicant.ProvincialAddress = (Address)this.ViewModel.OtherInformation.ProvincialAddress.DeserializeInto(new Address());
            applicant.EmailAddress = this.ViewModel.OtherInformation.EmailAddress;
            applicant.TelephoneNumber = this.ViewModel.OtherInformation.TelephoneNumber;
            applicant.CellphoneNumber = this.ViewModel.OtherInformation.CellphoneNumber;
            applicant.PassportNumber = this.ViewModel.OtherInformation.PassportNumber;
            applicant.TaxIdentificationNumber = this.ViewModel.OtherInformation.TaxIdentificationNumber;
            applicant.SocialSecuritySystemNumber = this.ViewModel.OtherInformation.SocialSecuritySystemNumber;
            applicant.AddPicture(picture);
            applicant.AddSignature(signature);
            applicant.FingerPrint.RightThumb.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb].ToImage();
            applicant.FingerPrint.RightIndex.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightIndex].ToImage();
            applicant.FingerPrint.RightMiddle.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightMiddle].ToImage();
            applicant.FingerPrint.RightRing.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightRing].ToImage();
            applicant.FingerPrint.RightPinky.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightPinky].ToImage();
            applicant.FingerPrint.LeftThumb.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftThumb].ToImage();
            applicant.FingerPrint.LeftIndex.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftIndex].ToImage();
            applicant.FingerPrint.LeftMiddle.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftMiddle].ToImage();
            applicant.FingerPrint.LeftRing.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftRing].ToImage();
            applicant.FingerPrint.LeftPinky.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftPinky].ToImage();

            // this fields will/may change every clearnce application. denormilize this fields
            clearance.ApplicantPicture = picture;
            clearance.ApplicantSignature = signature;
            clearance.ApplicantCivilStatus = applicant.CivilStatus;
            clearance.ApplicantAddress = applicant.Address.ToString();
            clearance.ApplicantCitizenship = applicant.Citizenship;

            clearance.SetVerifier(verifier);
            clearance.SetCertifier(certifier);
            clearance.SetStation(stationQuery.FirstOrDefault());
            clearance.ApplicationDate = this.ViewModel.Summary.ApplicationDate;
            clearance.IssueDate = this.ViewModel.Summary.IssuedDate;
            clearance.Validity = this.ViewModel.Summary.Validity;
            clearance.ControlNumber = this.ViewModel.Summary.ControlNumber;
            clearance.OfficialReceiptNumber = this.ViewModel.Summary.OfficialReceiptNumber;
            clearance.TaxCertificateNumber = this.ViewModel.Summary.TaxCertificateNumber;
            clearance.Fee = this.ViewModel.Summary.ClearanceFee;
            clearance.YearsResident = this.ViewModel.Summary.YearsOfResidency;
            clearance.FinalFindings = this.ViewModel.Summary.FinalFindings;
            clearance.Purpose = session.Get<Purpose>(this.ViewModel.PersonalInformation.Purpose.Id);

            if (this.ViewModel.Finding.HasHits)
            {
                clearance.Finding = new Finding();
                clearance.Finding.FinalFindings = this.ViewModel.Summary.FinalFindings;

                // hits
                clearance.Finding.Hits = this.ViewModel.Finding.Hits
                    .OfType<SuspectHitViewModel>()
                    .Select(x => new SuspectHit()
                    {
                        HitScore = x.HitScore,
                        IsIdentical = x.IsIdentifiedHit,
                        Suspect = session.Load<Suspect>(x.SuspectId)
                    })
                    .AsEnumerable<Hit>()
                    .ToList();

                // amendment
                if (this.ViewModel.Finding.HasAmendments)
                {
                    clearance.Finding.Amendment = new Amendment()
                    {
                        Approver = session.Load<User>(this.ViewModel.Finding.Amendment.ApproverUserId),
                        DocumentNumber = this.ViewModel.Finding.Amendment.DocumentNumber,
                        Reason = this.ViewModel.Finding.Amendment.Reason,
                        Remarks = this.ViewModel.Finding.Amendment.Remarks
                    };
                }
            }
            else
            {
                clearance.Finding = null;
            }

            if (isNewApplicant)
                session.Save(applicant);

            transaction.Commit();

            result.SerializeWith(clearance);
        }

        return result;
    }

    private void PrintClearance(RegistrationIdCardReportViewModel data)
    {
        var report = new LocalReport();
        report.EnableExternalImages = true;
        report.ReportEmbeddedResource = "CIS.UI.Features.Membership.Registration.Applications.RegistrationReport.rdlc";
        report.DataSources.Add(new ReportDataSource()
        {
            Name = "ItemDataSet",
            Value = new BindingSource() { DataSource = new object[] { data } }
        });
        var print = new ReportPrintDocument(report);
        print.Print();
    }

    #endregion

    public virtual void Reset()
    {
        this.ViewModel.Camera?.Stop.Execute(null);
        this.ViewModel.FingerScanner?.Stop.Execute(null);

        InitializeViews();
        PopulateLookups();
    }

    public virtual void Previous()
    {
        var position = this.GetMovementPosition(Direction.Previous);
        this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels[position];
        this.InitializeScreen(Direction.Previous);
    }

    public virtual void Next()
    {
        if (this.ValidateScreen() == false)
            return;

        var position = this.GetMovementPosition(Direction.Next);
        this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels[position];
        this.InitializeScreen(Direction.Next);
    }

    public virtual void Release()
    {
        var confirmed = this.MessageBox.Confirm("Do you want to release clearance.", "Registration");
        if (confirmed == false)
            return;

        var data = this.GenerateClearance();
        if (data == null)
            return;
        
        PrintClearance(data);
        this.MessageBox.Inform("Registration has been sent to the printer.", "Registration");
    }
}
