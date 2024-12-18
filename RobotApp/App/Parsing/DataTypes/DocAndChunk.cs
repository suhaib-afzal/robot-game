using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RobotApp.App.Parsing.DataTypes;

public class Doc<T>
{
    public Doc(List<Chunk<T>> chunks)
    {
        Chunks = chunks;
    }

    public List<Chunk<T>> Chunks { get; }

}

public class Chunk<T>
{
    public Chunk(List<T> lines)
    {
        Lines = lines;
    }


    public List<T> Lines;
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

    private static Either<L, Chunk<B>> Sequence<L,A,B>(this Chunk<A> chunk, Func<A, Either<L, B>> func)
    {
        return SequenceHelper(chunk.Lines, new List<B>(), func).Map(list => new Chunk<B>(list));
    }

    private static Either<L, List<B>> SequenceHelper<L, A, B>(List<A> list, List<B> acc, Func<A, Either<L, B>> func)
    {
        if (list.Count == 0)
        {
            return acc;
        }

        return from first in list.First().Apply(func)
               from rest in SequenceHelper(list.Skip(1).ToList(), acc.Append(first).ToList(), func)
               select rest;
    }

    public static List<TextLine> ToTextLines(this Chunk<TokenLine> chunk)
    {
        return chunk.Lines.Map(l => l.TextLine).ToList();
    }

}
