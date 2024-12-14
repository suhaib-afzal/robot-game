using LanguageExt;
using static LanguageExt.Prelude;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using RobotApp.App.DataTypes;
using RobotApp.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RobotApp.Parsing.Analysis;

public static class SchematizerFunctions
{

    public static Either<SchematizerFail, GameSpecification> StandardSchema(Doc<TokenLine> doc)
    {
        return from gridSize in GridSizeDefinition(doc.Chunks[0])
               from journies in Many(JourneyDefinition, doc.Chunks.Tail().ToList())
               select new GameSpecification(new GridConstraints(gridSize, new()), journies);
    }

    public static Either<SchematizerFail, GameSpecification> ObstaclesSchema(Doc<TokenLine> doc)
    {
        return from gridSize  in GridSizeDefinition(doc.Chunks[0])
               from obstacles in ObstaclesDefinition(doc.Chunks[1])
               from journies  in Many(JourneyDefinition, doc.Chunks.Skip(2).ToList())
               select new GameSpecification(new GridConstraints(gridSize, obstacles), journies);
    }

    public static Either<SchematizerFail, (int,int)> GridSizeDefinition(Chunk<TokenLine> chunk)
    {
        return from _         in chunk.Values.ExpectLength(1)
               from tokenLine in chunk.Values[0].ExpectLineTokenTypes(new() {
                                                  TokenType.Word,
                                                  TokenType.NumberxNumber })
               from __        in tokenLine.Tokens[0].ParseWordAsKeyword("GRID")
               from tuple     in tokenLine.Tokens[1].ParseNumberXNumber()
               select tuple;
    }

    public static Either<SchematizerFail, List<(int, int)>> ObstaclesDefinition(Chunk<TokenLine> chunk)
    {
        return InnerObstaclesDefinition(chunk.Values);
    }

    public static Either<SchematizerFail, List<(int, int)>> InnerObstaclesDefinition(List<TokenLine> tokenLines)
    {
        return tokenLines.Count == 0 ?
                   new() :
                   from tokenLine in tokenLines[0].ExpectLineTokenTypes(new() {
                                                    TokenType.Word,
                                                    TokenType.Number,
                                                    TokenType.Number })
                   from _         in tokenLine.Tokens[0].ParseWordAsKeyword("OBSTACLE")
                   from fstN      in tokenLine.Tokens[1].ParseNumber()
                   from sndN      in tokenLine.Tokens[2].ParseNumber()
                   from rest      in InnerObstaclesDefinition(tokenLines.Tail().ToList())
                   select (fstN, sndN).Cons(rest).ToList();
    }

    public static Either<SchematizerFail, List<T>> Many<T>(Func<Chunk<TokenLine>, Either<SchematizerFail, T>> func, List<Chunk<TokenLine>> list)
    {
        return list.Count == 0 ?
                   new() :
                   from t in func(list.First())
                   from rest in Many(func, list.Tail().ToList())
                   select t.Cons(rest).ToList();
    }

    public static Either<SchematizerFail, Journey> JourneyDefinition(Chunk<TokenLine> chunk)
    {
        return from _             in chunk.Values.ExpectLength(3)
               from startRobotPos in RobotPositionDefinition(chunk.Values[0])
               from tokenLine     in chunk.Values[1].ExpectLineTokenTypes(new () { 
                                                         TokenType.Word })
               from instructions  in tokenLine.Tokens[0].ParseWordAsLetterSequence()
               from goalRobotPos  in RobotPositionDefinition(chunk.Values[2])
               select new Journey(goalRobotPos, startRobotPos, instructions);
    }

    public static Either<SchematizerFail, RobotPosition> RobotPositionDefinition(TokenLine line)
    {
        return from startPosLine in line.ExpectLineTokenTypes(new() {
                                                     TokenType.Number,
                                                     TokenType.Number,
                                                     TokenType.StandaloneLetter })
               from xCoord       in startPosLine.Tokens[0].ParseNumber()
               from yCoord       in startPosLine.Tokens[1].ParseNumber()
               from facingDirect in startPosLine.Tokens[2].ParseStandaloneLetter()
               select new RobotPosition((xCoord, yCoord), facingDirect);
    }


    public static Either<SchematizerFail, Unit> ExpectLength<T>(this List<T> list, int n)
    {
        return list.Count == n ?
            Unit.Default :
            new SchematizerFail();
    }

    public static Either<SchematizerFail, TokenLine> ExpectLineTokenTypes(this TokenLine tokenLine, List<TokenType> types)
    {
        return tokenLine.Tokens.Map(tok => tok.TokenType).SequenceEqual(types) ?
            tokenLine :
            new SchematizerFail();
    }

    public static Either<SchematizerFail, Unit> ParseWordAsKeyword(this Token token, string word)
    {
        return token.Value == word ?
            Unit.Default :
            new SchematizerFail();
    }

    //Using custom implementation for Sequence, because Language-Ext defintion gives:
    //CS CS1501	No overload for method 'Sequence' takes 1 arguments
    // LanguageExt.Eff<RT, IEnumerable<B>> IEnumerable<char>.Sequence<RT,char,B>(Func<char, Eff<RT,B>> f)
    // where RT: struct
    public static Either<SchematizerFail, List<Instruction>> ParseWordAsLetterSequence(this Token token)
    {
        return token.Value.ToList().Sequence<char, SchematizerFail, Instruction>(c => c switch
        {
            'F' => (Instruction)moveRobotForward,
            'L' => (Instruction)turnRobotLeft,
            'R' => (Instruction)turnRobotRight,
            _ => new SchematizerFail()
        });
    }

    
    public static Either<L, List<R>>  Sequence<T,L,R>(this List<T> list, Func<T, Either<L,R>> func)
    {
        return list.Count == 0 ?
            new List<R>() :
            from kmd   in func(list.Head())
            from ejije in list.Tail().Sequence(func)
            select ejije.Prepend(kmd).ToList();
    }
    
    

    public static Either<SchematizerFail, CompassDirection> ParseStandaloneLetter(this Token token)
    {
        return token.Value switch
        {
            "N" => CompassDirection.North,
            "S" => CompassDirection.South,
            "E" => CompassDirection.East,
            "W" => CompassDirection.West,
            _ => new SchematizerFail(),
        };
    }

    public static Either<SchematizerFail, (int,int)> ParseNumberXNumber(this Token token)
    {
        var split = token.Value.Split(new char[] { 'x' });
        var firstParsed = int.TryParse(split[0], out var x);
        var secondParsed = int.TryParse(split[1], out var y);

        return firstParsed && secondParsed ?
            (x, y) :
            new SchematizerFail();
    }

    public static Either<SchematizerFail, int> ParseNumber(this Token token)
    {
        return int.TryParse(token.Value, out var x) ?
            x :
            new SchematizerFail();
    }
}
