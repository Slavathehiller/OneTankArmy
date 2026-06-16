using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IntExtensions
{
    public static string To4SymbString(this int value)
    {
        var result = value.ToString();
        if (result.Length > 12)
            result = "LOT";
        if (result.Length > 9)
            result = result.Substring(0, result.Length - 9) + " B";
        else
        if (result.Length > 6)
            result = result.Substring(0, result.Length - 6) + " M";
        else
        if (result.Length > 3)
            result = result.Substring(0, result.Length - 3) + " K";

        return result;
    }
}

