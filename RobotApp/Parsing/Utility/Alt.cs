using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Utility
{
    public static class AltFunction
    {
        public static Func<A, Option<B>> Alt<A, B>(Func<A, Option<B>> lhs, Func<A, Option<B>> rhs)
        {
            return a =>
                lhs(a).Match(
                    Some: b => b,
                    None: rhs(a)
                );
        }
    }
}
