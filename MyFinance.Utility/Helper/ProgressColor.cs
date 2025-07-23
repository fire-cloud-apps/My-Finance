using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;
/// <summary>
/// Provides a method to determine the color based on investment progress.
/// </summary>
public class ProgressColor
{
    /// <summary>
    /// Proides a color based on the investment amount and target amount.
    /// </summary>
    /// <param name="investmentAmt">invested amount</param>
    /// <param name="targetAmt">destination amount</param>
    /// <returns> returns color code </returns>
    public static Color GetProgressColor(decimal investmentAmt, decimal targetAmt)
    {
        if (targetAmt == 0) return Color.Default; // Avoid division by zero

        var value = (double)(investmentAmt / targetAmt * 100M);

        if (value >= 0 && value <= 10)
        {
            return Color.Error;
        }
        else if (value >= 11 && value <= 15)
        {
            return Color.Secondary;
        }
        else if (value >= 16 && value <= 30)
        {
            return Color.Warning;
        }
        else if (value >= 31 && value <= 55)
        {
            return Color.Primary;
        }
        else if (value >= 56 && value <= 75)
        {
            return Color.Info;
        }
        else if (value >= 76 && value <= 95)
        {
            return Color.Tertiary;
        }
        else if (value > 95) // Covers 95 and above
        {
            return Color.Success;
        }
        else
        {
            // Handle any edge cases where value might be negative or outside defined ranges,
            // although with investmentAmt and targetAmt typically positive, this might not be strictly necessary
            return Color.Default;
        }
    }
}

