using FluentAssertions;
using RobotApp.App.Parsing.DataTypes;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.App.Parsing.Tokenization.TokenizerFunctions;
using RobotApp.App.Parsing.Tokenization;
using LanguageExt.UnitTesting;
using RobotApp.App.Parsing;
using RobotApp.App.Parsing.ParsingFailType;
using LanguageExt.ClassInstances.Const;

namespace RobotAppTests.App.Parsing.Tokenization;

[TestClass]
public class TokenizerFunctionsTest
{
    [TestMethod]
    public void ValidInput_WhenInputWords_TokenizeShouldWork()
    {
        //Arrange
        var lineNumber = 0;
        var textLine = new TextLine("kdodkodp kdodk ododeo", lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new TokenLine(
                    new List<Token>()
                    {
                        new Token("kdodkodp", TokenType.Word, (0,8)),
                        new Token("kdodk", TokenType.Word, (9,14)),
                        new Token("ododeo", TokenType.Word, (15, 21)),
                    },
                    textLine
                );

        eitherTokenLine.ShouldBeRight(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }


    [TestMethod]
    public void ValidInput_WhenInputNumbers_TokenizeShouldWork()
    {
        //Arrange
        var lineNumber = 10;
        var textLine = new TextLine(" 98789 0 9090 4841 ", lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new TokenLine(
                new List<Token>()
                {
                    new Token("98789", TokenType.Number, (1,6)),
                    new Token("0", TokenType.Number, (7,8)),
                    new Token("9090", TokenType.Number, (9,13)),
                    new Token("4841", TokenType.Number, (14,18)),
                },
                textLine
            );

        eitherTokenLine.ShouldBeRight(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void ValidInput_WhenInputStandaloneLetters_TokenizeShouldWork()
    {
        //Arrange
        var lineNumber = 10;
        var textLine = new TextLine(" k L A L ", lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new TokenLine(
                new List<Token>()
                {
                    new Token("k", TokenType.StandaloneLetter, (1,2)),
                    new Token("L", TokenType.StandaloneLetter, (3,4)),
                    new Token("A", TokenType.StandaloneLetter, (5,6)),
                    new Token("L", TokenType.StandaloneLetter, (7,8)),
                },
                textLine
            );

        eitherTokenLine.ShouldBeRight(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void ValidInput_WhenInputNumberxNumber_TokenizeShouldWork()
    {
        //Arrange
        var lineNumber = 10;
        var textLine = new TextLine(" 10x7 900x8", lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new TokenLine(
                new List<Token>()
                {
                    new Token("10x7", TokenType.NumberxNumber, (1,5)),
                    new Token("900x8", TokenType.NumberxNumber, (6,11))
                },
                textLine
            );

        eitherTokenLine.ShouldBeRight(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void ValidInput_WhenInputMixed_TokenizeShouldWork()
    {
        //Arrange
        var lineNumber = 10;
        var textLine = new TextLine(" kkodkd L 9393 003x88 o dodiijdf ", lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new TokenLine(
                new List<Token>()
                {
                    new Token("kkodkd", TokenType.Word, (1,7)),
                    new Token("L", TokenType.StandaloneLetter, (8,9)),
                    new Token("9393", TokenType.Number, (10,14)),
                    new Token("003x88", TokenType.NumberxNumber, (15,21)),
                    new Token("o", TokenType.StandaloneLetter, (22,23)),
                    new Token("dodiijdf", TokenType.Word,(24,32)),
                },
                textLine
            );

        eitherTokenLine.ShouldBeRight(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void InvalidInput_UnexpectedCharecter_TokenizeShouldGiveLeft()
    {
        //Arrange
        var lineNumber = 10;
        var lineText = "838383 ! iiss";
        var textLine = new TextLine(lineText, lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new ParsingFail(TokenizeFailMesg, new(), textLine, Option<(int,int)>.None);

        eitherTokenLine.ShouldBeLeft(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void InvalidInput_UnexpectedSpace_TokenizeShouldGiveLeft()
    {
        //Arrange
        var lineNumber = 10;
        var lineText = "838383  iiss";
        var textLine = new TextLine(lineText, lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new ParsingFail(TokenizeFailMesg, new(), textLine, Option<(int, int)>.None);

        eitherTokenLine.ShouldBeLeft(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void InvalidInput_MixedAlphaNumericStrings_TokenizeShouldGiveLeft()
    {
        //Arrange
        var lineNumber = 10;
        var lineText = "838ii99ss";
        var textLine = new TextLine(lineText, lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new ParsingFail(TokenizeFailMesg, new(), textLine, Option<(int, int)>.None);

        eitherTokenLine.ShouldBeLeft(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void TokenizeDocumentWorks()
    {
        //Arrange
        var doc = @"jiedii 238x99

jeidjije 2 3
ooooooo 9 3 2
                   
1 2 L K
KKKKKKKKLL
84 994 J K

39 99 I I O P
eiojfjfi";

        //Act
        var eithTokenDoc = TokenizeDocument(doc);

        //Assert
        var expected = new Doc<TokenLine>(
            new() {
                new Chunk<TokenLine>(
                    new()
                    {
                        new TokenLine(
                            new() {
                                new Token("jiedii", TokenType.Word, (0,6)),
                                new Token("238x99", TokenType.NumberxNumber, (7,13))
                            }, 
                            new TextLine("jiedii 238x99", 1)
                        )
                    }
                ),
                new Chunk<TokenLine>(
                    new()
                    {
                        new TokenLine(
                            new() {
                                new Token("jeidjije", TokenType.Word, (0,8)),
                                new Token("2", TokenType.Number, (9,10)),
                                new Token("3", TokenType.Number, (11,12))
                            }, 
                            new TextLine("jeidjije 2 3", 3)
                        ),

                        new TokenLine( 
                            new() {
                                new Token("ooooooo", TokenType.Word, (0,7)),
                                new Token("9", TokenType.Number, (8,9)),
                                new Token("3", TokenType.Number, (10,11)),
                                new Token("2", TokenType.Number, (12,13))
                            },
                            new TextLine("ooooooo 9 3 2", 4)
                        )
                    }        
                ),
                new Chunk<TokenLine>(new()
                    {
                        new TokenLine(
                            new() {
                                new Token("1", TokenType.Number, (0,1)),
                                new Token("2", TokenType.Number, (2,3)),
                                new Token("L", TokenType.StandaloneLetter, (4,5)),
                                new Token("K", TokenType.StandaloneLetter, (6,7))
                            },
                            new TextLine("1 2 L K", 6)
                        ),
                        new TokenLine(
                            new() {
                                new Token("KKKKKKKKLL", TokenType.Word, (0,10))
                            },
                            new TextLine("KKKKKKKKLL", 7)
                        ),
                        new TokenLine(
                            new() {
                                new Token("84", TokenType.Number, (0,2)),
                                new Token("994", TokenType.Number, (3,6)),
                                new Token("J", TokenType.StandaloneLetter, (7,8)),
                                new Token("K", TokenType.StandaloneLetter, (9,10)) 
                            },
                            new TextLine("84 994 J K", 8)
                       )
                    }
                ),
                new Chunk<TokenLine>(new()
                    {
                        new TokenLine(
                            new() {
                                new Token("39", TokenType.Number, (0,2)),
                                new Token("99", TokenType.Number, (3,5)),
                                new Token("I", TokenType.StandaloneLetter,(6,7)),
                                new Token("I", TokenType.StandaloneLetter,(8,9)),
                                new Token("O", TokenType.StandaloneLetter,(10,11)),
                                new Token("P", TokenType.StandaloneLetter,(12,13)) 
                            },
                            new TextLine("39 99 I I O P", 10)
                        ),
                        new TokenLine(
                            new() {
                                new Token("eiojfjfi", TokenType.Word, (0,8)) 
                            },
                            new TextLine("eiojfjfi", 11)
                        )
                    }
                )    
            }
        );

        eithTokenDoc.ShouldBeRight(tokenDoc =>
            tokenDoc.Should().BeEquivalentTo(expected, options => options.AllowingInfiniteRecursion())
        );
    }
}
