using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Utility
{
    public static class AltFunctions
    {
        //TODO: Make these Alt implementations truly Generic with a wrapping monadic type
        //      variable M instead of concrete type Option
        public static Func<A, Option<B>> AltChain<A, B>(List<Func<A, Option<B>>> funcs)
        {
            return funcs.Aggregate(Alt);
        }

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
