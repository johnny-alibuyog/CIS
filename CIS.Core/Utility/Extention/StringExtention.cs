﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CIS.Core.Utility.Extention;

public static class StringExtention
{
    private static readonly StringFormatter _formatter = new();

    public static void SetProperCaseSuffix(IEnumerable<string> suffixes)
    {
        _formatter.SetSuffixes(suffixes);
    }

    public static void SetProperCaseSpecialWords(IEnumerable<string> specialWords)
    {
        _formatter.SetSpecialWords(specialWords);
    }

    public static string ToProperCase(this string input)
    {
        return _formatter.ToProperCase(input);
    }

    public static string SplitToWords(this string input)
    {
        return _formatter.SplitToWords(input);
    }

    public static bool IsEqualTo(this string stringA, string stringB)
    {
        var fixedStringA = Regex.Replace(stringA ?? string.Empty, @"\s+", string.Empty);
        var fixedStringB = Regex.Replace(stringB ?? string.Empty, @"\s+", string.Empty);

        return String.Equals(fixedStringA, fixedStringB, StringComparison.OrdinalIgnoreCase);
    }

    //#region Proper Casing

    //private static bool IsAllUpperOrAllLower(this string input)
    //{
    //    return (input.ToLower().Equals(input) || input.ToUpper().Equals(input));
    //}

    //private static string WordToProperCase(string word)
    //{
    //    if (string.IsNullOrEmpty(word)) 
    //        return word;

    //    // Standard case
    //    var value = CapitaliseFirstLetter(word);

    //    // Special cases:
    //    //value = ProperSuffix(value, "'");       // D'Artagnon, D'Silva
    //    value = ProperSuffix(value, ".");       // ???
    //    value = ProperSuffix(value, "-");       // Oscar-Meyer-Weiner
    //    value = ProperSuffix(value, "(");       
    //    value = ProperSuffix(value, ")");       
    //    //value = ProperSuffix(value, "Mc");      // Scots
    //    //value = ProperSuffix(value, "Mac");     // Scots

    //    // Special words:
    //    value = SpecialWords(value, "dela");    // dela Cruz
    //    value = SpecialWords(value, "del");     // del Rosario
    //    value = SpecialWords(value, "de");      // de Guzman
    //    value = SpecialWords(value, "van");     // Dick van Dyke
    //    value = SpecialWords(value, "von");     // Baron von Bruin-Valt
    //    value = SpecialWords(value, "di");
    //    value = SpecialWords(value, "da");      // Leonardo da Vinci, Eduardo da Silva
    //    value = SpecialWords(value, "of");      // The Grand Old Duke of York
    //    value = SpecialWords(value, "the");     // William the Conqueror
    //    value = SpecialWords(value, "HRH");     // His/Her Royal Highness
    //    value = SpecialWords(value, "HRM");     // His/Her Royal Majesty
    //    value = SpecialWords(value, "H.R.H.");  // His/Her Royal Highness
    //    value = SpecialWords(value, "H.R.M.");  // His/Her Royal Majesty

    //    value = HandleRomanNumerals(value);   // William Gates, III

    //    return value;
    //}

    //private static string ProperSuffix(string word, string prefix)
    //{
    //    if (string.IsNullOrEmpty(word)) 
    //        return word;

    //    var lowerWord = word.ToLower();
    //    var lowerPrefix = prefix.ToLower();

    //    if (!lowerWord.Contains(lowerPrefix)) 
    //        return word;

    //    var index = lowerWord.IndexOf(lowerPrefix);

    //    // if the search string is at the end of the word ignore.
    //    if (index + prefix.Length == word.Length) 
    //        return word;

    //    return word.Substring(0, index) + prefix + CapitaliseFirstLetter(word.Substring(index + prefix.Length));
    //}

    //private static string SpecialWords(string word, string specialWord)
    //{
    //    if (word.Equals(specialWord, StringComparison.InvariantCultureIgnoreCase))
    //        return specialWord;
    //    else
    //        return word;
    //}

    //private static string HandleRomanNumerals(string word)
    //{
    //    var ones = new List<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
    //    var tens = new List<string>() { "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC", "C" };

    //    // assume nobody uses hundreds
    //    foreach (string number in ones)
    //    {
    //        if (word.Equals(number, StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            return number;
    //        }
    //    }

    //    foreach (string ten in tens)
    //    {
    //        foreach (string one in ones)
    //        {
    //            if (word.Equals(ten + one, StringComparison.InvariantCultureIgnoreCase))
    //            {
    //                return ten + one;
    //            }
    //        }
    //    }

    //    return word;
    //}

    //private static string CapitaliseFirstLetter(string word)
    //{
    //    return char.ToUpper(word[0]) + word.Substring(1).ToLower();
    //}

    //public static string ToProperCase(this string input)
    //{
    //    if (input == null)
    //        return null;

    //    if (string.IsNullOrWhiteSpace(input))
    //        return string.Empty;

    //    var words = input.Split(' ')
    //        .Where(word => !string.IsNullOrWhiteSpace(word))
    //        .Select(word => WordToProperCase(word));

    //    return string.Join(" ", words);

    //    //if (IsAllUpperOrAllLower(input))
    //    //{
    //    //    // fix the ALL UPPERCASE or all lowercase names
    //    //    return string.Join(" ", input.Split(' ').Select(word => WordToProperCase(word)));
    //    //}
    //    //else
    //    //{
    //    //    // leave the CamelCase or Propercase names alone
    //    //    return input;
    //    //}

    //    /* 
    //     * original implementation
    //     */
    //    //    if (input == null)
    //    //        return null;

    //    //    var characters = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower()).ToCharArray();

    //    //    for (int i = 0; i + 1 < characters.Length; i++)
    //    //    {
    //    //        if ((characters[i].Equals('\'')) || (characters[i].Equals('-')))
    //    //        {
    //    //            characters[i + 1] = Char.ToUpper(characters[i + 1]);
    //    //        }
    //    //    }
    //    //    return new string(characters);
    //}

    //#endregion
}