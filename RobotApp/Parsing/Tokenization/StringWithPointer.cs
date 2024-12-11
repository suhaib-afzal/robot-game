using LanguageExt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Tokenization;

public class StringWithPointer
{
    public string Value { get; set; }

    public int Pointer { get; set; }
}

public static class StringWithPointerFunctions
{
    public static bool IsAtEnd(StringWithPointer strP)
    {
        return (strP.Value.Length - 1 == strP.Pointer);
    }

    public static Option<(char,StringWithPointer)> GetCharAndIncrement(StringWithPointer strP)
    {
        throw new NotImplementedException();
    }

}
