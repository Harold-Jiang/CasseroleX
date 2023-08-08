using System.Text.RegularExpressions;

namespace CasseroleX.Application.Utils;

public static class RegexExtensions
{
   
    public static string matchLatLng(this string latlng)
    {
        return latlng.preg_match(@"/^\d{1,3}\.\d{1,30}$/") ? latlng: "0";
    }


    /// <summary>
    /// Does the regular rule have matching results, ignoring case
    /// </summary>
    /// <param name='rexstr'>regular expression</param>
    /// <param name='input'>Matching items</param>
    /// <returns></returns>
    public static bool preg_match(this string input ,string rexstr)
    {
        Regex regex = new(rexstr, RegexOptions.IgnoreCase);
        return regex.Matches(input).Count > 0;
    }


    /// <summary>
    /// Equivalent to PHP preg_match but only for 3 requied parameters
    /// </summary>
    /// <param name="input"></param>
    /// <param name="regexstr"></param>
    /// <param name="matches"></param>
    /// <returns></returns>
    public static bool preg_match(this string input, string regexstr, out List<string> matches)
    {
        Regex regex = new(regexstr, RegexOptions.IgnoreCase);
        var match = regex.Match(input);
        var groups = (from object g in match.Groups select g.ToString()).ToList();

        matches = groups;
        return match.Success;
    }

    public static bool preg_match_all(this string input, string pattern, out List<string> matches)
    {
        var regex = new Regex(pattern);
        var matchCollection = regex.Matches(input);
        matches = new List<string>();

        foreach (Match match in matchCollection.Cast<Match>())
        {
            matches.Add(match.Value);
        }

        return matchCollection.Count > 0;
    }

    /// <summary>
    /// Equivalent to PHP preg_replace
    /// <see cref="http://stackoverflow.com/questions/166855/c-sharp-preg-replace"/>
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="replacements"></param>
    /// <returns></returns>
    public static string preg_replace(this string input, string[] pattern, string[] replacements)
    {
        if (replacements.Length != pattern.Length)
            throw new ArgumentException("Replacement and Pattern Arrays must be balanced");

        for (var i = 0; i < pattern.Length; i++)
        {
            input = Regex.Replace(input, pattern[i], replacements[i]);
        }

        return input;
    }

    public static bool VersionCompare(string version1, string version2, string op)
    {
        Version v1 = new(version1);
        Version v2 = new(version2);
        int cmp = v1.CompareTo(v2);

        return op switch
        {
            "==" => cmp == 0,
            "!=" => cmp != 0,
            ">" => cmp > 0,
            ">=" => cmp >= 0,
            "<" => cmp < 0,
            "<=" => cmp <= 0,
            _ => throw new ArgumentException($"Invalid operator: {op}"),
        };
    }

    /// <summary>
    /// Remove HTML tags from rich text
    /// </summary>
    public static string ReplaceHtmlTag(this string html, int length = 0)
    {
        string strText = Regex.Replace(html, "<[^>]+>", "");
        strText = Regex.Replace(strText, "&[^;]+;", "");

        if (length > 0 && strText.Length > length)
            return strText[..length];

        return strText;
    }

}
