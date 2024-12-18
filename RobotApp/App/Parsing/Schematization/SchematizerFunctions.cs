using LanguageExt;
using static LanguageExt.Prelude;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using RobotApp.App.DataTypes;
using RobotApp.App.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using RobotApp.App.Parsing.ParsingFailType;

namespace RobotApp.App.Parsing.Schematization;

public static class SchematizerFunctions
{   
    public static Either<ParsingFail, GameSpecification> Schematizer(Doc<TokenLine> doc)
    {
        var djiod = from gridSize in GridSizeDefinition(doc.Chunks[0], new ErrMsgContext())
               from journeyOrObstacles in Or(JourneyDefinition,
                                             ObstaclesDefinition,
                                             doc.Chunks[1],
                                             gridSize.Context)
               from restJournies in ManyChunks(JourneyDefinition,
                                      doc.Chunks.Skip(2).ToList(),
                                      journeyOrObstacles.Context)
               select journeyOrObstacles.Unwrap.Match(
                     Right: journ => new GameSpecification(
                            new GridConstraints(gridSize.Unwrap, new()),
                            restJournies.Unwrap.Prepend(journ).ToList()
                         ),
                     Left: obstacles => new GameSpecification(
                           new GridConstraints(gridSize.Unwrap, obstacles),
                           restJournies.Unwrap
                        )
                   );

        return djiod;
    }

    private static Either<ParsingFail, WithErrMsgCxt<Either<L, R>>> Or<L,R>(
        Func<Chunk<TokenLine>, ErrMsgContext, Either<ParsingFail, WithErrMsgCxt<R>>> tryFirst,
        Func<Chunk<TokenLine>, ErrMsgContext, Either<ParsingFail, WithErrMsgCxt<L>>> trySecond,
        Chunk<TokenLine> chunk,
        ErrMsgContext context)
    {
        (Either<L, R>, ErrMsgContext) mdmcc(WithErrMsgCxt<R> dkj) => (dkj.Unwrap, dkj.Context);
        (Either<L, R>, ErrMsgContext) mdmcc2(WithErrMsgCxt<L> dkj) => (dkj.Unwrap, dkj.Context);

        return tryFirst(chunk, context).Match(
                Left: p => trySecond(chunk, context).Match<Either<ParsingFail, WithErrMsgCxt<Either<L, R>>>>(
                        Left: p2 => p2,
                        Right: l => mdmcc2(l).Item1.WithContext(mdmcc2(l).Item2)
                    ),
                Right: r => mdmcc(r).Item1.WithContext(mdmcc(r).Item2)
            );
    }


    private static Either<ParsingFail, WithErrMsgCxt<(int,int)>> GridSizeDefinition(Chunk<TokenLine> chunk,
                                                                                   ErrMsgContext context)
    {
        return from unit1     in chunk.Lines.ExpectLength(1, context.GridDef(chunk))
               from tokenLine in chunk.Lines[0].ExpectLineTokenTypes(new() {
                                                  TokenType.Word,
                                                  TokenType.NumberxNumber },
                                                  unit1.Context)
               from unit2     in tokenLine.Unwrap.ParseWordAsKeyword(index: 0, "GRID", tokenLine.Context)
               from tuple     in tokenLine.Unwrap.ParseNumberXNumber(index: 1, unit2.Context)
               select tuple;
    }

    private static Either<ParsingFail, WithErrMsgCxt<List<(int, int)>>> ObstaclesDefinition(
                                                                            Chunk<TokenLine> chunk,
                                                                            ErrMsgContext context)
    {
        return InnerObstaclesDefinition(chunk.Lines, context.Obstacles(chunk));
    }

    private static Either<ParsingFail, WithErrMsgCxt<List<(int, int)>>> InnerObstaclesDefinition(
                                                                           List<TokenLine> tokenLines,
                                                                           ErrMsgContext context)
    {
        return tokenLines.Count == 0 ?
                   new List<(int,int)>().WithContext(context) :
                   from tokenLine in tokenLines[0].ExpectLineTokenTypes(new() {
                                                    TokenType.Word,
                                                    TokenType.Number,
                                                    TokenType.Number },
                                                    context)
                   from unit      in tokenLine.Unwrap.ParseWordAsKeyword(index: 0, "OBSTACLE", tokenLine.Context)
                   from fstN      in tokenLine.Unwrap.ParseNumber(index: 1, unit.Context)
                   from sndN      in tokenLine.Unwrap.ParseNumber(index: 2, fstN.Context)
                   from rest      in InnerObstaclesDefinition(tokenLines.Tail().ToList(), sndN.Context)
                   select (fstN.Unwrap, sndN.Unwrap).Cons(rest.Unwrap).ToList().WithContext(rest.Context);
    }


    private static Either<ParsingFail, WithErrMsgCxt<List<T>>> ManyChunks<T>(
        Func<Chunk<TokenLine>, ErrMsgContext,  Either<ParsingFail, WithErrMsgCxt<T>>> func,
        List<Chunk<TokenLine>> list,
        ErrMsgContext context)
    {
        return list.Count == 0 ?
                   new List<T>().WithContext(context) :
                   from t in func(list.Head(), context)
                   from rest in ManyChunks(func, list.Tail().ToList(), t.Context)
                   select t.Unwrap.Cons(rest.Unwrap).ToList().WithContext(rest.Context);
    }

