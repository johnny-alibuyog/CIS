using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class SuspectController : ControllerBase<SuspectViewModel>
    {
        public SuspectController(SuspectViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.AddAlias = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.AliasToAdd, x => !string.IsNullOrWhiteSpace(x.Value)));
            this.ViewModel.AddAlias.Subscribe(x => AddAlias());

            this.ViewModel.DeleteAlias = new ReactiveCommand();
            this.ViewModel.DeleteAlias.Subscribe(x => DeleteAlias((string)x));

            this.ViewModel.AddOccupation = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.OccupationToAdd, x => !string.IsNullOrWhiteSpace(x.Value)));
            this.ViewModel.AddOccupation.Subscribe(x => AddOccupation());

            this.ViewModel.DeleteOccupation = new ReactiveCommand();
            this.ViewModel.DeleteOccupation.Subscribe(x => DeleteOccupation((string)x));

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel.IsValidObservable());
            this.ViewModel.Save.Subscribe(x => Save());
        }

        public virtual void AddAlias()
        {
            this.ViewModel.Aliases.Add(this.ViewModel.AliasToAdd);
            this.ViewModel.AliasToAdd = string.Empty;
        }

        public virtual void DeleteAlias(string item)
        {
            this.ViewModel.Aliases.Remove(item);
        }

        public virtual void AddOccupation()
        {
            this.ViewModel.Occupations.Add(this.ViewModel.OccupationToAdd);
            this.ViewModel.OccupationToAdd = string.Empty;
        }

        public virtual void DeleteOccupation(string item)
        {
            this.ViewModel.Occupations.Remove(item);
        }

        public virtual void Save()
        {
            if (!string.IsNullOrWhiteSpace(this.ViewModel.AliasToAdd))
                this.AddAlias();

            if (!string.IsNullOrWhiteSpace(this.ViewModel.OccupationToAdd))
                this.AddOccupation();

            this.ViewModel.Close();
        }
    }
}
