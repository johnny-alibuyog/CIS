using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Polices;
using CIS.UI.Utilities.Extentions;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ClearanceReportViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual string Applicant { get; set; }
        public virtual string CivilStatus { get; set; }
        public virtual string Citizenship { get; set; }
        public virtual string Address { get; set; }
        public virtual BitmapSource Picture { get; set; }
        public virtual BitmapSource RightThumb { get; set; }
        public virtual BitmapSource LeftThumb { get; set; }
        public virtual string Purpose { get; set; }
        public virtual BitmapSource Barcode { get; set; }
        public virtual BitmapSource Logo { get; set; }
        public virtual string Verifier { get; set; }
        public virtual string VerifierRank { get; set; }
        public virtual string VerifierPosition { get; set; }
        public virtual BitmapSource VerifierSignature { get; set; }
        public virtual string Certifier { get; set; }
        public virtual string CertifierRank { get; set; }
        public virtual string CertifierPosition { get; set; }
        public virtual BitmapSource CertifierSignature { get; set; }
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
            target.Address = source.Applicant.Address.ToString();
            target.CivilStatus = source.Applicant.CivilStatus.ToString();
            target.Citizenship = source.Applicant.Citizenship;
            target.Picture = source.Applicant.Picture.Image.Content.ToBitmapSource();
            target.RightThumb = source.Applicant.FingerPrint.RightThumb.Content.ToBitmapSource();
            target.LeftThumb = source.Applicant.FingerPrint.LeftThumb.Content.ToBitmapSource();
            target.Purpose = source.Applicant.Purpose.Name;
            target.Barcode = source.Barcode.Image.Content.ToBitmapSource();
            target.Logo = source.Station.Logo.Image.Content.ToBitmapSource();
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
    }
}
