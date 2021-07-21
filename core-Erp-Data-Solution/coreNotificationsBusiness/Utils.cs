using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utils
{
    public static string Truncate(this string input, int maxLength)
    {
        return (input.Length > maxLength ? input.Substring(0, maxLength) : input);
    }
    public static string FirstName(this string input)
    {
        var split = input.Trim().Split(' ');
        return split[0];
    }
    public static string AccountNumber(this string input)
    {
        if (input.Length >= 4)
            return input.Substring(0, 2) + "***" + input.Substring(input.Length - 3);
        else
            return "ACC" + "***" + input.Substring(input.Length - 3);
    }

    public static string FirstNameTitleCased(this string input)
    {
        input = input.ToLower();
        TextInfo thisText = new CultureInfo("en-us", false).TextInfo;
        input = thisText.ToTitleCase(input);
        var split = input.Trim().Split(' ');
        return split[0];
    }
}
