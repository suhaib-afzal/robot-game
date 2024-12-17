using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Analysis;

public class WithErrMsgCxt<T>
{
    public ErrMsgContext Context { get; set; }

    public T Unwrap { get; set; }
}

public static class WithErrMsgCxtFunctions
{
    public static WithErrMsgCxt<T> WithContext<T>(this T value, ErrMsgContext context)
    {
        return new WithErrMsgCxt<T> { Unwrap = value, Context = context };
    }
} 
