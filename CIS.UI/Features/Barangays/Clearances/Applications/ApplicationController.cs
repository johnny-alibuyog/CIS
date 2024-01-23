using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using CIS.Core.Utilities.Extentions;
using CIS.Data.Commons.Exceptions;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Features.Barangays.Maintenances;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Cameras;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Features.Commons.Signatures;
using CIS.UI.Features.Memberships.Users.Approvals;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace CIS.UI.Features.Barangays.Clearances.Applications;

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
            .Subscribe(x => CheckIfExistingApplicant(x));
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
                dialog.ViewModel.Roles = new Role[] { Role.BarangayApprover };
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
        //this.ViewModel.OtherInformation = new OtherInformationViewModel();
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
            //this.ViewModel.ViewModels.Add(this.ViewModel.OtherInformation);

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
        using var session = this.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var purposeQuery = session.Query<Purpose>().Cacheable().ToList();
        var officeQuery = session.Query<Office>().Cacheable().ToList();

        this.ViewModel.PersonalInformation.Purposes = purposeQuery
            .Select(x => new Lookup<Guid>(x.Id, x.Name))
            .ToReactiveList();

        var office = officeQuery.FirstOrDefault();
        if (office.ClearanceFee == null || office.ClearanceFee <= 0M)
            office.ClearanceFee = 500.00M;

        if (office.CertificationFee == null || office.CertificationFee <= 0M)
            office.CertificationFee = 30.00M;

        if (office.DocumentStampTax == null || office.DocumentStampTax <= 0M)
            office.DocumentStampTax = 15.00M;

        this.ViewModel.Summary.ClearanceFee = office.ClearanceFee;
        this.ViewModel.PersonalInformation.Address.City = office.Address.City;
        this.ViewModel.PersonalInformation.Address.Province = office.Address.Province;

        transaction.Commit();
    }

    private void CheckIfExistingApplicant(PersonBasicViewModel person)
    {
        var citizen = default(Citizen);

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var citizenAlias = default(Citizen);
            var fingerPrintAlias = default(FingerPrint);

            var citizenQuery = session.QueryOver(() => citizenAlias)
                .Left.JoinAlias(() => citizenAlias.FingerPrint, () => fingerPrintAlias)
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
                    citizenAlias.Person.FirstName == person.FirstName &&
                    citizenAlias.Person.MiddleName == person.MiddleName &&
                    citizenAlias.Person.LastName == person.LastName &&
                    citizenAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch pictures
            session.QueryOver(() => citizenAlias)
                .Left.JoinQueryOver(() => citizenAlias.Pictures)
                .Where(() =>
                    citizenAlias.Person.FirstName == person.FirstName &&
                    citizenAlias.Person.MiddleName == person.MiddleName &&
                    citizenAlias.Person.LastName == person.LastName &&
                    citizenAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch signatures
            session.QueryOver(() => citizenAlias)
                .Left.JoinQueryOver(() => citizenAlias.Signatures)
                .Where(() =>
                    citizenAlias.Person.FirstName == person.FirstName &&
                    citizenAlias.Person.MiddleName == person.MiddleName &&
                    citizenAlias.Person.LastName == person.LastName &&
                    citizenAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            citizen = citizenQuery.Value;

            transaction.Commit();
        }

        if (citizen == null)
            return;
            //this.MessageBox.Inform("Applicant has an existing record.\nPlease update with latest data.");

        this.ViewModel.PersonalInformation.Person.Gender = citizen.Person.Gender;
        this.ViewModel.PersonalInformation.Person.BirthDate = citizen.Person.BirthDate;
        this.ViewModel.PersonalInformation.Address.SerializeWith(citizen.CurrentAddress);
        this.ViewModel.Camera.Picture = citizen.Pictures.Count() > 0 ? citizen.Pictures.Last().Image.ToBitmapSource() : null;
        this.ViewModel.Signature.SignatureImage = citizen.Signatures.Count() > 0 ? citizen.Signatures.Last().Image.ToBitmapSource() : null;
        this.ViewModel.FingerScanner.CapturedFingerImage = citizen.FingerPrint.RightThumb.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb] = citizen.FingerPrint.RightThumb.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightIndex] = citizen.FingerPrint.RightIndex.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightMiddle] = citizen.FingerPrint.RightMiddle.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightRing] = citizen.FingerPrint.RightRing.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightPinky] = citizen.FingerPrint.RightPinky.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftThumb] = citizen.FingerPrint.LeftThumb.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftIndex] = citizen.FingerPrint.LeftIndex.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftMiddle] = citizen.FingerPrint.LeftMiddle.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftRing] = citizen.FingerPrint.LeftRing.Image.ToBitmapSource();
        this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftPinky] = citizen.FingerPrint.LeftPinky.Image.ToBitmapSource();
    }

    private void Evaluate()
    {
        this.ViewModel.Finding.Amendment = null;
        this.ViewModel.Finding.Hits.Clear();

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var person = this.ViewModel.PersonalInformation.Person;

            var matchingBlotterQuery = session.Query<Blotter>()
                .Where(x => x.Respondents
                    .Any(o =>
                        o.Person.FirstName == person.FirstName &&
                        o.Person.LastName == person.LastName &&
                        (
                            o.Person.MiddleName == null ||
                            o.Person.MiddleName == string.Empty ||
                            (
                                o.Person.MiddleName.Length == 1 || o.Person.MiddleName.Contains(".")
                                    ? o.Person.MiddleName.Substring(0, 1) == person.MiddleName.Substring(0, 1)
                                    : o.Person.MiddleName == (person.MiddleName ?? string.Empty)
                            )
                        )
                    )
                )
                .ToFuture();

            foreach (var blotter in matchingBlotterQuery)
            {
                var citizenHit = blotter.Respondents
                    .Where(o =>
                        o.Person.FirstName.IsEqualTo(person.FirstName) &&
                        o.Person.LastName.IsEqualTo(person.LastName) &&
                        (
                            o.Person.MiddleName == null ||
                            o.Person.MiddleName == string.Empty ||
                            (
                                o.Person.MiddleName.Length == 1 || o.Person.MiddleName.Contains(".")
                                    ? o.Person.MiddleName.Substring(0, 1).IsEqualTo(person.MiddleName.Substring(0, 1))
                                    : o.Person.MiddleName.IsEqualTo((person.MiddleName ?? string.Empty))
                            )
                        )
                    )
                    .FirstOrDefault();

                if (citizenHit == null)
                    continue;

                var hit = new BlotterHitViewModel();
                hit.BlotterId = blotter.Id;
                hit.CitizenId = citizenHit.Id;
                hit.Applicant.SerializeWith(this.ViewModel.PersonalInformation.Person);
                hit.Respondent.SerializeWith(citizenHit.Person);
                hit.Complaint = blotter.Complaint;
                hit.Details = blotter.Details;
                hit.Remarks = blotter.Remarks;
                hit.Address.SerializeWith(blotter.Address);
                hit.OccuredOn = blotter.OccuredOn;

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
    }

    private ClearanceReportViewModel GenerateClearance()
    {
        var result = new ClearanceReportViewModel();

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var person = this.ViewModel.PersonalInformation.Person;

            var citizenAlias = default(Citizen);
            var fingerPrintAlias = default(FingerPrint);

            var citizenQuery = session.QueryOver(() => citizenAlias)
                .Left.JoinAlias(() => citizenAlias.FingerPrint, () => fingerPrintAlias)
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
                    citizenAlias.Person.FirstName == person.FirstName &&
                    citizenAlias.Person.MiddleName == person.MiddleName &&
                    citizenAlias.Person.LastName == person.LastName &&
                    citizenAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch pictures
            session.QueryOver(() => citizenAlias)
                .Left.JoinQueryOver(x => x.Pictures)
                .Where(() =>
                    citizenAlias.Person.FirstName == person.FirstName &&
                    citizenAlias.Person.MiddleName == person.MiddleName &&
                    citizenAlias.Person.LastName == person.LastName &&
                    citizenAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // fetch signatures
            session.QueryOver(() => citizenAlias)
                .Left.JoinQueryOver(x => x.Signatures)
                .Where(() =>
                    citizenAlias.Person.FirstName == person.FirstName &&
                    citizenAlias.Person.MiddleName == person.MiddleName &&
                    citizenAlias.Person.LastName == person.LastName &&
                    citizenAlias.Person.Suffix == person.Suffix
                )
                .FutureValue();

            // incumbent
            var incumbentQuery = session.Query<Incumbent>()
                .FetchMany(x => x.Officials)
                .ThenFetch(x => x.Position)
                .ToFuture();

            // office
            var officeQuery = session.QueryOver<Office>()
                .Left.JoinQueryOver(x => x.Logo)
                .Cacheable()
                .List();

            var office = officeQuery.FirstOrDefault();
            var incumbent = incumbentQuery.OrderByDescending(x => x.Date).FirstOrDefault();

            var isNewCitizen = false;
            var citizen = citizenQuery.Value;
            if (citizen == null)
            {
                isNewCitizen = true;
                citizen = new Citizen();
            }

            var isNewClearance = false;
            var clearance = isNewCitizen == false
                ? session.Query<Clearance>().FirstOrDefault(x =>
                    x.Applicant == citizen &&
                    x.IssueDate == DateTime.Today
                )
                : null;

            if (clearance == null)
            {
                isNewClearance = true;
                clearance = new Clearance();
                clearance.Applicant = citizen;
            }

            var picture = new ImageBlob(this.ViewModel.Camera.Picture.ToImage());
            var signature = new ImageBlob(this.ViewModel.Signature.SignatureImage.ToImage());

            citizen.Person = (Person)this.ViewModel.PersonalInformation.Person.DeserializeInto(new Person());
            citizen.CurrentAddress = (Address)this.ViewModel.PersonalInformation.Address.DeserializeInto(new Address());
            citizen.AddPicture(picture);
            citizen.AddSignature(signature);
            citizen.FingerPrint.RightThumb.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb].ToImage();
            citizen.FingerPrint.RightIndex.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightIndex].ToImage();
            citizen.FingerPrint.RightMiddle.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightMiddle].ToImage();
            citizen.FingerPrint.RightRing.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightRing].ToImage();
            citizen.FingerPrint.RightPinky.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightPinky].ToImage();
            citizen.FingerPrint.LeftThumb.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftThumb].ToImage();
            citizen.FingerPrint.LeftIndex.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftIndex].ToImage();
            citizen.FingerPrint.LeftMiddle.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftMiddle].ToImage();
            citizen.FingerPrint.LeftRing.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftRing].ToImage();
            citizen.FingerPrint.LeftPinky.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftPinky].ToImage();

            // this fields will/may change every clearnce application. denormilize this fields
            clearance.ApplicantPicture = picture;
            clearance.ApplicantSignature = signature;
            clearance.ApplicantAddress = citizen.CurrentAddress.ToString();

            clearance.Office = office;
            clearance.Officials = incumbent.Officials;
            clearance.ApplicationDate = this.ViewModel.Summary.ApplicationDate;
            clearance.IssueDate = this.ViewModel.Summary.IssuedDate;
            clearance.Fee = this.ViewModel.Summary.ClearanceFee;
            clearance.ControlNumber = this.ViewModel.Summary.ControlNumber;
            clearance.OfficialReceiptNumber = this.ViewModel.Summary.OfficialReceiptNumber;
            clearance.TaxCertificateNumber = this.ViewModel.Summary.TaxCertificateNumber;
            clearance.FinalFindings = this.ViewModel.Summary.FinalFindings;
            clearance.Purpose = session.Get<Purpose>(this.ViewModel.PersonalInformation.Purpose.Id);

            if (this.ViewModel.Finding.HasHits)
            {
                clearance.Finding = new Finding();
                clearance.Finding.FinalFindings = this.ViewModel.Summary.FinalFindings;

                // hits
                clearance.Finding.Hits = this.ViewModel.Finding.Hits
                    .OfType<BlotterHitViewModel>()
                    .Select(x => new BlotterHit()
                    {
                        HitScore = x.HitScore,
                        IsIdentical = x.IsIdentifiedHit,
                        Respondent = session.Load<Citizen>(x.CitizenId),
                        Blotter = session.Load<Blotter>(x.BlotterId),
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

            if (isNewClearance)
                session.Save(clearance);

            transaction.Commit();
        }

        //result.SerializeWith(clearance);

        return result;
    }

    private void PrintClearance(ClearanceReportViewModel data)
    {
        //var report = new LocalReport();
        //report.EnableExternalImages = true;
        //report.ReportEmbeddedResource = "CIS.UI.Features.Polices.Clearances.ClearanceReport.rdlc";
        //report.DataSources.Add(new ReportDataSource()
        //{
        //    Name = "ItemDataSet",
        //    Value = new BindingSource() { DataSource = new object[] { data } }
        //});
        //var print = new ReportPrintDocument(report);
        //print.Print();
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
        var confirmed = this.MessageBox.Confirm("Do you want to release clearance.", "Clearance");
        if (confirmed == false)
            return;

        var data = this.GenerateClearance();
        if (data == null)
            return;
        
        PrintClearance(data);
        this.MessageBox.Inform("Clearance has been sent to the printer.", "Clearance");
    }
}
