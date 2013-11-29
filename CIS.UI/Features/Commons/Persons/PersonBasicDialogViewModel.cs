using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Commons.Persons
{
    public class PersonBasicDialogViewModel : ViewModelBase
    {
        private readonly PersonBasicDialogController _controller;

        public virtual PersonBasicViewModel Person { get; set; }

        public virtual IReactiveCommand Accept { get; set; }  

        public PersonBasicDialogViewModel()
        {
            this.Person = new PersonBasicViewModel();

            this.WhenAnyValue(x => x.Person.IsValid)
                .Subscribe(x => this.Revalidate());

            this.ObservableForProperty(x => x.Person.ActionResult)
                .Subscribe(x => this.ActionResult = x.Value);

            _controller = IoC.Container.Resolve<PersonBasicDialogController>(new ViewModelDependency(this));
        }
    }
}
