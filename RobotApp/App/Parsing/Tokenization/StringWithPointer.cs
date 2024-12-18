using LanguageExt;

namespace RobotApp.App.Parsing.Tokenization;

internal class StringWithPointer
{
    public StringWithPointer(string value, int pointer)
    {
        Value = value;
        Pointer = pointer;
    }

    public string Value { get; }

    public int Pointer { get; }
}

internal static class StringWithPointerFunctions
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
