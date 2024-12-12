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
    public StringWithPointer(string value, int pointer)
    {
        Value = value;
        Pointer = pointer;
    }

    public string Value { get; set; }

    public int Pointer { get; set; }
}

public static class StringWithPointerFunctions
{
    public static StringWithPointer IncrementBy(this StringWithPointer strP, int n)
    {
        return new StringWithPointer(strP.Value, strP.Pointer + n);
    }

    public static Option<string> GetRestOfString(StringWithPointer strP)
    {
        if (strP.Pointer > strP.Value.Length - 1)
        {
            return Option<string>.None;
        }

        return strP.Value[strP.Pointer..];
    }

}
