using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Utility;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, Func<T, bool> spliter)
    {
        using (var enumerator = list.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                yield return SplitHelper(enumerator, spliter);
            }
        }
    }

    private static IEnumerable<T> SplitHelper<T>(IEnumerator<T> enumerator, Func<T, bool> spliter)
    {
        do
        {
            if (!spliter(enumerator.Current))
            {
                yield return enumerator.Current;
            }
        } while (!spliter(enumerator.Current) && enumerator.MoveNext());

    }

    public static bool isAlphabet(this char c) => Regex.IsMatch(c.ToString(), "[a-z]", RegexOptions.IgnoreCase);

    public static bool isNumeric(this char c) => Regex.IsMatch(c.ToString(), "[0-9]");
}
