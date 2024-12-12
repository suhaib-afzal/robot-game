using FluentAssertions;
using RobotApp.Parsing.DataTypes;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.Parsing.Tokenization.TokenizerFunctions;
using RobotApp.Parsing.Tokenization;
using LanguageExt.UnitTesting;

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
                lineNumber,
                new List<Token>()
                {
                    new Token("kdodkodp", TokenType.Word),
                    new Token("kdodk", TokenType.Word),
                    new Token("ododeo", TokenType.Word),
                });

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
                lineNumber,
                new List<Token>()
                {
                    new Token("98789", TokenType.Number),
                    new Token("0", TokenType.Number),
                    new Token("9090", TokenType.Number),
                    new Token("4841", TokenType.Number),
                });

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
                lineNumber,
                new List<Token>()
                {
                    new Token("k", TokenType.StandaloneLetter),
                    new Token("L", TokenType.StandaloneLetter),
                    new Token("A", TokenType.StandaloneLetter),
                    new Token("L", TokenType.StandaloneLetter),
                });

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
                lineNumber,
                new List<Token>()
                {
                    new Token("10x7", TokenType.NumberxNumber),
                    new Token("900x8", TokenType.NumberxNumber)
                });

        eitherTokenLine.ShouldBeRight(tokenLine =>
            tokenLine.Should().BeEquivalentTo(expected)
         );
    }

    [TestMethod]
    public void ValidInput_WhenInputMixed_TokenizeShouldWork()
    {
        //Arrange
        var lineNumber = 10;
        var textLine = new TextLine(" kkodkd L 9393 003x88 o dodiijdf", lineNumber);

        //Act
        var eitherTokenLine = Tokenize(textLine);

        //Assert
        var expected = new TokenLine(
                lineNumber,
                new List<Token>()
                {
                    new Token("kkodkd", TokenType.Word),
                    new Token("L", TokenType.StandaloneLetter),
                    new Token("9393", TokenType.Number),
                    new Token("003x88", TokenType.NumberxNumber),
                    new Token("o", TokenType.StandaloneLetter),
                    new Token("dodiijdf", TokenType.Word),
                });

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
        var expected = new TokenizeFail(TokenizeFailMesg, lineText, lineNumber);

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
        var expected = new TokenizeFail(TokenizeFailMesg, lineText, lineNumber);

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
        var expected = new TokenizeFail(TokenizeFailMesg, lineText, lineNumber);

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
                        new TokenLine(1, new() {
                            new Token("jiedii", TokenType.Word),
                            new Token("238x99", TokenType.NumberxNumber)
                        })
                    }
                ),
                new Chunk<TokenLine>(
                    new()
                    {
                        new TokenLine(3, new() {
                            new Token("jeidjije", TokenType.Word),
                            new Token("2", TokenType.Number),
                            new Token("3", TokenType.Number)
                        }),
                        new TokenLine(4, new() {
                            new Token("ooooooo", TokenType.Word),
                            new Token("9", TokenType.Number),
                            new Token("3", TokenType.Number),
                            new Token("2", TokenType.Number)})
                    }        
                ),
                new Chunk<TokenLine>(new()
                    {
                        new TokenLine(6, new() {
                            new Token("1", TokenType.Number),
                            new Token("2", TokenType.Number),
                            new Token("L", TokenType.StandaloneLetter),
                            new Token("K", TokenType.StandaloneLetter)
                        }),
                        new TokenLine(7, new() {
                            new Token("KKKKKKKKLL", TokenType.Word)
                        }),
                        new TokenLine(8, new() {
                            new Token("84", TokenType.Number),
                            new Token("994", TokenType.Number),
                            new Token("J", TokenType.StandaloneLetter),
                            new Token("K", TokenType.StandaloneLetter) 
                        })
                    }
                ),
                new Chunk<TokenLine>(new()
                    {
                        new TokenLine(10, new() {
                            new Token("39", TokenType.Number),
                            new Token("99", TokenType.Number),
                            new Token("I", TokenType.StandaloneLetter),
                            new Token("I", TokenType.StandaloneLetter),
                            new Token("O", TokenType.StandaloneLetter),
                            new Token("P", TokenType.StandaloneLetter) 
                        }),
                        new TokenLine(11, new() {
                            new Token("eiojfjfi", TokenType.Word) 
                        })
                    }
                )    
            }
        );

        eithTokenDoc.ShouldBeRight(tokenDoc =>
            tokenDoc.Should().BeEquivalentTo(expected, options => options.AllowingInfiniteRecursion())
        );
    }
}
