using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CIS.Store.Services.Warrants;
using CIS.UI.Bootstraps.InversionOfControl;
using Common.Logging;
using NHibernate;
using ServiceStack;

namespace CIS.UI.Features.Polices.Warrants
{
    public class WarrantStoreDataSyncronizer : IStoreDataSyncronizer
    {
        private static ILog _log = LogManager.GetCurrentClassLogger();

        private readonly ISessionFactory _sessionFactory;
        private readonly BackgroundWorker _worker;
        private readonly Timer _timer;
        private bool _workInProgress;

        public WarrantStoreDataSyncronizer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

            _worker = new BackgroundWorker();
            _worker.DoWork += (sender, e) => SyncronizeData();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;

            _timer = new Timer();
            _timer.Tick += (sender, e) => StartWork();
            _timer.Interval = 60000;
        }

        private void StartWork()
        {
            if (_workInProgress)
                return;

            _worker.RunWorkerAsync();
        }

        private void PullData()
        {
            var client = IoC.Container.Resolve<IRestClient>();

            var getRequest = new GetWarrantRequest();
            var getResponse = client.Get(getRequest);

            var insertRequest = new InsertWarrantRequest();
            var insertResponse = client.Put(insertRequest);

            var updateRequest = new UpdateWarrantRequest();
            var updateResponse = client.Patch(updateRequest);

            var deleteRequest = new DeleteWarrantRequest();
            var deleteResponse = client.Delete(deleteRequest);
        }

        private void PushData()
        {

        }

        public virtual bool WorkInProgress
        {
            get { return _workInProgress; }
        }

        public virtual void SyncronizeData()
        {
            try
            {
                _workInProgress = true;

                this.PullData();
                this.PushData();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.Message, ex);
            }
            finally
            {
                _workInProgress = false;
            }
        }
    }
}
