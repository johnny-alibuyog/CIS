using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ArchiveController : ControllerBase<ArchiveViewModel>
    {
        public ArchiveController(ArchiveViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Search = new ReactiveCommand();
            this.ViewModel.Search.Subscribe(x => Search());

            this.ViewModel.ViewItem = new ReactiveCommand();
            this.ViewModel.ViewItem.Subscribe(x => ViewItem((ArchiveItemViewModel)x));

            this.ViewModel.ViewReport = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.Items, x => x.Value != null && x.Value.Count > 0));
            this.ViewModel.ViewReport.Subscribe(x => ViewReport());
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Clearance>();

                if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.FirstName))
                    query = query.Where(x => x.Applicant.Person.FirstName.StartsWith(this.ViewModel.Criteria.FirstName));

                if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.MiddleName))
                    query = query.Where(x => x.Applicant.Person.MiddleName.StartsWith(this.ViewModel.Criteria.MiddleName));

                if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.LastName))
                    query = query.Where(x => x.Applicant.Person.LastName.StartsWith(this.ViewModel.Criteria.LastName));

                if (this.ViewModel.Criteria.FilterDate)
                    query = query.Where(x => x.IssueDate >= this.ViewModel.Criteria.FromDate && x.IssueDate <= this.ViewModel.Criteria.ToDate);

                this.ViewModel.Items = query
                    .Select(x => new ArchiveItemViewModel()
                    {
                        Id = x.Id,
                        IssueDate = x.IssueDate,
                        Applicant =
                            x.Applicant.Person.FirstName + " " +
                            x.Applicant.Person.MiddleName + " " +
                            x.Applicant.Person.LastName,
                        Purpose = x.Applicant.Purpose.Name
                    })
                    .ToList();

                transaction.Commit();
            }
        }

        public virtual void ViewItem(ArchiveItemViewModel item)
        {
            var reportData = new ClearanceReportViewModel();

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var clearanceAlias = (Clearance)null;
                var applicantAlias = (Applicant)null;
                var pictureAlias = (ImageBlob)null;
                var fingerPrintAlias = (FingerPrint)null;
                var stationAlias = (Station)null;
                var verifierAlias = (Officer)null;
                var certifierAlias = (Officer)null;
                var barcodeAlais = (Barcode)null;

                var clearanceQuery = session.QueryOver<Clearance>(() => clearanceAlias)
                    .Left.JoinAlias(() => clearanceAlias.Applicant, () => applicantAlias)
                    .Left.JoinAlias(() => clearanceAlias.Station, () => stationAlias)
                    .Left.JoinAlias(() => clearanceAlias.Verifier, () => verifierAlias)
                    .Left.JoinAlias(() => clearanceAlias.Certifier, () => certifierAlias)
                    .Left.JoinAlias(() => clearanceAlias.Barcode, () => barcodeAlais)
                    .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
                    .Left.JoinAlias(() => applicantAlias.Picture, () => pictureAlias)
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
                    .Where(() => clearanceAlias.Id == item.Id)
                    .FutureValue();

                var clearance = clearanceQuery.Value;
                if (clearance == null)
                    clearance = new Clearance();

                reportData.SerializeWith(clearance);
                transaction.Commit();
            }

            var dialog = new DialogService<ClearanceReportView, ClearanceReportViewModel>();
            dialog.Show(this, "Clearance", reportData);
        }

        public virtual void ViewReport()
        {
            var dialog = new DialogService<ArchiveReportView, ArchiveReportViewModel>();
            dialog.ViewModel.Items = this.ViewModel.Items;
            dialog.Show(this, "Archive", null);
        }
    }
}
