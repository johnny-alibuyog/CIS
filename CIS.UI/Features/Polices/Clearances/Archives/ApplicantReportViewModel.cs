using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.Core.Utilities.Extentions;

namespace CIS.UI.Features.Polices.Clearances.Archives
{
    public class ApplicantReportViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual string ApplicantName { get; set; }
        public virtual string ApplicantNickname { get; set; }
        public virtual string CityAddress { get; set; }
        public virtual string ProvincialAddress { get; set; }
        public virtual Nullable<DateTime> BirthDate { get; set; }
        public virtual string BirthPlace { get; set; }
        public virtual Nullable<int> AgeUponApplication { get; set; }
        public virtual string Father { get; set; }
        public virtual string Mother { get; set; }
        public virtual string Relatives { get; set; }
        public virtual string CivilStatus { get; set; }
        public virtual string Religion { get; set; }
        public virtual string Occupation { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string TelephoneNumber { get; set; }
        public virtual string CellphoneNumber { get; set; }
        public virtual string TaxCertificateNumber { get; set; }
        public virtual string PassportNumber { get; set; }
        public virtual string TaxIdentificationNumber { get; set; }
        public virtual string SocialSecuritySystemNumber { get; set; }
        public virtual Nullable<int> YearsOfResidency { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Build { get; set; }
        public virtual string Marks { get; set; }
        public virtual string Height { get; set; }
        public virtual string Weight { get; set; }
        public virtual string Citizenship { get; set; }
        public virtual Nullable<int> YearsRecident { get; set; }
        public virtual Nullable<DateTime> IssueDate { get; set; }
        public virtual byte[] Picture { get; set; }
        public virtual byte[] Signature { get; set; }
        public virtual byte[] RightThumb { get; set; }
        public virtual byte[] RightIndex { get; set; }
        public virtual byte[] RightMiddle { get; set; }
        public virtual byte[] RightRing { get; set; }
        public virtual byte[] RightPinky { get; set; }
        public virtual byte[] LeftThumb { get; set; }
        public virtual byte[] LeftIndex { get; set; }
        public virtual byte[] LeftMiddle { get; set; }
        public virtual byte[] LeftRing { get; set; }
        public virtual byte[] LeftPinky { get; set; }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is ApplicantReportViewModel)
            {
                var target = this;
                var source = instance as ApplicantReportViewModel;

                target.Id = source.Id;
                target.ApplicantName = source.ApplicantName;
                target.ApplicantNickname = source.ApplicantNickname;
                target.CityAddress = source.CityAddress;
                target.ProvincialAddress = source.ProvincialAddress;
                target.BirthDate = source.BirthDate;
                target.BirthPlace = source.BirthPlace;
                target.AgeUponApplication = source.AgeUponApplication;
                target.Father = source.Father;
                target.Mother = source.Mother;
                target.Relatives = source.Relatives;
                target.CivilStatus = source.CivilStatus;
                target.Religion = source.Religion;
                target.Occupation = source.Occupation;
                target.Purpose = source.Purpose;
                target.EmailAddress = source.EmailAddress;
                target.TelephoneNumber = source.TelephoneNumber;
                target.CellphoneNumber = source.CellphoneNumber;
                target.TaxCertificateNumber = source.TaxCertificateNumber;
                target.PassportNumber = source.PassportNumber;
                target.TaxIdentificationNumber = source.TaxIdentificationNumber;
                target.SocialSecuritySystemNumber = source.SocialSecuritySystemNumber;
                target.YearsRecident = source.YearsRecident;
                target.Gender = source.Gender;
                target.Build = source.Build;
                target.Marks = source.Marks;
                target.Height = source.Height;
                target.Weight = source.Weight;
                target.Citizenship = source.Citizenship;
                target.IssueDate = source.IssueDate;
                target.Picture = source.Picture;
                target.Signature = source.Signature;
                target.RightThumb = source.RightThumb;
                target.RightIndex = source.RightIndex;
                target.RightMiddle = source.RightMiddle;
                target.RightRing = source.RightRing;
                target.RightPinky = source.RightPinky;
                target.LeftThumb = source.LeftThumb;
                target.LeftIndex = source.LeftIndex;
                target.LeftMiddle = source.LeftMiddle;
                target.LeftRing = source.LeftRing;
                target.LeftPinky = source.LeftPinky;

                return target;
            }
            else if (instance is Clearance)
            {
                var target = this;
                var source = instance as Clearance;

                if (source.ApplicantPicture == null)
                {
                    if (source.Applicant.Pictures.Count() > 0)
                        source.ApplicantPicture = source.Applicant.Pictures.First();
                    else
                        source.ApplicantPicture = new ImageBlob();
                }

                if (source.ApplicantSignature == null)
                {
                    if (source.Applicant.Signatures.Count() > 0)
                        source.ApplicantSignature = source.Applicant.Signatures.First();
                    else
                        source.ApplicantSignature = new ImageBlob();
                }

                if (source.ApplicantCivilStatus == null)
                    source.ApplicantCivilStatus = source.Applicant.CivilStatus;

                if (source.ApplicantAddress == null)
                    source.ApplicantAddress = source.Applicant.Address.ToString();

                if (source.ApplicantCitizenship == null)
                    source.ApplicantCitizenship = source.Applicant.Citizenship;

                target.Id = source.Id;
                target.ApplicantName = source.Applicant.Person.GetDisplayValue();
                target.ApplicantNickname = source.Applicant.AlsoKnownAs;
                target.CityAddress = source.Applicant.Address.GetDisplayValue();
                target.ProvincialAddress = source.Applicant.ProvincialAddress.GetDisplayValue();
                target.BirthDate = source.Applicant.Person.GetBirthDate();
                target.BirthPlace = source.Applicant.BirthPlace;
                target.AgeUponApplication = source.IssueDate.DifferenceInYears(source.Applicant.Person.GetBirthDate());
                target.Father = source.Applicant.Father.GetDisplayValue();
                target.Mother = source.Applicant.Mother.GetDisplayValue();
                target.Relatives = source.Applicant.Relatives.GetDisplayValue();
                target.CivilStatus = source.Applicant.CivilStatus.ToString();
                target.Religion = source.Applicant.Religion;
                target.Occupation = source.Applicant.Occupation;
                target.Purpose = source.Purpose.Name;
                target.EmailAddress = source.Applicant.EmailAddress;
                target.TelephoneNumber = source.Applicant.TelephoneNumber;
                target.CellphoneNumber = source.Applicant.CellphoneNumber;
                target.TaxCertificateNumber = source.TaxCertificateNumber;
                target.PassportNumber = source.Applicant.PassportNumber;
                target.TaxIdentificationNumber = source.Applicant.TaxIdentificationNumber;
                target.SocialSecuritySystemNumber = source.Applicant.SocialSecuritySystemNumber;
                target.YearsRecident = source.YearsResident;
                target.Gender = source.Applicant.Person.Gender.ToString();
                target.Build = source.Applicant.Build;
                target.Marks = source.Applicant.Marks;
                target.Height = source.Applicant.Height;
                target.Weight = source.Applicant.Weight;
                target.Citizenship = source.Applicant.Citizenship;
                target.IssueDate = source.IssueDate;
                target.Picture = source.ApplicantPicture.Bytes;
                target.Signature = source.ApplicantSignature.Bytes;
                target.RightThumb = source.Applicant.FingerPrint.RightThumb.Bytes;
                target.RightIndex = source.Applicant.FingerPrint.RightIndex.Bytes;
                target.RightMiddle = source.Applicant.FingerPrint.RightMiddle.Bytes;
                target.RightRing = source.Applicant.FingerPrint.RightRing.Bytes;
                target.RightPinky = source.Applicant.FingerPrint.RightPinky.Bytes;
                target.LeftThumb = source.Applicant.FingerPrint.LeftThumb.Bytes;
                target.LeftIndex = source.Applicant.FingerPrint.LeftIndex.Bytes;
                target.LeftMiddle = source.Applicant.FingerPrint.LeftMiddle.Bytes;
                target.LeftRing = source.Applicant.FingerPrint.LeftRing.Bytes;
                target.LeftPinky = source.Applicant.FingerPrint.LeftPinky.Bytes;

                return target;
            }

            return null;
        }
    }
}
