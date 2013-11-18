using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Commons.Persons
{
    public class BasicPersonDialogViewModel : ViewModelBase
    {
        private readonly BasicPersonDialogController _controller;

        public virtual BasicPersonViewModel Person { get; set; }

        public virtual IReactiveCommand Accept { get; set; }  

        public BasicPersonDialogViewModel()
        {
            this.Person = new BasicPersonViewModel();

            this.WhenAnyValue(x => x.Person.IsValid)
                .Subscribe(x => this.Revalidate());

            this.ObservableForProperty(x => x.Person.ActionResult)
                .Subscribe(x => this.ActionResult = x.Value);

            _controller = IoC.Container.Resolve<BasicPersonDialogController>(new ViewModelDependency(this));
        }
    }
}
