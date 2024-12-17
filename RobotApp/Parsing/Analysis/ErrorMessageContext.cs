using LanguageExt;
using RobotApp.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Analysis;

public class ErrMsgContext
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

    public int SectionNumber { get; set; }

    public ExpectationType ExpectationType { get; set; }

    public AssertionLevel AssertionLevel { get; set; }

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


public static class ErrMsgContextFunctions
{

    public static ParsingFail CouldntParseNumber(this ErrMsgContext context, int tokenIndex)
    {
        return context.AssertionLevel switch
        {
            AssertionLevel.Line => context.ExpectationType switch
            {
                ExpectationType.RobotPosition => new ParsingFail(
                    "Could not parse as a number, expecting number in this position in an Robot Position defintion",
                    RobotPositionDefinitionShouldBe,
                    RobotPositionDefinitionExamples,
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
                    GridDefinitionShouldBe,
                    GridDefinitionExamples,
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
                    "Could not parse as a Standalone Letter, which is expected in this position in a Robot Position Defintion",
                    RobotPositionDefinitionShouldBe,
                    RobotPositionDefinitionExamples,
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
                    InstructionDefinitionShouldBe,
                    InstructionDefinitionExamples,
                    context.TokenLine.TextLine),
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
                    GridDefinitionShouldBe,
                    GridDefinitionExamples,
                    context.TokenLine.TextLine,
                    context.TokenLine.Tokens[tokenIndex].PositionRange),
                ExpectationType.Obstacles => new ParsingFail(
                    "Could not parse word as OBSTACLE keyword, expecting Obstacle Defintion as failed to parse this section as Journey defintion",
                    ObstaclesDefinitionShouldBe,
                    ObstaclesDefinitionExamples,
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
                    GridDefinitionShouldBe, GridDefinitionExamples, context.TokenLine.TextLine),
                ExpectationType.Obstacles => new ParsingFail(
                    "Failed to find the expected line structure, expecting Obstacle Defintion, as this section has already failed to be parsed as Journey Defintion",
                    ObstaclesDefinitionShouldBe, ObstaclesDefinitionExamples, context.TokenLine.TextLine),
                ExpectationType.RobotPosition => new ParsingFail(
                    "Failed to find the expected line structure, expecting Robot Position",
                    RobotPositionDefinitionShouldBe, RobotPositionDefinitionExamples, context.TokenLine.TextLine),
                ExpectationType.Instructions => new ParsingFail(
                    "Failed to find the expected line structure, expecting Instructions",
                    InstructionDefinitionShouldBe, InstructionDefinitionExamples, context.TokenLine.TextLine),
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
                GridDefinitionShouldBe,
                GridDefinitionExamples,
                context.Chunk.ToTextLines()),
            (ExpectationType.Journey, int i) when i > 1 => new ParsingFail(
                "This section has an unexpected length, expected Journey Defintion",
                JourneyDefinitionShouldBe,
                JourneyDefinitionExamples,
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


    public static string GridDefinitionShouldBe = @"Grid Definition should be GRID followed by NumxNum, followed by an empty line";

    public static List<string> GridDefinitionExamples = new List<string>()
    {
        "GRID 4x5",
        "GRID 10x8",
        "GRID 209x80"
    };

    public static string ObstaclesDefinitionShouldBe = @"Obstacles Definition should be multiple lines of OBSTACLE followed by Num Num, the section to end with an empty line";

    public static List<string> ObstaclesDefinitionExamples = new List<string>()
    {
        @"OBSTACLE 1 100
          OBSTACLE 7 99
          OBSTACLE 8 88",

        @"OBSTACLE 6 7",

        @"OBSTACLE 9 0
          OBSTACLE 0 2
          OBSTACLE 1000 10000
          OBSTACLE 880 73",
    };

    public static string RobotPositionDefinitionShouldBe = @"Robot Position should be defined as 1 line with Num Num then a Compass Direction as a single letter";

    public static List<string> RobotPositionDefinitionExamples = new List<string>()
    {
        "1 1 N",
        "90 28 E",
        "89 50 W",
    };

    public static string InstructionDefinitionShouldBe = @"The Instructions should be defined on 1 line as a sequence of Fs Rs and Ls";

    public static List<string> InstructionDefinitionExamples = new List<string>()
    {
        "RRRRRRRRRR",
        "RLLLRFF",
        "FFFLLR",
    };

    public static string JourneyDefinitionShouldBe
        = @$"The journey defintion should be 3 lines, with the start robot position, then the instructions, then the goal robot position, the section should end with an empty line
{RobotPositionDefinitionShouldBe}
{InstructionDefinitionShouldBe}";

    public static List<string> JourneyDefinitionExamples = new List<string>()
    {
        @"89 4 W
LLFF
7 0 E",

        @"7 0 N
RRRL
0 0 E",

        @"40 40 N
FFFFFFFFFF
39 39 N",
    };
}
