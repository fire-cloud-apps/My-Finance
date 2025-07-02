using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;
public class Utils
{
    public static CultureInfo CultureInfo_IN
    {
        get
        {
            return new CultureInfo("en-IN", false)
            {
                NumberFormat = new NumberFormatInfo
                {
                    CurrencySymbol = "₹",
                    CurrencyDecimalDigits = 2,
                    CurrencyGroupSeparator = ",",
                    CurrencyGroupSizes = new[] { 3, 2 },
                    CurrencyNegativePattern = 1, // -1,234.56
                    NumberDecimalDigits = 2,
                    NumberGroupSeparator = ",",
                    NumberGroupSizes = new[] { 3, 2 }
                }
            };
        }
    }
}

