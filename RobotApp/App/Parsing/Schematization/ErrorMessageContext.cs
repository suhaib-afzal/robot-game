using LanguageExt;
using RobotApp.App.Parsing.DataTypes;
using RobotApp.App.Parsing.ParsingFailType;
using System.Collections.Generic;

namespace RobotApp.App.Parsing.Schematization;

internal class ErrMsgContext
{
    public ErrMsgContext(
        int sectionNumber,
        ExpectationType expectationType,
        AssertionLevel assertionLevel,
        TokenLine tokenLine)
    {
        SectionNumber = sectionNumber;
        ExpectationType = expectationType;
        AssertionLevel = assertionLevel;
        TokenLine = tokenLine;
    }

    public ErrMsgContext(
        int sectionNumber,
        ExpectationType expectationType,
        AssertionLevel assertionLevel,
        Chunk<TokenLine> chunk)
    {
        SectionNumber = sectionNumber;
        ExpectationType = expectationType;
        AssertionLevel = assertionLevel;
        Chunk = chunk;
    }

    public ErrMsgContext()
    {
        SectionNumber = 0;
        ExpectationType = ExpectationType.None;
        AssertionLevel = AssertionLevel.None;
    }

    public int SectionNumber { get; }

    public ExpectationType ExpectationType { get; }

    public AssertionLevel AssertionLevel { get; }

    private TokenLine _tokenLine;

    private Chunk<TokenLine> _chunk;

    public TokenLine TokenLine
    {
        get 
        {
            if (_tokenLine == null)
                throw new UnexpectedContextException();
            return _tokenLine;
        }
        set
        {
            _tokenLine = value;
        }
    }

    public Chunk<TokenLine> Chunk
    {
        get
        {
            if (_chunk == null)
                throw new UnexpectedContextException();
            return _chunk;
        }
        set
        {
            _chunk = value;
        }
    }

}

public enum ExpectationType
{
    None,
    GridDefinition,
    Obstacles,
    Journey,
    RobotPosition,
    Instructions,
}

public enum AssertionLevel
{
    None,
    Chunk,
    Line,
}


internal static class ErrMsgContextFunctions
{

    public static ParsingFail CouldntParseNumber(this ErrMsgContext context, int tokenIndex)
    {
        return context.AssertionLevel switch
        {
            AssertionLevel.Line => context.ExpectationType switch
            {
                ExpectationType.RobotPosition => new ParsingFail(
                    "Could not parse as a number, expecting number in this position in an Robot Position defintion",
                    RobotPositionContextualInfo,
                    context.TokenLine.TextLine,
                    context.TokenLine.Tokens[tokenIndex].PositionRange),
                _ => throw new UnexpectedContextException()
            },
            _ => throw new UnexpectedContextException()
        };
    }

    public static ParsingFail CouldntParseNumberXNumber(this ErrMsgContext context, int tokenIndex)
    {
        return context.AssertionLevel switch
        {
            AssertionLevel.Line => context.ExpectationType switch
            {
                ExpectationType.GridDefinition => new ParsingFail(
                    "Could not parse as a Number-by-Number pair, which is expected in this position in an Grid Defintion",
                    GridDefContextualInfo,
                    context.TokenLine.TextLine,
                    context.TokenLine.Tokens[tokenIndex].PositionRange),
                _ => throw new UnexpectedContextException()
            },
            _ => throw new UnexpectedContextException()
        };
    }

    public static ParsingFail CouldntParseStandaloneLetter(this ErrMsgContext context, int tokenIndex)
    {
        return context.AssertionLevel switch
        {
            AssertionLevel.Line => context.ExpectationType switch
            {
                ExpectationType.RobotPosition => new ParsingFail(
                    "Could not parse as a Compass Direction, which is expected in this position in a Robot Position Defintion",
                    RobotPositionContextualInfo,
                    context.TokenLine.TextLine,
                    context.TokenLine.Tokens[tokenIndex].PositionRange),
                _ => throw new UnexpectedContextException()
            },
            _ => throw new UnexpectedContextException()
        };
    }

    public static ParsingFail CouldntParseWordAsLetterSequence(this ErrMsgContext context, int tokenIndex)
    {
        return context.AssertionLevel switch
        {
            AssertionLevel.Line => context.ExpectationType switch
            {
                ExpectationType.Instructions => new ParsingFail(
                    "Could not parse as a Instructions letter sequence",
                    InstructionContextualInfo,
                    context.TokenLine.TextLine,
                    Option<(int, int)>.None),
                _ => throw new UnexpectedContextException()
            },
            _ => throw new UnexpectedContextException()

        };
    }

    public static ParsingFail CouldntParseWordAsKeyword(this ErrMsgContext context, int tokenIndex)
    {
        return context.AssertionLevel switch
        {
            AssertionLevel.Line => context.ExpectationType switch
            {
                ExpectationType.GridDefinition => new ParsingFail(
                    "Could not parse word as GRID keyword, expecting Grid Defintion at this point in the document",
                    GridDefContextualInfo,
                    context.TokenLine.TextLine,
                    context.TokenLine.Tokens[tokenIndex].PositionRange),
                ExpectationType.Obstacles => new ParsingFail(
                    "Could not parse word as OBSTACLE keyword, currently expecting Obstacle Defintion because this section has already failed to be parsed as a Journey Defintion",
                    ObstacleAndJourneyContextualInfo,
                    context.TokenLine.TextLine,
                    context.TokenLine.Tokens[tokenIndex].PositionRange),
                _ => throw new UnexpectedContextException()
            },
            _ => throw new UnexpectedContextException()
        };
    }

