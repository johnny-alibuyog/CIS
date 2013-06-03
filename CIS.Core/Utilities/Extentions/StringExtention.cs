using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CIS.Core.Utilities.Extentions
{
    public static class StringExtention
    {
        public static string ToProperCase(this string input)
        {
            if (input == null)
                return null;

            char[] chars = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower()).ToCharArray();

            for (int i = 0; i + 1 < chars.Length; i++)
            {
                if ((chars[i].Equals('\'')) ||
                    (chars[i].Equals('-')))
                {
                    chars[i + 1] = Char.ToUpper(chars[i + 1]);
                }
            }
            return new string(chars);
        }

        public static bool IsEqualTo(this string stringA, string stringB)
        {
            var fixedStringA = Regex.Replace(stringA ?? string.Empty, @"\s+", string.Empty);
            var fixedStringB = Regex.Replace(stringB ?? string.Empty, @"\s+", string.Empty);

            return String.Equals(fixedStringA, fixedStringB, StringComparison.OrdinalIgnoreCase);
        }
    }
}
