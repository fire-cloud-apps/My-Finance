using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;
public class TextHelper
{
    public static string TruncateString(string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        if (maxLength <= 0)
            return string.Empty;

        if (input.Length <= maxLength)
            return input;

        if (maxLength <= 3)
            return input.Substring(0, maxLength);

        return input.Substring(0, maxLength - 3) + "...";
    }
}

