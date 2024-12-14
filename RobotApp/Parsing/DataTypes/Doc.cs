using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RobotApp.Parsing.DataTypes;

public class Doc<T>
{
    public Doc(List<Chunk<T>> chunks)
    {
        Chunks = chunks;
    }

    public List<Chunk<T>> Chunks { get; set; }

}

public class Chunk<T>
{
    public Chunk(List<T> values)
    {
        Values = values;
    }


    public List<T> Values;
}

public static class DocAndChunkFunctions
{

    //TODO: Make these Traverse implementations truly Generic with a wrapping monadic type
    //      variable M instead of concrete type Either
    public static Either<L, Doc<B>> Sequence<L,A,B>(this Doc<A> doc, Func<A, Either<L,B>> func)
    {
        return doc.Chunks
                  .Sequence(chunk => chunk.Sequence(func))
                  .Map(chunks => new Doc<B>(chunks.ToList()));
    }

    public static Either<L, Chunk<B>> Sequence<L,A,B>(this Chunk<A> chunk, Func<A, Either<L, B>> func)
    {
        return SequenceHelper(chunk.Values, new List<B>(), func).Map(list => new Chunk<B>(list));
    }

    public static Either<L, List<B>> SequenceHelper<L, A, B>(List<A> list, List<B> acc, Func<A, Either<L, B>> func)
    {
        if (list.Count == 0)
        {
            return acc;
        }

        return from first in list.First().Apply(func)
               from rest in SequenceHelper(list.Skip(1).ToList(), acc.Append(first).ToList(), func)
               select rest;
    }

    /*
    public static Either<L, T> Expect<L, T>(this Doc<TokenLine> doc, int lineNum, Either<L,Chunk<TokenLine>> expectation)
    {

    }
    */


}
