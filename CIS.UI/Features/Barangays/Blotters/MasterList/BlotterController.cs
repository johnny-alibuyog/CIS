using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using CIS.UI.Features.Barangays.Citizens;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Blotters.MasterList
{
    public class BlotterController : ControllerBase<BlotterViewModel>
    {
        public BlotterController(BlotterViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Load((Guid)x));
            this.ViewModel.Load.ThrownExceptions.Handle(this);

            this.ViewModel.CreateComplainant = new ReactiveCommand();
            this.ViewModel.CreateComplainant.Subscribe(x => CreateComplainant());
            this.ViewModel.CreateComplainant.ThrownExceptions.Handle(this);

            this.ViewModel.EditComplainant = new ReactiveCommand();
            this.ViewModel.EditComplainant.Subscribe(x => EditComplainant((BlotterCitizenViewModel)x));
            this.ViewModel.EditComplainant.ThrownExceptions.Handle(this);

            this.ViewModel.DeleteComplainant = new ReactiveCommand();
            this.ViewModel.DeleteComplainant.Subscribe(x => DeleteComplainant((BlotterCitizenViewModel)x));
            this.ViewModel.DeleteComplainant.ThrownExceptions.Handle(this);

            this.ViewModel.CreateRespondent = new ReactiveCommand();
            this.ViewModel.CreateRespondent.Subscribe(x => CreateRespondent());
            this.ViewModel.CreateRespondent.ThrownExceptions.Handle(this);

            this.ViewModel.EditRespondent = new ReactiveCommand();
            this.ViewModel.EditRespondent.Subscribe(x => EditRespondent((BlotterCitizenViewModel)x));
            this.ViewModel.EditRespondent.ThrownExceptions.Handle(this);

            this.ViewModel.DeleteRespondent = new ReactiveCommand();
            this.ViewModel.DeleteRespondent.Subscribe(x => DeleteRespondent((BlotterCitizenViewModel)x));
            this.ViewModel.DeleteRespondent.ThrownExceptions.Handle(this);

            this.ViewModel.CreateWitness = new ReactiveCommand();
            this.ViewModel.CreateWitness.Subscribe(x => CreateWitness());
            this.ViewModel.CreateWitness.ThrownExceptions.Handle(this);

            this.ViewModel.EditWitness = new ReactiveCommand();
            this.ViewModel.EditWitness.Subscribe(x => EditWitness((BlotterCitizenViewModel)x));
            this.ViewModel.EditWitness.ThrownExceptions.Handle(this);

            this.ViewModel.DeleteWitness = new ReactiveCommand();
            this.ViewModel.DeleteWitness.Subscribe(x => DeleteWitness((BlotterCitizenViewModel)x));
            this.ViewModel.DeleteWitness.ThrownExceptions.Handle(this);

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel.IsValidObservable());
            this.ViewModel.Save.Subscribe(x => Save());
            this.ViewModel.Save.ThrownExceptions.Handle(this);

            this.PopulateLookup(Guid.Empty);
        }

        private void PopulateLookup(Guid incumbentId)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                if (incumbentId == Guid.Empty)
                {
                    incumbentId = session.Query<Incumbent>()
                        .OrderByDescending(x => x.Date)
                        .Select(x => x.Id)
                        .FirstOrDefault();
                }

                var query = session.Query<Incumbent>()
                    .Where(x => x.Id == incumbentId)
                    .FetchMany(x => x.Officials)
                    .ThenFetch(x => x.Position)
                    .ToFutureValue();

                var incumbent = query.Value;
                if (incumbent != null)
                {
                    this.ViewModel.Officials = incumbent.Officials
                        .Where(x => x.IsActive)
                        .Select(x => new BlotterOfficialViewModel()
                        {
                            Id = x.Id,
                            Selected = false,
                            Name = x.Person.Fullname,
                            Position = x.Position.Name,
                        })
                        .OrderBy(x => x.Position)
                        .ThenBy(x => x.Name)
                        .ToReactiveList();
                }

                transaction.Commit();
            }
        }

        public virtual void CreateComplainant()
        {
            var dialog = new DialogService<CitizenDialogViewModel>();
            var value = dialog.ShowModal(this, "New Complainant");
            if (value != null)
            {
                this.ViewModel.Complainants.Add(new BlotterCitizenViewModel()
                {
                    Id = value.Citizen.Id,
                    Name = value.Citizen.Person.FullName,
                    Gender = value.Citizen.Person.Gender
                });
            }
        }

        public virtual void EditComplainant(BlotterCitizenViewModel item)
        {
            this.ViewModel.SelectedComplainant = item;

            var dialog = new DialogService<CitizenDialogViewModel>();
            dialog.ViewModel.Citizen.Load.Execute(item.Id);

            var value = dialog.ShowModal(this, "Edit Complainant");
            if (value != null)
            {
                this.ViewModel.SelectedComplainant.Id = value.Citizen.Id;
                this.ViewModel.SelectedComplainant.Name = value.Citizen.Person.FullName;
                this.ViewModel.SelectedComplainant.Gender = value.Citizen.Person.Gender;
            }
        }

        public virtual void DeleteComplainant(BlotterCitizenViewModel item)
        {
            this.ViewModel.Complainants.Remove(item);
            this.ViewModel.SelectedComplainant = null;
        }

        public virtual void CreateRespondent()
        {
            var dialog = new DialogService<CitizenDialogViewModel>();
            var value = dialog.ShowModal(this, "New Respondent");
            if (value != null)
            {
                this.ViewModel.Respondents.Add(new BlotterCitizenViewModel()
                {
                    Id = value.Citizen.Id,
                    Name = value.Citizen.Person.FullName,
                    Gender = value.Citizen.Person.Gender
                });
            }
        }

        public virtual void EditRespondent(BlotterCitizenViewModel item)
        {
            this.ViewModel.SelectedRespondent = item;

            var dialog = new DialogService<CitizenDialogViewModel>();
            dialog.ViewModel.Citizen.Load.Execute(item.Id);

            var value = dialog.ShowModal(this, "Edit Respondent");
            if (value != null)
            {
                this.ViewModel.SelectedRespondent.Id = value.Citizen.Id;
                this.ViewModel.SelectedRespondent.Name = value.Citizen.Person.FullName;
                this.ViewModel.SelectedRespondent.Gender = value.Citizen.Person.Gender;
            }
        }

        public virtual void DeleteRespondent(BlotterCitizenViewModel item)
        {
            this.ViewModel.Respondents.Remove(item);
            this.ViewModel.SelectedRespondent = null;
        }

        public virtual void CreateWitness()
        {
            var dialog = new DialogService<CitizenDialogViewModel>();
            var value = dialog.ShowModal(this, "New Witness");
            if (value != null)
            {
                this.ViewModel.Witnesses.Add(new BlotterCitizenViewModel()
                {
                    Id = value.Citizen.Id,
                    Name = value.Citizen.Person.FullName,
                    Gender = value.Citizen.Person.Gender
                });
            }
        }

        public virtual void EditWitness(BlotterCitizenViewModel item)
        {
            this.ViewModel.SelectedWitness = item;

            var dialog = new DialogService<CitizenDialogViewModel>();
            dialog.ViewModel.Citizen.Load.Execute(item.Id);

            var value = dialog.ShowModal(this, "Edit Witness");
            if (value != null)
            {
                this.ViewModel.SelectedWitness.Id = value.Citizen.Id;
                this.ViewModel.SelectedWitness.Name = value.Citizen.Person.FullName;
                this.ViewModel.SelectedWitness.Gender = value.Citizen.Person.Gender;
            }
        }

        public virtual void DeleteWitness(BlotterCitizenViewModel item)
        {
            this.ViewModel.Witnesses.Remove(item);
            this.ViewModel.SelectedWitness = null;
        }

        public virtual void Load(Guid id)
        {
            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Blotter>()
                    .Where(x => x.Id == id)
                    .Fetch(x => x.Incumbent)
                    .FetchMany(x => x.Officials)
                    .ThenFetch(x => x.Position)
                    .ToFutureValue();

                session.Query<Blotter>()
                    .Where(x => x.Id == id)
                    .FetchMany(x => x.Complainants)
                    .ToFutureValue();

                session.Query<Blotter>()
                    .Where(x => x.Id == id)
                    .FetchMany(x => x.Respondents)
                    .ToFutureValue();

                session.Query<Blotter>()
                    .Where(x => x.Id == id)
                    .FetchMany(x => x.Witnesses)
                    .ToFutureValue();

                var blotter = query.Value;

                this.PopulateLookup(blotter.Incumbent.Id);
                this.ViewModel.SerializeWith(blotter);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }


        }

        public virtual void Save()
        {
            var message = string.Format("Do you want to save warrant?");
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;


            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Blotter>()
                    .Where(x => x.Id == this.ViewModel.Id)
                    .Fetch(x => x.Incumbent)
                    .FetchMany(x => x.Officials)
                    .ThenFetch(x => x.Position)
                    .ToFutureValue();

                session.Query<Blotter>()
                    .Where(x => x.Id == this.ViewModel.Id)
                    .FetchMany(x => x.Complainants)
                    .ToFutureValue();

                session.Query<Blotter>()
                    .Where(x => x.Id == this.ViewModel.Id)
                    .FetchMany(x => x.Respondents)
                    .ToFutureValue();

                session.Query<Blotter>()
                    .Where(x => x.Id == this.ViewModel.Id)
                    .FetchMany(x => x.Witnesses)
                    .ToFutureValue();

                var blotter = query.Value;

                this.ViewModel.DeserializeInto(blotter);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBox.Inform("Save has been successfully completed.");

            this.ViewModel.Close();
        }
    }
}
