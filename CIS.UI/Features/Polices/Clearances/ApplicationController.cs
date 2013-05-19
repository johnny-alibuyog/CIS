using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            this.ViewModel.Print = new ReactiveCommand();
            this.ViewModel.Print.Subscribe(x => Print());

            this.ViewModel.Reset = new ReactiveCommand();
            this.ViewModel.Reset.Subscribe(x => Reset());

            this.ViewModel.Release = new ReactiveCommand();
            this.ViewModel.Release.Subscribe(x => Release());
        }

        #region Routine Helpers

        private void PopulateLookups()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var purposeQuery = session.Query<Purpose>().Cacheable().ToFuture();
                var officerQuery = session.Query<Officer>().Cacheable().ToFuture();
                var stationQuery = session.Query<Station>().Cacheable().ToFuture();

                this.ViewModel.PersonalInformation.Purposes = purposeQuery
                    .Select(x => x.Name)
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
                this.ViewModel.Summary.Validity = station.ClearanceValidity;
                this.ViewModel.PersonalInformation.Address.City = station.Address.City;
                this.ViewModel.PersonalInformation.Address.Province = station.Address.Province;

                transaction.Commit();
            }
        }

        #endregion

        public virtual void Reset()
        {
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

        public virtual void Release()
        {
            if (this.ViewModel.Camera != null)
                this.ViewModel.Camera.Stop.Execute(null);

            if (this.ViewModel.FingerScanner != null)
                this.ViewModel.FingerScanner.Stop.Execute(null);
        }

        public virtual void Previous()
        {
            var currentIndex = this.ViewModel.ViewModels.IndexOf(this.ViewModel.CurrentViewModel);
            this.ViewModel.CurrentViewModel = this.ViewModel.ViewModels[currentIndex - 1];

            this.HandleHardwareInteraction();
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

            this.HandleHardwareInteraction();
            if (this.ViewModel.CurrentViewModel == this.ViewModel.Summary)
            {
                Evaluate();
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

        public virtual void HandleHardwareInteraction()
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

        public virtual void Print()
        {
            MessageDialog.Show("Clearance has been printed.", "Clearance", MessageBoxButton.OK);
        }
    }
}
