using System;
using System.Data;
using System.Data.SqlTypes;
using CIS.Core.Utility.Extention;

namespace CIS.UI.Features.Membership.Warrants.Imports
{
    /*
    use [cisdb];
    go

    select top 10000
        w.WarrantCode,
        w.CaseNumber,
        w.Crime,
        w.Description,
        w.Remarks,
        w.BailAmount,
        w.IssuedOn,
        w.IssuedBy,
        w.IssuedAtAddress1,
        w.IssuedAtAddress2,
        w.IssuedAtBarangay,
        w.IssuedAtCity,
        w.IssuedAtProvince,
        s.FirstName,
        s.MiddleName,
        s.LastName,
        s.Suffix,
        s.Gender,
        s.BirthDate,
        s.Address1,
        s.Address2,
        s.Barangay,
        s.City,
        s.Province,
        s.Hair,
        s.Eyes,
        s.Complexion,
        s.Build,
        s.ScarsAndMarks,
        s.Race,
        s.Nationality
    from 
        Membership.Warrants as w
    inner join
        Membership.Suspects as s
            on w.WarrantId = s.WarrantId
    where
        w.CaseNumber <> '' and
        w.CaseNumber is not null
*/

    public class NaraImportSuspect
    {
        private string _caseNumber;
        private string _crime;
        private string _prefix;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _suffix;
        private string _gender;
        private DateTime? _birthDate;
        private string _address1;
        private string _address2;
        private string _barangay;
        private string _city;
        private string _province;
        private string _hair;
        private string _eyes;
        private string _complexion;
        private string _build;
        private string _scarsAndMarks;
        private string _race;
        private string _nationality;

        public virtual string CaseNumber
        {
            get { return _caseNumber; }
            set { _caseNumber = (value ?? string.Empty).Trim().ToProperCase(); }
        }

        public virtual string Crime
        {
            get { return _crime; }
            set { _crime = (value ?? string.Empty).Trim().ToProperCase(); }
        }

        public virtual string Prefix
        {
            get { return _prefix; }
            set { _prefix = (value ?? string.Empty).Trim(); }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = (value ?? string.Empty).Trim(); }
        }

        public virtual string MiddleName
        {
            get { return _middleName; }
            set { _middleName = (value ?? string.Empty).Trim(); }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = (value ?? string.Empty).Trim(); }
        }

        public virtual string Suffix
        {
            get { return _suffix; }
            set { _suffix = (value ?? string.Empty).Trim(); }
        }

        public virtual string Gender
        {
            get { return _gender; }
            set { _gender = (value ?? string.Empty).Trim(); }
        }

        public virtual DateTime? BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value <= SqlDateTime.MinValue.Value ? null : value; }
        }

        public virtual string Address1
        {
            get { return _address1; }
            set { _address1 = (value ?? string.Empty).Trim(); }
        }

        public virtual string Address2
        {
            get { return _address2; }
            set { _address2 = (value ?? string.Empty).Trim(); }
        }

        public virtual string Barangay
        {
            get { return _barangay; }
            set { _barangay = (value ?? string.Empty).Trim(); }
        }

        public virtual string City
        {
            get { return _city; }
            set { _city = (value ?? string.Empty).Trim(); }
        }

        public virtual string Province
        {
            get { return _province; }
            set { _province = (value ?? string.Empty).Trim(); }
        }

        public virtual string Hair
        {
            get { return _hair; }
            set { _hair = (value ?? string.Empty).Trim(); }
        }

        public virtual string Eyes
        {
            get { return _eyes; }
            set { _eyes = (value ?? string.Empty).Trim(); }
        }

        public virtual string Complexion
        {
            get { return _complexion; }
            set { _complexion = (value ?? string.Empty).Trim(); }
        }

        public virtual string Build
        {
            get { return _build; }
            set { _build = (value ?? string.Empty).Trim(); }
        }

        public virtual string ScarsAndMarks
        {
            get { return _scarsAndMarks; }
            set { _scarsAndMarks = (value ?? string.Empty).Trim(); }
        }

        public virtual string Race
        {
            get { return _race; }
            set { _race = (value ?? string.Empty).Trim(); }
        }

        public virtual string Nationality
        {
            get { return _nationality; }
            set { _nationality = (value ?? string.Empty).Trim(); }
        }

        public static NaraImportSuspect OfDataRow(DataRow row)
        {
            return new()
            {
                CaseNumber = row.Field<string>("CaseNumber"),
                Crime = row.Field<string>("Crime"),
                Prefix = row.Field<string>("Prefix"),
                FirstName = row.Field<string>("FirstName"),
                MiddleName = row.Field<string>("MiddleName"),
                LastName = row.Field<string>("LastName"),
                Suffix = row.Field<string>("Suffix"),
                Gender = row.Field<string>("Gender"),
                BirthDate = row.Field<string>("BirthDate").ParseDate(),
                Address1 = row.Field<string>("Address1"),
                Address2 = row.Field<string>("Address2"),
                Barangay = row.Field<string>("Barangay"),
                City = row.Field<string>("City"),
                Province = row.Field<string>("Province"),
                Hair = row.Field<string>("Hair"),
                Eyes = row.Field<string>("Eyes"),
                Complexion = row.Field<string>("Complexion"),
                Build = row.Field<string>("Build"),
                ScarsAndMarks = row.Field<string>("ScarsAndMarks"),
                Race = row.Field<string>("Race"),
                Nationality = row.Field<string>("Nationality")
            };
        }

        public static bool IsValid(NaraImportSuspect suspect)
        {
            return 
                !string.IsNullOrWhiteSpace(suspect.FirstName) &&
                !string.IsNullOrWhiteSpace(suspect.LastName);
        }
    }
}
