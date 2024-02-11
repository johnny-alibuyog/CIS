using System;
using System.Data;
using System.Linq;
using CIS.Core.Utility.Extention;

namespace CIS.UI.Features.Membership.Warrants.Imports
{
    public class CodelessWarrant
    {
        private string _prefix;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _suffix;
        private string _address;
        private string _case;
        private string _disposition;
        private DateTime? _arrestedOn;

        public virtual string Prefix
        {
            get { return _prefix; }
            set { _prefix = value.ToProperCase(); }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value.ToProperCase(); }
        }

        public virtual string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value.ToProperCase(); }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value.ToProperCase(); }
        }
        
        public virtual string Suffix
        {
            get { return _suffix; }
            set { _suffix = value.ToProperCase(); }
        }

        public virtual string Address
        {
            get { return _address; }
            set { _address = value.ToProperCase(); }
        }

        public virtual string Case
        {
            get { return _case; }
            set { _case = value.ToProperCase(); }
        }

        public virtual string Disposition
        {
            get { return _disposition; }
            set { _disposition = value.ToProperCase(); }
        }

        public virtual DateTime? ArrestedOn
        {
            get { return _arrestedOn; }
            set { _arrestedOn = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                this.Prefix ?? string.Empty,
                this.FirstName ?? string.Empty,
                this.MiddleName ?? string.Empty,
                this.LastName ?? string.Empty,
                this.Suffix ?? string.Empty
            )
            .ToProperCase();
        }

        public static CodelessWarrant Fix(CodelessWarrant item)
        {
            if (item.LastName != null && item.LastName.Contains(','))
            {
                var names = item.LastName.Split(',')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();

                item.LastName = names.FirstOrDefault();

                if (names.Count() > 1 && string.IsNullOrWhiteSpace(item.FirstName))
                    item.FirstName = names.LastOrDefault();
            }

            if (item.FirstName != null && item.FirstName.Contains(' ') &&
                (
                    item.FirstName.EndsWith("jr", StringComparison.InvariantCultureIgnoreCase) ||
                    item.FirstName.EndsWith("jr.", StringComparison.InvariantCultureIgnoreCase) ||
                    item.FirstName.EndsWith("sr", StringComparison.InvariantCultureIgnoreCase) ||
                    item.FirstName.EndsWith("sr.", StringComparison.InvariantCultureIgnoreCase) ||
                    item.FirstName.EndsWith("ii", StringComparison.InvariantCultureIgnoreCase) ||
                    item.FirstName.EndsWith("iii", StringComparison.InvariantCultureIgnoreCase) ||
                    item.FirstName.EndsWith("iv", StringComparison.InvariantCultureIgnoreCase) ||
                    item.FirstName.EndsWith("v", StringComparison.InvariantCultureIgnoreCase)
                ))
            {
                item.Suffix = item.FirstName.Split(' ').Last();
                item.FirstName = item.FirstName.Replace(item.Suffix, string.Empty);
            }

            if (item.FirstName != null && item.FirstName.Contains(','))
            {
                item.FirstName = item.FirstName.Replace(",", string.Empty);
            }

            return item;
        }

        public static bool IsValid(CodelessWarrant warrant)
        {
            return
                !string.IsNullOrWhiteSpace(warrant.FirstName) &&
                !string.IsNullOrWhiteSpace(warrant.LastName);
        }

        public static CodelessWarrant OfDataRow(DataRow row)
        {
            return new CodelessWarrant()
            {
                FirstName = row.Field<string>("GIVEN NAME"),
                MiddleName = row.Field<string>("MIDDLE NAME"),
                LastName = row.Field<string>("SURNAME"),
                Address = row.Field<string>("ADDRESS"),
                Case = row.Field<string>("CASE"),
                Disposition = row.Field<string>("DISPOSITION"),
                ArrestedOn = row.Field<string>("DATE ARRESTED").ParseDate()
            };
        }
    }
}
