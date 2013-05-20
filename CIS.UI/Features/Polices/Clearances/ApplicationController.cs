using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Cameras;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ApplicationController : ControllerBase<ApplicationViewModel>
    {
        public ApplicationController(ApplicationViewModel viewModel) : base(viewModel)
        {
            this.Reset();

            this.ViewModel.Previous = new ReactiveCommand(this.ViewModel
                .WhenAny(
                    x => x.CurrentViewModel, 
                    x => x.CurrentViewModel.IsValid, 
                    (current, isValid) => 
                    {
                        if (current.Value != this.ViewModel.ViewModels.First())
                            return true;

                        return false;
                    })
                );
            this.ViewModel.Previous.Subscribe(x => Previous());

            this.ViewModel.Next = new ReactiveCommand(this.ViewModel
                .WhenAny(
                    x => x.CurrentViewModel,
                    x => x.CurrentViewModel.IsValid,
                    (current, isValid) =>
                    {
                        if (current.Value != this.ViewModel.ViewModels.Last())
                            return true;

                        if (current.Value.IsValid)
                            return true;

                        if (isValid.Value == true)
                            return true;

                        return false;
                    })
                );
            this.ViewModel.Next.Subscribe(x => Next());

            this.ViewModel.Reset = new ReactiveCommand();
            this.ViewModel.Reset.Subscribe(x => Reset());

            this.ViewModel.Release = new ReactiveCommand();
            this.ViewModel.Release.Subscribe(x => Release());
        }

        #region Routine Helpers

        private void InitializeDevices()
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

        private void PopulateLookups()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var purposeQuery = session.Query<Purpose>().Cacheable().ToFuture();
                var officerQuery = session.Query<Officer>().Cacheable().ToFuture();
                var stationQuery = session.Query<Station>().Cacheable().ToFuture();

                this.ViewModel.PersonalInformation.Purposes = purposeQuery
                  .Select(x => new Lookup<Guid>()
                  {
                      Id = x.Id,
                      Name = x.Name
                  })
                  .ToReactiveColletion();

                this.ViewModel.PersonalInformation.Verifiers = officerQuery
                    .Select(x => new Lookup<Guid>()
                    {
                        Id = x.Id,
                        Name = x.Person.Fullname
                    })
                    .ToReactiveColletion();

                this.ViewModel.PersonalInformation.Certifiers = officerQuery
                    .Select(x => new Lookup<Guid>()
                    {
                        Id = x.Id,
                        Name = x.Person.Fullname
                    })
                    .ToReactiveColletion();

                var station = stationQuery.FirstOrDefault();
                this.ViewModel.Summary.Validity = station.GetValidity(DateTime.Today);
                this.ViewModel.PersonalInformation.Address.City = station.Address.City;
                this.ViewModel.PersonalInformation.Address.Province = station.Address.Province;

                transaction.Commit();
            }
        }

        private void Evaluate()
        {
            this.ViewModel.Summary.PerfectMatchFindings = string.Empty;
            this.ViewModel.Summary.PartialMatchFindings = string.Empty;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var person = this.ViewModel.PersonalInformation.Person;

                var warrants = session.Query<Warrant>()
                    .Where(x => x.Suspects
                      .Any(o =>
                          o.Person.FirstName == person.FirstName &&
                          o.Person.LastName == person.LastName
                      )
                  )
                  .FetchMany(x => x.Suspects)
                  .ToFuture();

                var expiredFirearmsLicenses = session.Query<License>()
                    .Where(x =>
                        x.ExpiryDate <= DateTime.Today &&
                        x.Person.FirstName == person.FirstName &&
                        x.Person.MiddleName == person.MiddleName &&
                        x.Person.LastName == person.LastName
                    )
                    .ToFuture();

                var suspectPerfectMatch = warrants
                    .SelectMany(x => x.Suspects)
                    .Where(x =>
                        x.Person.FirstName == person.FirstName &&
                        x.Person.MiddleName == person.MiddleName &&
                        x.Person.LastName == person.LastName
                    )
                    .ToList();

                var suspectPartialMatch = warrants
                    .SelectMany(x => x.Suspects)
                    .Where(x => string.IsNullOrWhiteSpace(x.Person.MiddleName))
                    .Except(suspectPerfectMatch)
                    .ToList();

                if (suspectPartialMatch.Count > 0)
                {
                    this.ViewModel.Summary.PartialMatchFindings = string.Format("Person with the name of {0} and criminial record {1} has partialy the name as the applicant. Please verfiy.",
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
                                string.Format("Expired Firearm Lincense - FA Lic. No. {0} - {2}",
                                    x.LicenseNumber, x.ExpiryDate.ToString("MMM-dd-yyyy")
                                )
                            )
                            .Distinct()
                    );
                }

                transaction.Commit();
            }

            this.ViewModel.Summary.Picture = this.ViewModel.Camera.Picture;
            this.ViewModel.Summary.Address = this.ViewModel.PersonalInformation.Address.ToString();
            this.ViewModel.Summary.FullName = this.ViewModel.PersonalInformation.Person.FullName;
            this.ViewModel.Summary.BirthDate = this.ViewModel.PersonalInformation.Person.BirthDate;
            this.ViewModel.Summary.BirthPlace = this.ViewModel.PersonalInformation.BirthPlace;

            if (!string.IsNullOrWhiteSpace(this.ViewModel.Summary.PartialMatchFindings))
            {
                MessageDialog.Show(this.ViewModel.Summary.PartialMatchFindings, "Clearance", MessageBoxButton.OK);
            }
        }

        private ClearanceReportViewModel Generate()
        {
            try
            {
                var result = new ClearanceReportViewModel();

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var person = this.ViewModel.PersonalInformation.Person;

                    var clearanceAlias = (Clearance)null;
                    var applicantAlias = (Applicant)null;
                    var pictureAlias = (Picture)null;
                    var fingerPrintAlias = (FingerPrint)null;
                    var imageAlias = (ImageBlob)null;
                    var verifierAlias = (Officer)null;
                    var certifierAlias = (Officer)null;

                    var clearanceQuery = session.QueryOver<Clearance>(() => clearanceAlias)
                        .Left.JoinAlias(() => clearanceAlias.Applicant, () => applicantAlias)
                        .Left.JoinAlias(() => clearanceAlias.Verifier, () => verifierAlias)
                        .Left.JoinAlias(() => clearanceAlias.Certifier, () => certifierAlias)
                        .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
                        .Left.JoinAlias(() => applicantAlias.Picture, () => pictureAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.RightThumb, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.RightIndex, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.RightMiddle, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.RightRing, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.RightPinky, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.LeftThumb, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.LeftIndex, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.LeftMiddle, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.LeftRing, () => imageAlias)
                        .Left.JoinAlias(() => fingerPrintAlias.LeftPinky, () => imageAlias)
                        .Left.JoinAlias(() => pictureAlias.Image, () => imageAlias)
                        .Where(() =>
                            applicantAlias.Person.FirstName == person.FirstName &&
                            applicantAlias.Person.MiddleName == person.MiddleName &&
                            applicantAlias.Person.LastName == person.LastName &&
                            applicantAlias.Person.Suffix == person.Suffix &&
                            clearanceAlias.IssueDate == DateTime.Today
                        )
                        .FutureValue();

                    var stationQuery = session.QueryOver<Station>()
                        .Left.JoinQueryOver(x => x.Logo)
                        .Left.JoinQueryOver(x => x.Image)
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

                    var clearance = clearanceQuery.Value;
                    if (clearance == null)
                        clearance = new Clearance();

                    clearance.Applicant.Person = (Person)this.ViewModel.PersonalInformation.Person.SerializeInto(new Person());
                    clearance.Applicant.Address = (Address)this.ViewModel.PersonalInformation.Address.SerializeInto(new Address());
                    clearance.Applicant.Picture.Image.Content = this.ViewModel.Camera.Picture.ToImage();
                    clearance.Applicant.FingerPrint.RightThumb.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightThumb].ToImage();
                    clearance.Applicant.FingerPrint.RightIndex.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightIndex].ToImage();
                    clearance.Applicant.FingerPrint.RightMiddle.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightMiddle].ToImage();
                    clearance.Applicant.FingerPrint.RightRing.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightRing].ToImage();
                    clearance.Applicant.FingerPrint.RightPinky.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.RightPinky].ToImage();
                    clearance.Applicant.FingerPrint.LeftThumb.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftThumb].ToImage();
                    clearance.Applicant.FingerPrint.LeftIndex.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftIndex].ToImage();
                    clearance.Applicant.FingerPrint.LeftMiddle.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftMiddle].ToImage();
                    clearance.Applicant.FingerPrint.LeftRing.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftRing].ToImage();
                    clearance.Applicant.FingerPrint.LeftPinky.Content = this.ViewModel.FingerScanner.FingerImages[FingerViewModel.LeftPinky].ToImage();
                    clearance.Applicant.Height = this.ViewModel.PersonalInformation.Height;
                    clearance.Applicant.Weight = this.ViewModel.PersonalInformation.Weight;
                    clearance.Applicant.AlsoKnownAs = this.ViewModel.PersonalInformation.AlsoKnownAs;
                    clearance.Applicant.BirthPlace = this.ViewModel.PersonalInformation.BirthPlace;
                    clearance.Applicant.Occupation = this.ViewModel.PersonalInformation.Occupation;
                    clearance.Applicant.Religion = this.ViewModel.PersonalInformation.Religion;
                    clearance.Applicant.Purpose = session.Load<Purpose>(this.ViewModel.PersonalInformation.Purpose.Id);

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

                    session.SaveOrUpdate(clearance);
                    transaction.Commit();

                    result.SerializeWith(clearance);
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message, "Clearance Application", MessageBoxButton.OK);
                return null;
            }
        }

        #endregion

        public virtual void Reset()
        {
            if (this.ViewModel.Camera != null)
                this.ViewModel.Camera.Stop.Execute(null);

            if (this.ViewModel.FingerScanner != null)
                this.ViewModel.FingerScanner.Stop.Execute(null);

            this.ViewModel.PersonalInformation = new PersonalInformationViewModel();
            this.ViewModel.Camera = new CameraViewModel();
            this.ViewModel.FingerScanner = new FingerScannerViewModel();
            this.ViewModel.Summary = new SummaryViewModel();

            this.ViewModel.ViewModels = new List<ViewModelBase>()
            {
                this.ViewModel.PersonalInformation,
                this.ViewModel.Camera,
                this.ViewModel.FingerScanner,
                this.ViewModel.Summary
            };

            if (!Properties.Settings.Default.WithFingerPrintDevice)
                this.ViewModel.ViewModels.Remove(this.ViewModel.FingerScanner);

            this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels.First();

            PopulateLookups();
        }

        public virtual void Previous()
        {
            var currentIndex = this.ViewModel.ViewModels.IndexOf(this.ViewModel.CurrentViewModel);
            this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels[currentIndex - 1];

            this.InitializeDevices();
        }

        public virtual void Next()
        {
            if (this.ViewModel.CurrentViewModel.IsValid == false)
            {
                MessageDialog.Show(this.ViewModel.CurrentViewModel.Error, "Application", MessageBoxButton.OK);
                return;
            }

            var currentIndex = this.ViewModel.ViewModels.IndexOf(this.ViewModel.CurrentViewModel);
            this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels[currentIndex + 1];

            this.InitializeDevices();
            if (this.ViewModel.CurrentViewModel == this.ViewModel.Summary)
            {
                Evaluate();
            }
        }

        public virtual void Release()
        {



            MessageDialog.Show("Clearance has been printed.", "Clearance", MessageBoxButton.OK);
        }

        //public virtual void Print()
        //{
        //    MessageDialog.Show("Clearance has been printed.", "Clearance", MessageBoxButton.OK);
        //}
    }
}
