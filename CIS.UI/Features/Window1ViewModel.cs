using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features
{
    public class Window1ViewModel : ViewModelBase
    {
        private readonly Window1Controller _controller;

        [NotNullNotEmpty]
        public virtual string Text1 { get; set; }

        [NotNullNotEmpty]
        public virtual string Text2 { get; set; }

        [NotNullNotEmpty]
        public virtual string Text3 { get; set; }

        [Valid]
        public virtual Window1ChildViewModel Child { get; set; }

        public virtual IReactiveCommand Submit { get; set; }

        public Window1ViewModel()
        {
            this.WhenAnyValue(x => x.Child.IsValid)
                .Subscribe(x => this.Revalidate());

            this.Child = IoC.Container.Resolve<Window1ChildViewModel>();
            _controller = IoC.Container.Resolve<Window1Controller>(new Dependency("viewModel", this));
            //this.Submit = new ReactiveCommand(this.IsValidObservable());
            //this.Submit.Subscribe(x => System.Windows.MessageBox.Show("Hey man."));
        }
    }
}
