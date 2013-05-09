using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Utilities.Extentions
{
    public static class StringExtention
    {
        public static string ToProperCase(this string input)
        {
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

        public static bool EqualsIgnoreCase(this string stringA, string stringB)
        {
            return (string.Compare(stringA, stringB, true) == 0);
        }
    }
}
