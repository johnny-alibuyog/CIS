using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    [HandleError]
    public class ImportController : ControllerBase<ImportViewModel>
    {
        private readonly BackgroundWorker _importWorker;

        public ImportController(ImportViewModel viewModel) : base(viewModel)
        {
            _importWorker = new System.ComponentModel.BackgroundWorker();
            _importWorker.DoWork += (sender, e) => Import();
            //_importWorker.ProgressChanged += (sender, e) => ProgressChanged(this.ViewModel));
            //_importWorker.RunWorkerCompleted += (sender, e) => RunWorkerCompleted(this.ViewModel));
            _importWorker.WorkerReportsProgress = true;
            _importWorker.WorkerSupportsCancellation = true;

            this.ViewModel.LookupPath = new ReactiveCommand();
            this.ViewModel.LookupPath.Subscribe(x => LookupPath());
            this.ViewModel.LookupPath.ThrownExceptions.Handle(this);

            this.ViewModel.Reset = new ReactiveCommand();
            this.ViewModel.Reset.Subscribe(x => Reset());
            this.ViewModel.Reset.ThrownExceptions.Handle(this);

            this.ViewModel.Import = new ReactiveCommand();
            this.ViewModel.Import.Subscribe(x => RunImportWorker());
            this.ViewModel.Import.ThrownExceptions.Handle(this);
        }

        public virtual void LookupPath()
        {
            var openDirectoryDialog = IoC.Container.Resolve<IOpenDirectoryDialogService>();
            var result = openDirectoryDialog.Show();
            if (result != null)
                this.ViewModel.SourcePath = result;
        }

        public virtual void Reset()
        {
            var confirmed = this.MessageBox.Confirm("Do you want to reset?", "Import");
            if (confirmed == false)
                return;

            this.ViewModel.SourcePath = string.Empty;
            this.ViewModel.ImportStart = null;
            this.ViewModel.ImportEnd = null;
            this.ViewModel.TotalTime = null;
            this.ViewModel.TotalTime = null;
            this.ViewModel.TotalLicenses = null;
            this.ViewModel.Status = string.Empty;
            this.ViewModel.ErrorMessage = string.Empty;
        }

        public virtual void RunImportWorker()
        {
            if (!Directory.Exists(this.ViewModel.SourcePath))
            {
                this.MessageBox.Inform("Please specify valid directory", "Import");
                return;
            }

            var confirmed = this.MessageBox.Confirm("Do you want to import from this directory?", "Import");
            if (confirmed == false)
                return;

            _importWorker.RunWorkerAsync();
        }

        public virtual void Import()
        {
            this.ViewModel.ImportStart = null;
            this.ViewModel.ImportEnd = null;
            this.ViewModel.TotalTime = null;
            this.ViewModel.TotalLicenses = 0M;
            this.ViewModel.ErrorMessage = string.Empty;

            var start = DateTime.Now;

            try
            {
                var service = (IImportService)IoC.Container.Resolve<ImportService>(new Dependency("viewModel", this.ViewModel));
                service.Execute();
            }
            catch (Exception ex)
            {
                this.ViewModel.ErrorMessage = ex.Message;
                this.Log.Error(ex.Message, ex);
            }

            var end = DateTime.Now;
            var duration = end - start;

            this.ViewModel.ImportStart = start;
            this.ViewModel.ImportEnd = end;
            this.ViewModel.TotalTime = duration;
        }
    }
}