    public static ParsingFail DidntFindExpectedTokenTypes(this ErrMsgContext context)
    {
        return context.AssertionLevel switch
        {
            AssertionLevel.Line => context.ExpectationType switch
            {
                ExpectationType.GridDefinition => new ParsingFail(
                    "Failed to find the expected line structure, expecting Grid Defintion",
                    GridDefContextualInfo,
                    context.TokenLine.TextLine, Option<(int, int)>.None),

                ExpectationType.Obstacles => new ParsingFail(
                    "Failed to find the expected line structure, currently expecting Obstacle Defintion because this section has already failed to be parsed as a Journey Defintion",
                    ObstacleAndJourneyContextualInfo,
                    context.TokenLine.TextLine, Option<(int, int)>.None),

                ExpectationType.RobotPosition => new ParsingFail(
                    "Failed to find the expected line structure, expecting Robot Position",
                    RobotPositionContextualInfo,
                    context.TokenLine.TextLine, Option<(int, int)>.None),

                ExpectationType.Instructions => new ParsingFail(
                    "Failed to find the expected line structure, expecting Instructions",
                    InstructionContextualInfo,
                    context.TokenLine.TextLine, Option<(int, int)>.None),
                _ => throw new UnexpectedContextException()
            },
            _ => throw new UnexpectedContextException()
        };
    }

    public static ParsingFail DidntFindExpectedLength(this ErrMsgContext context)
    {
        return (context.ExpectationType, context.SectionNumber) switch
        {
            (ExpectationType.GridDefinition, 1) => new ParsingFail(
                "The first section is expected to be a Grid Defintion",
                GridDefContextualInfo,
                context.Chunk.ToTextLines()),
            (ExpectationType.Journey, int i) when i > 1 => new ParsingFail(
                "This section has an unexpected length, expected Journey Defintion",
                JourneyContextualInfo,
                context.Chunk.ToTextLines()),
            _ => throw new UnexpectedContextException()
        };

    }

    public static ErrMsgContext GridDef(this ErrMsgContext context, Chunk<TokenLine> chunkLines)
    {
        return new ErrMsgContext(context.SectionNumber + 1,
            ExpectationType.GridDefinition, AssertionLevel.Chunk, chunkLines);
    }

    public static ErrMsgContext Obstacles(this ErrMsgContext context, Chunk<TokenLine> chunkLines)
    {
        return new ErrMsgContext(context.SectionNumber + 1,
            ExpectationType.Obstacles, AssertionLevel.Chunk, chunkLines);
    }

    public static ErrMsgContext Journey(this ErrMsgContext context, Chunk<TokenLine> chunkLines)
    {
        return new ErrMsgContext(context.SectionNumber + 1,
            ExpectationType.Journey, AssertionLevel.Chunk, chunkLines);
    }

    public static ErrMsgContext RobotPosition(this ErrMsgContext context, TokenLine tokenLine)
    {
        return new ErrMsgContext(context.SectionNumber,
            ExpectationType.RobotPosition, AssertionLevel.Line, tokenLine);
    }

    public static ErrMsgContext Instructions(this ErrMsgContext context, TokenLine tokenLine)
    {
        return new ErrMsgContext(context.SectionNumber,
            ExpectationType.Instructions, AssertionLevel.Line, tokenLine);
    }

    public static ErrMsgContext Line(this ErrMsgContext context, TokenLine tokenLine)
    {
        return new ErrMsgContext(context.SectionNumber,
            context.ExpectationType, AssertionLevel.Line, tokenLine);
    }

    public static List<ContextualInfo> GridDefContextualInfo = new()
    {
        new ContextualInfo(ShouldBeAndExamples.GridDefinitionShouldBe,
            ShouldBeAndExamples.GridDefinitionExamples)
    };

    public static List<ContextualInfo> JourneyContextualInfo = new()
    {
        new ContextualInfo(ShouldBeAndExamples.JourneyDefinitionShouldBe,
            ShouldBeAndExamples.JourneyDefinitionExamples)
    };

    public static List<ContextualInfo> InstructionContextualInfo = new()
    {
        new ContextualInfo(ShouldBeAndExamples.InstructionDefinitionShouldBe,
            ShouldBeAndExamples.InstructionDefinitionExamples)
    };

    public static List<ContextualInfo> RobotPositionContextualInfo = new()
    {
        new ContextualInfo(ShouldBeAndExamples.RobotPositionDefinitionShouldBe,
            ShouldBeAndExamples.RobotPositionDefinitionExamples)
    };

    public static List<ContextualInfo> ObstacleAndJourneyContextualInfo = new()
    {
        new ContextualInfo(ShouldBeAndExamples.ObstaclesDefinitionShouldBe,
            ShouldBeAndExamples.ObstaclesDefinitionExamples),
        new ContextualInfo(ShouldBeAndExamples.JourneyDefinitionShouldBe,
            ShouldBeAndExamples.JourneyDefinitionExamples),
    };
}
