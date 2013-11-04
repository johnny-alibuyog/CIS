using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;
using CIS.Core.Entities.Memberships;
using CIS.Core.Entities.Polices;
using CIS.Core.Utilities.Extentions;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Cameras;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Features.Commons.Signatures;
using CIS.UI.Features.Memberships.Users;
using CIS.UI.Features.Polices.Maintenances;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using CIS.UI.Utilities.Reports;
using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Reporting.WinForms;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Clearances
{

    [HandleError]
    public class ApplicationController : ControllerBase<ApplicationViewModel>
    {
        private enum Direction { Previous, Next }

        public ApplicationController(ApplicationViewModel viewModel)
            : base(viewModel)
        {
            this.Reset();

            this.MessageBus.Listen<MaintenanceMessage>()
                .Subscribe(x =>
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
        }

        #region Routine Helpers

        private bool ValidateScreen()
        {
            if (this.ViewModel.CurrentViewModel.IsValid == false)
            {
                this.MessageBox.Warn(this.ViewModel.CurrentViewModel.Error, "Application");
                return false;
            }

            if (this.ViewModel.Finding == this.ViewModel.CurrentViewModel)
            {
                if (this.ViewModel.Finding.Amendment != null)
                {
                    var dialog = new DialogService<ApprovalViewModel>();
                    var result = dialog.ShowModal();
                    if (result == null)
                        return false;

                    this.ViewModel.Finding.Amendment.ApproverUserId = result.UserId;
                }
            }
            return true;
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

            if (direction == Direction.Next)
            {
                if (this.ViewModel.Finding == this.ViewModel.CurrentViewModel)
                    this.PopulateFinding();

                if (this.ViewModel.Summary == this.ViewModel.CurrentViewModel)
                    this.PopulateSummary();
            }
        }

        private void InitializeViews()
        {
            this.ViewModel.PersonalInformation = new PersonalInformationViewModel();
            this.ViewModel.Camera = new CameraViewModel();
            this.ViewModel.FingerScanner = new FingerScannerViewModel();
            this.ViewModel.Signature = new SignatureViewModel();
            this.ViewModel.Finding = new FindingViewModel();
            this.ViewModel.Summary = new SummaryViewModel();

            //this.ViewModel.PersonalInformation = IoC.Container.Resolve<PersonalInformationViewModel>();
            //this.ViewModel.Camera = IoC.Container.Resolve<CameraViewModel>();
            //this.ViewModel.FingerScanner = IoC.Container.Resolve<FingerScannerViewModel>();
            //this.ViewModel.Signature = IoC.Container.Resolve<SignatureViewModel>();
            //this.ViewModel.Finding = IoC.Container.Resolve<FindingViewModel>();
            //this.ViewModel.Summary = IoC.Container.Resolve<SummaryViewModel>();

            this.ViewModel.ViewModels = new List<ViewModelBase>();

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Setting>()
                    .Where(x => x.Terminal.MachineName == Environment.MachineName)
                    .ToFutureValue();

                var setting = query.Value;

                this.ViewModel.ViewModels.Add(this.ViewModel.PersonalInformation);

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
                var purposeQuery = session.Query<Purpose>().Cacheable().ToFuture();
                var officerQuery = session.Query<Officer>().Cacheable().ToFuture();
                var stationQuery = session.Query<Station>().Cacheable().ToFuture();
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
                this.ViewModel.Summary.Validity = station.GetValidity(DateTime.Today);
                this.ViewModel.PersonalInformation.Address.City = station.Address.City;
                this.ViewModel.PersonalInformation.Address.Province = station.Address.Province;

                transaction.Commit();
            }
        }

        private void PopulateView()
        {
            if (this.ViewModel.Finding == this.ViewModel.CurrentViewModel)
                this.PopulateFinding();

            if (this.ViewModel.Summary == this.ViewModel.CurrentViewModel)
                this.PopulateSummary();
        }

        private void Evaluate()
        {
            this.ViewModel.Summary.PerfectMatchFindings = string.Empty;
            this.ViewModel.Summary.PartialMatchFindings = string.Empty;
            this.ViewModel.Summary.FinalFindings = string.Empty;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var person = this.ViewModel.PersonalInformation.Person;

                var suspectHits = session.Query<Suspect>()
                    .Where(x =>
                        x.Person.FirstName == person.FirstName &&
                        x.Person.LastName == person.LastName
                    )
                    .Fetch(x => x.Warrant)
                    .ToFuture();

                var expiredFirearmsLicenses = session.Query<License>()
                    .Where(x =>
                        x.ExpiryDate <= DateTime.Today &&
                        x.Person.FirstName == person.FirstName &&
                        x.Person.MiddleName == person.MiddleName &&
                        x.Person.LastName == person.LastName
                    )
                    .ToFuture();

                var suspectPerfectMatch = suspectHits
                    .Where(x =>
                        x.Person.FirstName.IsEqualTo(person.FirstName) &&
                        x.Person.MiddleName.IsEqualTo(person.MiddleName) &&
                        x.Person.LastName.IsEqualTo(person.LastName)
                    )
                    .ToList();

                var suspectPartialMatch = suspectHits
                    .Where(x =>
                        x.Person.FirstName.IsEqualTo(person.FirstName) &&
                        x.Person.LastName.IsEqualTo(person.LastName) &&
                        string.IsNullOrWhiteSpace(x.Person.MiddleName)
                    )
                    .Except(suspectPerfectMatch)
                    .ToList();

                if (suspectPartialMatch.Count > 0)
                {
                    this.ViewModel.Summary.PartialMatchFindings = string.Format("Person with the name of {0} and criminial record {1} has partialy same name as the applicant.",
                        suspectPartialMatch.First().Person.Fullname, suspectPartialMatch.First().Warrant.Crime);
                }

                if (suspectPerfectMatch.Count > 0)
                {
                    this.ViewModel.Summary.PerfectMatchFindings = string.Join(
                        separator: Environment.NewLine,
                        values: suspectPerfectMatch
                            .Select(x => x.Warrant.Crime)
                            .Distinct()
                    );
                }

                if (expiredFirearmsLicenses.Count() > 0)
                {
                    if (this.ViewModel.Summary.PerfectMatchFindings != string.Empty)
                        this.ViewModel.Summary.PerfectMatchFindings += Environment.NewLine;

                    this.ViewModel.Summary.PerfectMatchFindings += string.Join(
                        separator: Environment.NewLine,
                        values: expiredFirearmsLicenses
                            .Select(x =>
                                string.Format("Expired Firearm Lincense - FA Lic. No. {0} - {1}",
                                    x.LicenseNumber, x.ExpiryDate.Value.ToString("MMM-dd-yyyy")
                                )
                            )
                            .Distinct()
                    );
                }

                this.ViewModel.Summary.FinalFindings = !string.IsNullOrWhiteSpace(this.ViewModel.Summary.PerfectMatchFindings)
                    ? this.ViewModel.Summary.PerfectMatchFindings
                    : "No Derogatory Records/Information";

                transaction.Commit();
            }

            this.ViewModel.Summary.Picture = this.ViewModel.Camera.Picture;
            this.ViewModel.Summary.RightThumb = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb];
            this.ViewModel.Summary.Address = this.ViewModel.PersonalInformation.Address.ToString();
            this.ViewModel.Summary.FullName = this.ViewModel.PersonalInformation.Person.FullName;
            this.ViewModel.Summary.BirthDate = this.ViewModel.PersonalInformation.Person.BirthDate;
            this.ViewModel.Summary.BirthPlace = this.ViewModel.PersonalInformation.BirthPlace;

            //if (!string.IsNullOrWhiteSpace(this.ViewModel.Summary.PartialMatchFindings))
            //{
            //    MessageDialog.Show(this.ViewModel.Summary.PartialMatchFindings, "Clearance", MessageBoxButton.OK);
            //}
        }

        private void PopulateFinding()
        {
            this.ViewModel.Finding.Amendment = null;
            ((ICollection<HitViewModel>)this.ViewModel.Finding.Hits).Clear();

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
                                    : x.Person.MiddleName == person.MiddleName
                            )
                        )
                    )
                    .Fetch(x => x.Warrant)
                    .ToFuture();

                var expiredFirearmsLicenseQuery = session.Query<License>()
                    .Where(x =>
                        x.ExpiryDate != null &&
                        x.ExpiryDate <= DateTime.Today &&
                        x.Person.FirstName == person.FirstName &&
                        x.Person.LastName == person.LastName &&
                        (
                            x.Person.MiddleName == null ||
                            x.Person.MiddleName == string.Empty ||
                            (
                                x.Person.MiddleName.Length == 1 || x.Person.MiddleName.Contains(".")
                                    ? x.Person.MiddleName.Substring(0, 1) == person.MiddleName.Substring(0, 1)
                                    : x.Person.MiddleName == person.MiddleName
                            )
                        )
                    )
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

                foreach (var item in expiredFirearmsLicenseQuery)
                {
                    var hit = new ExpiredLicenseHitViewModel();
                    hit.LicenseId = item.Id;
                    hit.ExpiryDate = item.ExpiryDate;
                    hit.LicenseNumber = item.LicenseNumber;
                    hit.Applicant.SerializeWith(this.ViewModel.PersonalInformation.Person);
                    hit.Person.SerializeWith(item.Person);
                    hit.Address.SerializeWith(item.Address);
                    hit.Gun.SerializeWith(item.Gun);

                    this.ViewModel.Finding.Hits.Add(hit);
                }

                transaction.Commit();
            }

            this.ViewModel.Finding.SelectedHit = this.ViewModel.Finding.Hits.FirstOrDefault();
        }

        private void PopulateSummary()
        {
            this.ViewModel.Summary.FinalFindings = this.ViewModel.Finding.Evaluate();
            this.ViewModel.Summary.Picture = this.ViewModel.Camera.Picture;
            this.ViewModel.Summary.RightThumb = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb];
            this.ViewModel.Summary.Address = this.ViewModel.PersonalInformation.Address.ToString();
            this.ViewModel.Summary.FullName = this.ViewModel.PersonalInformation.Person.FullName;
            this.ViewModel.Summary.BirthDate = this.ViewModel.PersonalInformation.Person.BirthDate;
            this.ViewModel.Summary.BirthPlace = this.ViewModel.PersonalInformation.BirthPlace;

        }

        private ClearanceReportViewModel GenerateClearance()
        {
            var result = new ClearanceReportViewModel();

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var person = this.ViewModel.PersonalInformation.Person;

                var clearanceAlias = (Clearance)null;
                var applicantAlias = (Applicant)null;
                var pictureAlias = (ImageBlob)null;
                var fingerPrintAlias = (FingerPrint)null;
                var stationAlias = (Station)null;
                var verifierAlias = (Officer)null;
                var certifierAlias = (Officer)null;
                var barcodeAlais = (Barcode)null;
                var findingAlias = (Finding)null;

                var clearanceQuery = session.QueryOver<Clearance>(() => clearanceAlias)
                    .Left.JoinAlias(() => clearanceAlias.Applicant, () => applicantAlias)
                    .Left.JoinAlias(() => clearanceAlias.Station, () => stationAlias)
                    .Left.JoinAlias(() => clearanceAlias.Verifier, () => verifierAlias)
                    .Left.JoinAlias(() => clearanceAlias.Certifier, () => certifierAlias)
                    .Left.JoinAlias(() => clearanceAlias.Barcode, () => barcodeAlais)
                    .Left.JoinAlias(() => clearanceAlias.Finding, () => findingAlias)
                    .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
                    .Left.JoinAlias(() => applicantAlias.Picture, () => pictureAlias)
                    .Left.JoinQueryOver(() => findingAlias.Hits)
                    .Left.JoinQueryOver(() => findingAlias.Amendment)
                    .Left.JoinQueryOver(() => stationAlias.Logo)
                    .Left.JoinQueryOver(() => barcodeAlais.Image)
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
                        clearanceAlias.IssueDate == DateTime.Today
                    )
                    .FutureValue();

                var stationQuery = session.QueryOver<Station>()
                    .Left.JoinQueryOver(x => x.Logo)
                    .Future();

                var officerAlias = (Officer)null;
                var rankAlias = (Rank)null;

                var certifierQuery = session.QueryOver<Officer>(() => officerAlias)
                    .Left.JoinAlias(() => officerAlias.Rank, () => rankAlias)
                    .Where(() => officerAlias.Id == this.ViewModel.PersonalInformation.Certifier.Id)
                    .FutureValue();

                var verifierQuery = session.QueryOver<Officer>(() => officerAlias)
                    .Left.JoinAlias(() => officerAlias.Rank, () => rankAlias)
                    .Where(() => officerAlias.Id == this.ViewModel.PersonalInformation.Verifier.Id)
                    .FutureValue();

                var settingQuery = session.Query<Setting>()
                    .Where(x => x.Terminal.MachineName == Environment.MachineName)
                    .Fetch(x => x.CurrentVerifier)
                    .Fetch(x => x.CurrentCertifier)
                    .Cacheable();


                var setting = settingQuery.FirstOrDefault();
                if (setting != null)
                {
                    setting.CurrentVerifier = verifierQuery.Value;
                    setting.CurrentCertifier = certifierQuery.Value;
                }

                var clearance = clearanceQuery.Value;
                if (clearance == null)
                    clearance = new Clearance();

                clearance.Applicant.Person = (Person)this.ViewModel.PersonalInformation.Person.DeserializeInto(new Person());
                clearance.Applicant.Address = (Address)this.ViewModel.PersonalInformation.Address.DeserializeInto(new Address());
                clearance.Applicant.Picture.Image = this.ViewModel.Camera.Picture.ToImage();
                clearance.Applicant.Signature.Image = this.ViewModel.Signature.SignatureImage.ToImage();
                clearance.Applicant.FingerPrint.RightThumb.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb].ToImage();
                clearance.Applicant.FingerPrint.RightIndex.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightIndex].ToImage();
                clearance.Applicant.FingerPrint.RightMiddle.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightMiddle].ToImage();
                clearance.Applicant.FingerPrint.RightRing.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightRing].ToImage();
                clearance.Applicant.FingerPrint.RightPinky.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightPinky].ToImage();
                clearance.Applicant.FingerPrint.LeftThumb.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftThumb].ToImage();
                clearance.Applicant.FingerPrint.LeftIndex.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftIndex].ToImage();
                clearance.Applicant.FingerPrint.LeftMiddle.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftMiddle].ToImage();
                clearance.Applicant.FingerPrint.LeftRing.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftRing].ToImage();
                clearance.Applicant.FingerPrint.LeftPinky.Image = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftPinky].ToImage();
                clearance.Applicant.Height = this.ViewModel.PersonalInformation.Height;
                clearance.Applicant.Weight = this.ViewModel.PersonalInformation.Weight;
                clearance.Applicant.AlsoKnownAs = this.ViewModel.PersonalInformation.AlsoKnownAs;
                clearance.Applicant.BirthPlace = this.ViewModel.PersonalInformation.BirthPlace;
                clearance.Applicant.Occupation = this.ViewModel.PersonalInformation.Occupation;
                clearance.Applicant.Religion = this.ViewModel.PersonalInformation.Religion;
                clearance.Applicant.Citizenship = this.ViewModel.PersonalInformation.Citizenship;
                clearance.Applicant.CivilStatus = this.ViewModel.PersonalInformation.CivilStatus;
                clearance.Applicant.Purpose = session.Get<Purpose>(this.ViewModel.PersonalInformation.Purpose.Id);

                clearance.SetVerifier(verifierQuery.Value);
                clearance.SetCertifier(certifierQuery.Value);
                clearance.SetStation(stationQuery.FirstOrDefault());
                clearance.IssueDate = this.ViewModel.Summary.IssuedDate;
                clearance.Validity = this.ViewModel.Summary.Validity;
                clearance.OfficialReceiptNumber = this.ViewModel.Summary.OfficialReceiptNumber;
                clearance.TaxCertificateNumber = this.ViewModel.Summary.TaxCertificateNumber;
                clearance.PartialMatchFindings = this.ViewModel.Summary.PartialMatchFindings;
                clearance.PerfectMatchFindings = this.ViewModel.Summary.PerfectMatchFindings;
                clearance.FinalFindings = this.ViewModel.Summary.FinalFindings;

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
                            IsIdentical = x.IsIdentical,
                            Suspect = session.Load<Suspect>(x.SuspectId)
                        })
                        .AsEnumerable<Hit>()
                        .Concat(this.ViewModel.Finding.Hits
                        .OfType<ExpiredLicenseHitViewModel>()
                        .Select(x => new ExpiredLicenseHit()
                        {
                            HitScore = x.HitScore,
                            IsIdentical = x.IsIdentical,
                            ExpiryDate = x.ExpiryDate,
                            License = session.Load<License>(x.LicenseId),
                        })
                        .AsEnumerable<Hit>())
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

                session.SaveOrUpdate(clearance);
                transaction.Commit();

                result.SerializeWith(clearance);
            }

            if (!string.IsNullOrWhiteSpace(result.PartialMatchFindings))
            {
                var message = result.PartialMatchFindings + " This will not reflect in the findings. Do you still want to proceed?";
                var confirmed = this.MessageBox.Confirm(message, "Clearance");
                if (confirmed == false)
                    return null;
            }

            return result;
        }

        private void PrintClearance(ClearanceReportViewModel data)
        {
            var report = new LocalReport();
            report.EnableExternalImages = true;
            report.ReportEmbeddedResource = "CIS.UI.Features.Polices.Clearances.ClearanceReport.rdlc";
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
            if (this.ViewModel.Camera != null)
                this.ViewModel.Camera.Stop.Execute(null);

            if (this.ViewModel.FingerScanner != null)
                this.ViewModel.FingerScanner.Stop.Execute(null);

            InitializeViews();
            PopulateLookups();
        }

        public virtual void Previous()
        {
            var currentIndex = this.ViewModel.ViewModels.IndexOf(this.ViewModel.CurrentViewModel);
            this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels[currentIndex - 1];

            this.InitializeScreen(Direction.Previous);
        }

        //[Authorize(Roles = new Role[] { Role.PoliceEncoder })]
        public virtual void Next()
        {
            if (this.ValidateScreen() == false)
                return;

            var currentIndex = this.ViewModel.ViewModels.IndexOf(this.ViewModel.CurrentViewModel);
            this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels[currentIndex + 1];

            this.InitializeScreen(Direction.Next);
        }

        public virtual void Release()
        {
            var confirmed = this.MessageBox.Confirm("Do you want to release clearance.", "Clearance");
            if (confirmed == false)
                return;

            var data = this.GenerateClearance();
            if (data != null)
            {
                PrintClearance(data);
                this.MessageBox.Inform("Clearance has been sent to the printer.", "Clearance");
            }
        }
    }
}