    private static Either<ParsingFail, WithErrMsgCxt<Journey>> JourneyDefinition(Chunk<TokenLine> chunk,
                                                                                ErrMsgContext context)
    {
        return from unit          in chunk.Lines.ExpectLength(3, context.Journey(chunk))
               from startRobotPos in chunk.Lines[0].RobotPositionDefinition(unit.Context)
               from tokenLine     in chunk.Lines[1].ExpectLineTokenTypes(
                                                        new () {TokenType.Word},
                                                        startRobotPos.Context.Instructions(chunk.Lines[1]))
               from instructions  in tokenLine.Unwrap.ParseWordAsLetterSequence(index: 0, tokenLine.Context)
               from goalRobotPos  in chunk.Lines[2].RobotPositionDefinition(instructions.Context)
               select new Journey(goalRobotPos.Unwrap,
                                  startRobotPos.Unwrap,
                                  instructions.Unwrap)
                               .WithContext(goalRobotPos.Context);
    }


    private static Either<ParsingFail, WithErrMsgCxt<RobotPosition>> RobotPositionDefinition(this TokenLine line,
                                                                                            ErrMsgContext context)
    {
        return from posLine in line.ExpectLineTokenTypes(new() {
                                                     TokenType.Number,
                                                     TokenType.Number,
                                                     TokenType.StandaloneLetter },
                                                     context.RobotPosition(line))
               from xCoord in posLine.Unwrap.ParseNumber(index: 0, posLine.Context)
               from yCoord in posLine.Unwrap.ParseNumber(index: 1, xCoord.Context)
               from facing in posLine.Unwrap.ParseStandaloneLetter(index: 2, yCoord.Context)
               select new RobotPosition((xCoord.Unwrap, yCoord.Unwrap),
                                        facing.Unwrap)
                                     .WithContext(facing.Context);
    }


    private static Either<ParsingFail, WithErrMsgCxt<Unit>> ExpectLength<T>(this List<T> list, 
                                                                           int n,
                                                                           ErrMsgContext context)
    {
        return list.Count == n ?
            Unit.Default.WithContext(context) :
            context.DidntFindExpectedLength();
    }

    private static Either<ParsingFail, WithErrMsgCxt<TokenLine>> ExpectLineTokenTypes(
        this TokenLine tokenLine,
        List<TokenType> types,
        ErrMsgContext context)
    {
        var newContext = context.Line(tokenLine);
        return tokenLine.Tokens.Map(tok => tok.TokenType).SequenceEqual(types) ?
            tokenLine.WithContext(newContext) :
            newContext.DidntFindExpectedTokenTypes();
    }

    private static Either<ParsingFail, WithErrMsgCxt<Unit>> ParseWordAsKeyword(
        this TokenLine tokenLine,
        int index,
        string word,
        ErrMsgContext context)
    {
        return tokenLine.Tokens[index].Value == word ?
            Unit.Default.WithContext(context) :
            context.CouldntParseWordAsKeyword(index);
    }

    //Using custom implementation for Sequence, because Language-Ext defintion gives:
    //CS CS1501	No overload for method 'Sequence' takes 1 arguments
    // LanguageExt.Eff<RT, IEnumerable<B>> IEnumerable<char>.Sequence<RT,char,B>(Func<char, Eff<RT,B>> f)
    // where RT: struct
    private static Either<ParsingFail, WithErrMsgCxt<List<Instruction>>> ParseWordAsLetterSequence(
        this TokenLine tokenLine,
        int index,
        ErrMsgContext context)
    {
        return tokenLine.Tokens[index].Value.ToList().Sequence<char, ParsingFail, Instruction>(c => c switch
        {
            'F' => (Instruction)MoveRobotForward,
            'L' => (Instruction)TurnRobotLeft,
            'R' => (Instruction)TurnRobotRight,
            _ => context.CouldntParseWordAsLetterSequence(index)
        }).Map(instruct => instruct.WithContext(context));
    }


    private static Either<L, List<R>>  Sequence<T,L,R>(
       this List<T> list,
       Func<T, Either<L,R>> func)
    {
        return list.Count == 0 ?
            new List<R>() :
            from head   in func(list.Head())
            from rest in list.Tail().Sequence(func)
            select rest.Prepend(head).ToList();
    }



    private static Either<ParsingFail, WithErrMsgCxt<CompassDirection>> ParseStandaloneLetter(
        this TokenLine tokenLine,
        int index,
        ErrMsgContext context)
    {
        return tokenLine.Tokens[index].Value switch
        {
            "N" => CompassDirection.North.WithContext(context),
            "S" => CompassDirection.South.WithContext(context),
            "E" => CompassDirection.East.WithContext(context),
            "W" => CompassDirection.West.WithContext(context),
            _ => context.CouldntParseStandaloneLetter(index),
        };
    }

    private static Either<ParsingFail, WithErrMsgCxt<(int,int)>> ParseNumberXNumber(
        this TokenLine tokenLine,
        int index,
        ErrMsgContext context)
    {
        var split = tokenLine.Tokens[index].Value.Split(new char[] { 'x' });
        var firstParsed = int.TryParse(split[0], out var x);
        var secondParsed = int.TryParse(split[1], out var y);

        return firstParsed && secondParsed ?
            (x, y).WithContext(context) :
            context.CouldntParseNumberXNumber(index);
    }

    private static Either<ParsingFail, WithErrMsgCxt<int>> ParseNumber(
        this TokenLine tokenLine,
        int index,
        ErrMsgContext context)
    {
        return int.TryParse(tokenLine.Tokens[index].Value, out var x) ?
            x.WithContext(context) :
            context.CouldntParseNumber(index);
    }
}
