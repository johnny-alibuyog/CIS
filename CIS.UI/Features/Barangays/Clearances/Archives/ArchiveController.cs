﻿using CIS.Core.Entities.Barangays;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Linq;

namespace CIS.UI.Features.Barangays.Clearances.Archives;

public class ArchiveController : ControllerBase<ArchiveViewModel>
{
    public ArchiveController(ArchiveViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.Criteria = new ArchiveCriteriaViewModel();

        this.ViewModel.Search = new ReactiveCommand();
        this.ViewModel.Search.Subscribe(x => Search());
        this.ViewModel.Search.ThrownExceptions.Handle(this);

        this.ViewModel.ViewClearance = new ReactiveCommand();
        this.ViewModel.ViewClearance.Subscribe(x => ViewClearance((ArchiveItemViewModel)x));
        this.ViewModel.ViewClearance.ThrownExceptions.Handle(this);

        this.ViewModel.ViewApplicant = new ReactiveCommand();
        this.ViewModel.ViewApplicant.Subscribe(x => ViewApplicant((ArchiveItemViewModel)x));
        this.ViewModel.ViewApplicant.ThrownExceptions.Handle(this);

        this.ViewModel.GenerateListReport = new ReactiveCommand(
            this.ViewModel.WhenAny(
                x => x.Items,
                x => x.Value != null && x.Value.Count > 0
            )
        );
        this.ViewModel.GenerateListReport.Subscribe(x => GenerateListReport());
        this.ViewModel.GenerateListReport.ThrownExceptions.Handle(this);
    }

    public virtual void Search()
    {
        using var session = this.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
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
                ControlNumber = x.ControlNumber,
                OfficalReceiptNumber = x.OfficialReceiptNumber,
                Applicant =
                    x.Applicant.Person.FirstName + " " +
                    x.Applicant.Person.MiddleName + " " +
                    x.Applicant.Person.LastName,
                Purpose = x.Purpose.Name
            })
            .ToList();

        transaction.Commit();
    }

    public virtual void ViewApplicant(ArchiveItemViewModel item)
    {
        //var reportData = new ApplicantReportViewModel();

        //using (var session = this.SessionFactory.OpenSession())
        //using (var transaction = session.BeginTransaction())
        //{
        //    var clearanceAlias = (Clearance)null;
        //    var applicantAlias = (Applicant)null;
        //    var fingerPrintAlias = (FingerPrint)null;

        //    var clearanceQuery = session.QueryOver<Clearance>(() => clearanceAlias)
        //        .Left.JoinAlias(() => clearanceAlias.Applicant, () => applicantAlias)
        //        .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
        //        .Left.JoinQueryOver(() => clearanceAlias.ApplicantPicture)
        //        .Left.JoinQueryOver(() => clearanceAlias.ApplicantSignature)
        //        .Left.JoinQueryOver(() => applicantAlias.Relatives)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightThumb)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightIndex)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightMiddle)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightRing)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightPinky)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftThumb)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftIndex)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftMiddle)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftRing)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftPinky)
        //        .Where(() => clearanceAlias.Id == item.Id)
        //        .FutureValue();

        //    var clearance = clearanceQuery.Value;
        //    if (clearance == null)
        //        clearance = new Clearance();

        //    reportData.SerializeWith(clearance);
        //    transaction.Commit();
        //}

        //var dialog = new DialogService<ApplicantReportViewModel>();
        //dialog.ShowModal(this, "Applicant", reportData);
    }

    public virtual void ViewClearance(ArchiveItemViewModel item)
    {
        //var reportData = new ClearanceReportViewModel();

        //using (var session = this.SessionFactory.OpenSession())
        //using (var transaction = session.BeginTransaction())
        //{
        //    var clearanceAlias = (Clearance)null;
        //    var applicantAlias = (Applicant)null;
        //    var fingerPrintAlias = (FingerPrint)null;
        //    var stationAlias = (Station)null;
        //    var verifierAlias = (Officer)null;
        //    var certifierAlias = (Officer)null;
        //    var barcodeAlais = (Barcode)null;

        //    var clearanceQuery = session.QueryOver<Clearance>(() => clearanceAlias)
        //        .Left.JoinAlias(() => clearanceAlias.Applicant, () => applicantAlias)
        //        .Left.JoinAlias(() => clearanceAlias.Station, () => stationAlias)
        //        .Left.JoinAlias(() => clearanceAlias.Verifier, () => verifierAlias)
        //        .Left.JoinAlias(() => clearanceAlias.Certifier, () => certifierAlias)
        //        .Left.JoinAlias(() => clearanceAlias.Barcode, () => barcodeAlais)
        //        .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
        //        .Left.JoinQueryOver(() => stationAlias.Logo)
        //        .Left.JoinQueryOver(() => barcodeAlais.Image)
        //        .Left.JoinQueryOver(() => verifierAlias.Signature)
        //        .Left.JoinQueryOver(() => certifierAlias.Signature)
        //        .Left.JoinQueryOver(() => clearanceAlias.Purpose)
        //        .Left.JoinQueryOver(() => clearanceAlias.ApplicantPicture)
        //        .Left.JoinQueryOver(() => clearanceAlias.ApplicantSignature)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightThumb)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightIndex)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightMiddle)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightRing)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.RightPinky)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftThumb)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftIndex)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftMiddle)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftRing)
        //        .Left.JoinQueryOver(() => fingerPrintAlias.LeftPinky)
        //        .Where(() => clearanceAlias.Id == item.Id)
        //        .FutureValue();

        //    var clearance = clearanceQuery.Value;
        //    if (clearance == null)
        //        clearance = new Clearance();

        //    reportData.SerializeWith(clearance);
        //    transaction.Commit();
        //}

        //var dialog = new DialogService<ClearanceReportViewModel>();
        //dialog.ShowModal(this, "Clearance", reportData);
    }

    public virtual void GenerateListReport()
    {
        var office = (Office)null;

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var stationQuery = session.QueryOver<Office>().Cacheable().List();
            office = stationQuery.FirstOrDefault();
            transaction.Commit();
        }

        var viewModel = IoC.Container.Resolve<ArchiveReportViewModel>();
        viewModel.Station = office.Name.ToUpper();
        viewModel.Office = office.Name.ToUpper();
        viewModel.Location = office.Location.ToUpper();
        viewModel.FromDate = this.ViewModel.Criteria.FromDate;
        viewModel.ToDate = this.ViewModel.Criteria.ToDate;
        viewModel.FilterDate = this.ViewModel.Criteria.FilterDate;
        viewModel.Items = this.ViewModel.Items;

        var dialog = new DialogService<ArchiveReportViewModel>();
        dialog.ViewModel = viewModel;
        dialog.ShowModal(this, "Archive");
    }
}
