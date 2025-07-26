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


public class SvgConverter
{
    public static string GenerateSvgDataUrl(string svgContent)
    {
        // Simple URL encoding (might need more robust for complex SVGs)
        // It's often better to Base64 encode the SVG directly for more reliability
        // For this example, let's just show the concept of a "data:image/svg+xml" URL
        #region Broken Image
        string brokenImage = "<svg version='1.1' id='Capa_1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 52.965 52.965' xml:space='preserve' fill='#000000'><g id='SVGRepo_bgCarrier' stroke-width='0'></g><g id='SVGRepo_tracerCarrier' stroke-linecap='round' stroke-linejoin='round'></g><g id='SVGRepo_iconCarrier'> <path style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' d='M24.008,13.401L33.2,4.208 c4.278-4.278,11.278-4.278,15.556,0l0,0c4.278,4.278,4.278,11.278,0,15.556L36.735,31.786c-4.278,4.278-11.278,4.278-15.556,0l0,0'></path> <path style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' d='M27.543,40.978l-7.778,7.778 c-4.278,4.278-11.278,4.278-15.556,0l0,0c-4.278-4.278-4.278-11.278,0-15.556l11.314-11.314c4.278-4.278,11.278-4.278,15.556,0l0,0'></path> <line style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' x1='33.965' y1='45' x2='33.965' y2='51'></line> <line style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' x1='37.722' y1='43' x2='41.965' y2='47.243'></line> <line style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' x1='38.965' y1='39' x2='44.965' y2='39'></line> <line style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' x1='15.965' y1='10' x2='15.965' y2='4'></line> <line style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' x1='12.207' y1='12' x2='7.965' y2='7.757'></line> <line style='fill:none;stroke:#D5354E;stroke-width:2;stroke-linecap:round;stroke-miterlimit:10;' x1='10.965' y1='16' x2='4.965' y2='16'></line> </g></svg>";
        #endregion

        // Basic SVG content for demonstration
        /* string baseSvg = $"<svg xmlns='http://www.w3.org/2000/svg' width='200' height='50'>" +
                         $"<text x='10' y='30' font-family='Verdana' font-size='20' fill='blue'>" +
                         $"{svgContent}" + // Placeholder for your dynamic text
                         $"</text></svg>";
        */
        string baseSvg = svgContent;
        if (string.IsNullOrEmpty(baseSvg))
        {
            baseSvg = brokenImage;
        }

        // Base64 encode the SVG content
        byte[] svgBytes = Encoding.UTF8.GetBytes(baseSvg);
        string base64Svg = Convert.ToBase64String(svgBytes);

        return $"data:image/svg+xml;base64,{base64Svg}";
    }
}