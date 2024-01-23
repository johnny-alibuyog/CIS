using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate;
using System;
using System.Linq;

namespace CIS.UI.Features.Polices.Clearances.Applications;

public class ClearanceReportViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }
    public virtual string Applicant { get; set; }
    public virtual DateTime? BirthDate { get; set; }
    public virtual string BirthPlace { get; set; }
    public virtual string Gender { get; set; }
    public virtual string CivilStatus { get; set; }
    public virtual string Citizenship { get; set; }
    public virtual string Address { get; set; }
    public virtual byte[] Picture { get; set; }
    public virtual byte[] Signature { get; set; }
    public virtual byte[] RightThumb { get; set; }
    public virtual byte[] LeftThumb { get; set; }
    public virtual string Purpose { get; set; }
    public virtual string Validity { get; set; }
    public virtual byte[] Barcode { get; set; }
    public virtual string BarcodeText { get; set; }
    public virtual string Verifier { get; set; }
    public virtual string VerifierRank { get; set; }
    public virtual string VerifierPosition { get; set; }
    public virtual byte[] VerifierSignature { get; set; }
    public virtual string Certifier { get; set; }
    public virtual string CertifierRank { get; set; }
    public virtual string CertifierPosition { get; set; }
    public virtual byte[] CertifierSignature { get; set; }
    public virtual string Issuer { get; set; }
    public virtual string IssueAddress { get; set; }
    public virtual DateTime? ApplicationDate { get; set; }
    public virtual DateTime? IssueDate { get; set; }
    public virtual byte[] Logo { get; set; }
    public virtual string Office { get; set; }
    public virtual string Station { get; set; }
    public virtual string Location { get; set; }
    public virtual string OfficialReceiptNumber { get; set; }
    public virtual string TaxCertificateNumber { get; set; }
    public virtual string FinalFindings { get; set; }
    public virtual decimal? Fee { get; set; }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is ClearanceReportViewModel)
        {
            var target = this;
            var source = instance as ClearanceReportViewModel;

            target.Id = source.Id;
            target.Applicant = source.Applicant;
            target.BirthDate = source.BirthDate;
            target.BirthPlace = source.BirthPlace;
            target.Gender = source.Gender;
            target.CivilStatus = source.CivilStatus;
            target.Citizenship = source.Citizenship;
            target.Address = source.Address;
            target.Picture = source.Picture;
            target.Signature = source.Signature;
            target.RightThumb = source.RightThumb;
            target.LeftThumb = source.LeftThumb;
            target.Purpose = source.Purpose;
            target.Validity = source.Validity;
            target.Barcode = source.Barcode;
            target.BarcodeText = source.BarcodeText;
            target.Verifier = source.Verifier;
            target.VerifierRank = source.VerifierRank;
            target.VerifierPosition = source.VerifierPosition;
            target.VerifierSignature = source.VerifierSignature;
            target.Certifier = source.Certifier;
            target.CertifierRank = source.CertifierRank;
            target.CertifierPosition = source.CertifierPosition;
            target.CertifierSignature = source.CertifierSignature;
            target.Issuer = source.Issuer;
            target.ApplicationDate = source.ApplicationDate;
            target.IssueDate = source.IssueDate;
            target.IssueAddress = source.IssueAddress;
            target.Logo = source.Logo;
            target.Station = source.Station;
            target.Office = source.Office;
            target.Location = source.Location;
            target.OfficialReceiptNumber = source.OfficialReceiptNumber;
            target.TaxCertificateNumber = source.TaxCertificateNumber;
            target.FinalFindings = source.FinalFindings;
            target.Fee = source.Fee;

            return target;
        }
        else if (instance is Clearance)
        {
            var target = this;
            var source = instance as Clearance;

            target.Id = source.Id;
            target.Applicant = source.Applicant.Person.Fullname;
            target.BirthDate = source.Applicant.Person.BirthDate;
            target.BirthPlace = source.Applicant.BirthPlace;
            target.Gender = source.Applicant.Person.Gender.ToString();
            target.CivilStatus = source.ApplicantCivilStatus.Value.ToString();
            target.Citizenship = source.ApplicantCitizenship;
            target.Address = source.ApplicantAddress;
            target.Picture = source.ApplicantPicture.Bytes;
            target.Signature = source.ApplicantSignature.Bytes;
            target.RightThumb = source.Applicant.FingerPrint.RightThumb.Bytes;
            target.LeftThumb = source.Applicant.FingerPrint.LeftThumb.Bytes;
            target.Purpose = source.Purpose.Name;
            target.Validity = source.Validity;
            target.Barcode = source.Barcode.Image.Bytes;
            target.BarcodeText = source.Barcode.Text;
            target.Verifier = source.Verifier.Person.Fullname;
            target.VerifierRank = source.VerifierRank;
            target.VerifierPosition = source.VerifierPosition;
            target.VerifierSignature = source.Verifier.Signature.Bytes;
            target.Certifier = source.Certifier.Person.Fullname;
            target.CertifierRank = source.CertifierRank;
            target.CertifierPosition = source.CertifierPosition;
            target.CertifierSignature = source.Certifier.Signature.Bytes;
            target.Issuer = source.Audit.UpdatedBy != null ? source.Audit.UpdatedBy : source.Audit.CreatedBy;
            target.ApplicationDate = source.ApplicationDate;
            target.IssueDate = source.IssueDate;
            target.IssueAddress = source.Station.Address.ToString();
            target.Logo = source.Station.Logo.Bytes;
            target.Station = source.Station.Name;
            target.Office = source.Station.Office;
            target.Station = source.Station.Name;
            target.Location = source.Station.Location;
            target.OfficialReceiptNumber = source.OfficialReceiptNumber;
            target.TaxCertificateNumber = source.TaxCertificateNumber;
            target.FinalFindings = source.FinalFindings;
            target.Fee = source.Fee;

            return target;
        }

        return null;
    }

    public static ClearanceReportViewModel GetTestData()
    {
        var result = new ClearanceReportViewModel();

        using (var session = IoC.Container.Resolve<ISessionFactory>().OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var clearanceAlias = (Clearance)null;
            var applicantAlias = (Applicant)null;
            //var pictureAlias = (ImageBlob)null;
            var fingerPrintAlias = (FingerPrint)null;
            var verifierAlias = (Officer)null;
            var certifierAlias = (Officer)null;

            var clearanceQuery = session.QueryOver<Clearance>(() => clearanceAlias)
                .Left.JoinAlias(() => clearanceAlias.Applicant, () => applicantAlias)
                .Left.JoinAlias(() => clearanceAlias.Verifier, () => verifierAlias)
                .Left.JoinAlias(() => clearanceAlias.Certifier, () => certifierAlias)
                .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
                //.Left.JoinAlias(() => applicantAlias.Picture, () => pictureAlias)
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
                .Left.JoinQueryOver(() => clearanceAlias.ApplicantPicture)
                .Left.JoinQueryOver(() => clearanceAlias.ApplicantSignature)
                .Future();



            var stationQuery = session.QueryOver<Station>()
                .Left.JoinQueryOver(x => x.Logo)
                .Future();

            var clearance = clearanceQuery.LastOrDefault();
            if (clearance != null)
                result.SerializeWith(clearance);

            transaction.Commit();
        }

        return result;
    }
}
