using LanguageExt;
using static LanguageExt.Prelude;
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

        public static Func<T, Either<L,R>> AltChain<T, L, R>(List<Func<T, Either<L, R>>> funcs)
        {
            return funcs.Aggregate(Alt);
        }

        public static Func<T, Either<L, R>> Alt<T, L, R>(Func<T, Either<L, R>> lhs, Func<T, Either<L, R>> rhs)
        {
            return t=>
                lhs(t).Match(
                    Right: r => Right(r),
                    Left: l => rhs(t)
                );
        }
        /*
        public static Func<T1, T2, Either<L, R>> Alt<T1, T2, L, R>(Func<T1, T2, Either<L, R>> lhs, Func<T1, T2, Either<L, R>> rhs)
        {
            return (t1,t2) =>
                lhs(t1, t2).Match(
                    Right: r => Right(r),
                    Left: l => rhs(t1, t2)
                );
        }
        */
    }
}

