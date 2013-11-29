using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Utilities.Extentions
{
    public static class EntityExtention
    {
        public static string GetDisplayValue(this Person entity)
        {
            return entity != null ? entity.Fullname : string.Empty;
        }

        public static Nullable<DateTime> GetBirthDate(this Person entity)
        {
            return entity != null && entity.BirthDate != null ? entity.BirthDate : null;
        }

        public static string GetDisplayValue(this IEnumerable<Person> values)
        {
            return string.Join(", ", values.Select(x => x.GetDisplayValue()));
        }

        public static string GetDisplayValue(this PersonBasic entity)
        {
            return entity != null ? entity.Fullname : string.Empty;
        }

        public static string GetDisplayValue(this IEnumerable<PersonBasic> values)
        {
            return string.Join(", ", values.Select(x => x.GetDisplayValue()));
        }

        public static Nullable<int> DifferenceInYears(this DateTime dateTime1, Nullable<DateTime> dateTime2)
        {
            return DifferenceInYears(new Nullable<DateTime>(dateTime1), dateTime2);
        }

        public static Nullable<int> DifferenceInYears(this Nullable<DateTime> dateTime1, DateTime dateTime2)
        {
            return DifferenceInYears(dateTime1, new Nullable<DateTime>(dateTime2));
        }

        public static Nullable<int> DifferenceInYears(this Nullable<DateTime> dateTime1, Nullable<DateTime> dateTime2)
        {
            if (dateTime1 == null)
                return null;

            if (dateTime2 == null)
                return null;

            var span = dateTime1.Value - dateTime2.Value;
            return Convert.ToInt32(span.TotalDays / 365D);
        }


        public static string GetDisplayValue(this Address entity)
        {
            return entity != null ? entity.ToString() : string.Empty;
        }
    }
}
