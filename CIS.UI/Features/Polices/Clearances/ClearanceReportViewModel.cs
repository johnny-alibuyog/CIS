using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.DependencyInjection;
using CIS.UI.Utilities.Extentions;
using NHibernate;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ClearanceReportViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual string Applicant { get; set; }
        public virtual Nullable<DateTime> BirthDate { get; set; }
        public virtual string BirthPlace { get; set; }
        public virtual string Gender { get; set; }
        public virtual string CivilStatus { get; set; }
        public virtual string Citizenship { get; set; }
        public virtual string Address { get; set; }
        public virtual byte[] Picture { get; set; }
        public virtual byte[] RightThumb { get; set; }
        public virtual byte[] LeftThumb { get; set; }
        public virtual string Purpose { get; set; }
        public virtual byte[] Barcode { get; set; }
        public virtual string BarcodeText { get; set; }
        public virtual byte[] Logo { get; set; }
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
        public virtual DateTime IssueDate { get; set; }
        public virtual string Office { get; set; }
        public virtual string Station { get; set; }
        public virtual string Location { get; set; }
        public virtual string OfficialReceiptNumber { get; set; }
        public virtual string TaxCertificateNumber { get; set; }
        public virtual string PartialMatchFindings { get; set; }
        public virtual string PerfectMatchFindings { get; set; }
        public virtual string FinalFindings { get; set; }

        public override object SerializeWith(object instance)
        {
            var target = this;
            var source = instance as Clearance;
            if (source == null) 
                return null;

            target.Id = source.Id;
            target.Applicant = source.Applicant.Person.Fullname;
            target.BirthDate = source.Applicant.Person.BirthDate;
            target.BirthPlace = source.Applicant.BirthPlace;
            target.Gender = source.Applicant.Person.Gender.ToString();
            target.CivilStatus = source.Applicant.CivilStatus.ToString();
            target.Citizenship = source.Applicant.Citizenship;
            target.Address = source.Applicant.Address.ToString();
            target.Picture = source.Applicant.Picture.Image.Data;
            target.RightThumb = source.Applicant.FingerPrint.RightThumb.Data;
            target.LeftThumb = source.Applicant.FingerPrint.LeftThumb.Data;
            target.Purpose = source.Applicant.Purpose.Name;
            target.Barcode = source.Barcode.Image.Data;
            target.BarcodeText = source.Barcode.Text;
            target.Logo = source.Station.Logo.Image.Data;
            target.Verifier = source.Verifier.Person.Fullname;
            target.VerifierRank = source.VerifierRank;
            target.VerifierPosition = source.VerifierPosition;
            target.Certifier = source.Certifier.Person.Fullname;
            target.CertifierRank = source.CertifierRank;
            target.CertifierPosition = source.CertifierPosition;
            target.Issuer = source.Audit.CreatedBy;
            target.IssueDate = source.IssueDate;
            target.IssueAddress = source.Station.Address.ToString();
            target.Office = source.Station.Office;
            target.Location = source.Station.Location;
            target.OfficialReceiptNumber = source.OfficialReceiptNumber;
            target.TaxCertificateNumber = source.TaxCertificateNumber;
            target.PartialMatchFindings = source.PartialMatchFindings;
            target.PerfectMatchFindings = source.PerfectMatchFindings;
            target.FinalFindings = source.FinalFindings;

            return target;
        }

        public static ClearanceReportViewModel GetTestData()
        {
            var result = new ClearanceReportViewModel();

            using (var session = IoC.Container.Resolve<ISessionFactory>().OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var clearanceAlias = (Clearance)null;
                var applicantAlias = (Applicant)null;
                var pictureAlias = (Picture)null;
                var fingerPrintAlias = (FingerPrint)null;
                var verifierAlias = (Officer)null;
                var certifierAlias = (Officer)null;

                var clearanceQuery = session.QueryOver<Clearance>(() => clearanceAlias)
                    .Left.JoinAlias(() => clearanceAlias.Applicant, () => applicantAlias)
                    .Left.JoinAlias(() => clearanceAlias.Verifier, () => verifierAlias)
                    .Left.JoinAlias(() => clearanceAlias.Certifier, () => certifierAlias)
                    .Left.JoinAlias(() => applicantAlias.FingerPrint, () => fingerPrintAlias)
                    .Left.JoinAlias(() => applicantAlias.Picture, () => pictureAlias)
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
                    .Left.JoinQueryOver(() => pictureAlias.Image)
                    .Future();

                var stationQuery = session.QueryOver<Station>()
                    .Left.JoinQueryOver(x => x.Logo)
                    .Left.JoinQueryOver(x => x.Image)
                    .Future();

                var clearance = clearanceQuery.LastOrDefault();
                if (clearance != null)
                {
                    result.SerializeWith(clearance);
                }

                transaction.Commit();
            }

            return result;
        }
    }
}
