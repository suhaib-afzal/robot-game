using RobotApp.App.Parsing.DataTypes;
using static RobotApp.App.Parsing.Schematization.SchematizerFunctions;
using static RobotApp.App.Parsing.Schematization.ErrMsgContextFunctions;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using LanguageExt.UnitTesting;
using RobotApp.App.Parsing.ParsingFailType;
using FluentAssertions;
using LanguageExt;
using RobotApp.App.DataTypes;

namespace RobotAppTests.App.Parsing.Analysis;

[TestClass]
public class SchematizerFunctionsTests
{
    [TestMethod]
    public void InvalidGridDefintion_InvalidGridKeyword_WillFail()
    {
        var problemSpan = (0, 4);
        var problemLine = new TextLine("Gris 10x10", 1);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("Gris", TokenType.Word, problemSpan),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            problemLine
                        )
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Could not parse word as GRID keyword, expecting Grid Defintion at this point in the document",
                GridDefContextualInfo,
                problemLine,
                problemSpan
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidGridDefintion_InvalidGridNumOfLines_WillFail()
    {
        var problemLine1 = new TextLine("GRID ", 1);
        var problemLine2 = new TextLine("10x10", 2);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                            },
                            new TextLine("GRID ", 1)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("10x10", TokenType.NumberxNumber, (0,5))
                            },
                            new TextLine("10x10", 2)
                        )
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "The first section is expected to be a Grid Defintion",
                GridDefContextualInfo,
                new() { problemLine1, problemLine2  }
            );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidGridDefintion_InvalidStructure_WillFail()
    {
        var problemLine = new TextLine("4 10x10", 1);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("4", TokenType.Number, (0, 1)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("4 10x10", 1)
                        )
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Failed to find the expected line structure, expecting Grid Defintion",
                GridDefContextualInfo,
                problemLine,
                Option<(int,int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }


    [TestMethod]
    public void InvalidSecondSectionJourneyDefintion_InvalidFirstRobotPosition_WillFailWithObstacles()
    {
        var problemLine = new TextLine("1 1 K", 3);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("K", TokenType.StandaloneLetter, (4,5))
                            },
                            problemLine
                        )
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Failed to find the expected line structure, currently expecting Obstacle Defintion because this section has already failed to be parsed as a Journey Defintion",
                ObstacleAndJourneyContextualInfo,
                problemLine,
                Option<(int, int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidSecondSectionJourneyDefintion_InvalidInstructions_WillFail()
    {
        var problemLine = new TextLine("1 1 N", 3);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            problemLine
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("HIJOOIOIJIOIO", TokenType.Word, (0, 13)),
                            },
                            new TextLine("HIJOOIOIJIOIO", 4)
                        )
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Failed to find the expected line structure, currently expecting Obstacle Defintion because this section has already failed to be parsed as a Journey Defintion",
                ObstacleAndJourneyContextualInfo,
                problemLine,
                Option<(int, int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidSecondSectionJourneyDefintion_InvalidLastRobotPosition_WillFail()
    {
        var problemLine = new TextLine("1 1 N", 3);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            problemLine
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFFLLRRR", TokenType.Word, (0, 8)),
                            },
                            new TextLine("FFFLLRRR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("L", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("L", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("L 3 L", 5)
                        ),
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Failed to find the expected line structure, currently expecting Obstacle Defintion because this section has already failed to be parsed as a Journey Defintion",
                ObstacleAndJourneyContextualInfo,
                problemLine,
                Option<(int, int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }


    [TestMethod]
    public void InvalidSecondSectionObstacles_InvalidStructure_WillFail()
    {
        var problemLine = new TextLine("899999 8 10", 3);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("899999", TokenType.Number, (0, 6)),
                                new Token("8", TokenType.Number, (8, 9)),
                                new Token("10", TokenType.Number, (10, 12)),
                            },
                            problemLine
                        ),
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Failed to find the expected line structure, currently expecting Obstacle Defintion because this section has already failed to be parsed as a Journey Defintion",
                ObstacleAndJourneyContextualInfo,
                problemLine,
                Option<(int, int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidSecondSectionObstacles_InvalidKeyword_WillFail()
    {
        var problemLine = new TextLine("JJJJJJ 8 10", 3);
        var problemSpan = (0, 6);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("JJJJJJ", TokenType.Word, (0, 6)),
                                new Token("8", TokenType.Number, (8, 9)),
                                new Token("10", TokenType.Number, (10, 12)),
                            },
                            problemLine
                        ),
                    }
                ),
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Could not parse word as OBSTACLE keyword, currently expecting Obstacle Defintion because this section has already failed to be parsed as a Journey Defintion",
                ObstacleAndJourneyContextualInfo,
                problemLine,
                problemSpan
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }


    [TestMethod]
    public void InvalidRobotPosition_InvalidStructure_WillFail()
    {
        var problemLine = new TextLine("1 1 dijd", 3);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 N", 3)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFFLLRRR", TokenType.Word, (0, 8)),
                            },
                            new TextLine("FFFLLRRR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 5)
                        ),
                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("dijd", TokenType.Word, (4,8))
                            },
                            new TextLine("1 1 dijd", 3)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFFLLRRR", TokenType.Word, (0, 8)),
                            },
                            new TextLine("FFFLLRRR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 5)
                        ),
                    }
                )
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Failed to find the expected line structure, expecting Robot Position",
                RobotPositionContextualInfo,
                problemLine,
                Option<(int, int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidRobotPosition_InvalidCompassDirection_WillFail()
    {
        var problemLine = new TextLine("1 1 K", 3);
        var problemSpan = (4, 5);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 N", 3)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFFLLRRR", TokenType.Word, (0, 8)),
                            },
                            new TextLine("FFFLLRRR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 5)
                        ),
                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("K", TokenType.StandaloneLetter, problemSpan)
                            },
                            problemLine
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFFLLRRR", TokenType.Word, (0, 8)),
                            },
                            new TextLine("FFFLLRRR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 X", 5)
                        ),
                    }
                )
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Could not parse as a Compass Direction, which is expected in this position in a Robot Position Defintion",
                RobotPositionContextualInfo,
                problemLine,
                problemSpan
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidInstructions_InvalidStructure_WillFail()
    {
        var problemLine = new TextLine("1000", 8);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 N", 3)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFFLLRRR", TokenType.Word, (0, 8)),
                            },
                            new TextLine("FFFLLRRR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 5)
                        ),
                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("E", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 E", 7)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1000", TokenType.Number, (0,4)),
                            },
                            problemLine
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 9)
                        ),
                    }
                )
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Failed to find the expected line structure, expecting Instructions",
                InstructionContextualInfo,
                problemLine,
                Option<(int,int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void InvalidInstructions_InvalidLetter_WillFail()
    {
        var problemLine = new TextLine("FFXXXFLL", 8);
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 N", 3)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFFLLRRR", TokenType.Word, (0, 8)),
                            },
                            new TextLine("FFFLLRRR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 5)
                        ),
                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("E", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 E", 7)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFXXXFLL", TokenType.Word, (0,8)),
                            },
                            problemLine
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 9)
                        ),
                    }
                )
            }
        );

        var result = Schematizer(doc);

        var expected = new ParsingFail(
                "Could not parse as a Instructions letter sequence",
                InstructionContextualInfo,
                problemLine,
                Option<(int, int)>.None
                );

        result.ShouldBeLeft(p => p.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void ValidDoc_ProducesGameSpec()
    {
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 N", 3)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FLR", TokenType.Word, (0, 3)),
                            },
                            new TextLine("FLR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 5)
                        ),
                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("2", TokenType.Number, (0, 1)),
                                new Token("2", TokenType.Number, (2,3)),
                                new Token("W", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 E", 7)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFF", TokenType.Word, (0,4)),
                            },
                            new TextLine("FFF", 8)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("8", TokenType.Number, (0, 1)),
                                new Token("9", TokenType.Number, (2,3)),
                                new Token("E", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("8 9 E", 9)
                        ),
                    }
                )
            }
        );

        var result = Schematizer(doc);

        var expected = new GameSpecification(
                new GridConstraints((10, 10), new()),
                new()
                {
                    new Journey(
                         new RobotPosition((1,3), CompassDirection.South),
                         new RobotPosition((1,1), CompassDirection.North),
                         new () { MoveRobotForward, TurnRobotLeft, TurnRobotRight }
                        ),

                    new Journey(
                         new RobotPosition((8,9), CompassDirection.East),
                         new RobotPosition((2,2), CompassDirection.West),
                         new () { MoveRobotForward, MoveRobotForward, MoveRobotForward }
                        )
                }
            );

        result.ShouldBeRight(spec => spec.Should().BeEquivalentTo(expected));
    }

    [TestMethod]
    public void ValidDocWithObstacles_ProducesGameSpec()
    {
        var doc = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("GRID", TokenType.Word, (0, 4)),
                                new Token("10x10", TokenType.NumberxNumber, (5,11))
                            },
                            new TextLine("GRID 10x10", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("OBSTACLE", TokenType.Word, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("1", TokenType.Number, (4,5))
                            },
                            new TextLine("OBSTACLE 1 1", 3)
                        ),
                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("1", TokenType.Number, (2,3)),
                                new Token("N", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 N", 3)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FLR", TokenType.Word, (0, 3)),
                            },
                            new TextLine("FLR", 4)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("1", TokenType.Number, (0, 1)),
                                new Token("3", TokenType.Number, (2,3)),
                                new Token("S", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 3 S", 5)
                        ),
                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {
                        new TokenLine(
                            new()
                            {
                                new Token("2", TokenType.Number, (0, 1)),
                                new Token("2", TokenType.Number, (2,3)),
                                new Token("W", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("1 1 E", 7)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("FFF", TokenType.Word, (0,4)),
                            },
                            new TextLine("FFF", 8)
                        ),
                        new TokenLine(
                            new()
                            {
                                new Token("8", TokenType.Number, (0, 1)),
                                new Token("9", TokenType.Number, (2,3)),
                                new Token("E", TokenType.StandaloneLetter, (4,5))
                            },
                            new TextLine("8 9 E", 9)
                        ),
                    }
                )
            }
        );

        var result = Schematizer(doc);

        var expected = new GameSpecification(
                new GridConstraints((10, 10), new() { (1,1) }),
                new()
                {
                    new Journey(
                         new RobotPosition((1,3), CompassDirection.South),
                         new RobotPosition((1,1), CompassDirection.North),
                         new () { MoveRobotForward, TurnRobotLeft, TurnRobotRight }
                        ),

                    new Journey(
                         new RobotPosition((8,9), CompassDirection.East),
                         new RobotPosition((2,2), CompassDirection.West),
                         new () { MoveRobotForward, MoveRobotForward, MoveRobotForward }
                        )
                }
            );

        result.ShouldBeRight(spec => spec.Should().BeEquivalentTo(expected));
    }
}
