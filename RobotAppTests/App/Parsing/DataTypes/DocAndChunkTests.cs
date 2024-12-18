using FluentAssertions;
using LanguageExt;
using LanguageExt.UnitTesting;
using RobotApp.App.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotAppTests.App.Parsing.DataTypes;

[TestClass]
public class DocAndChunkTests
{
    public static string NineDetected = "nine detected";
    public Either<string, int> FuncFailsOn9(int i) => i == 9 ? NineDetected : i + 1;

    [TestMethod]
    public void SequenceDoc_ProducesALeft_IfFuncOfAnyElementProducesALeft()
    {
        var doc = new Doc<int>(
                new List<Chunk<int>>()
                {
                    new Chunk<int>(
                        new(){ 1, 2, 3 }
                    ),
                    new Chunk<int>(
                        new(){ 4, 5, 6 }
                    ),
                    new Chunk<int>(
                        new(){ 7, 8, 9 }
                    ),
                }
            );

        var result = doc.Sequence(FuncFailsOn9);

        result.ShouldBeLeft(str => str.Should().Be(NineDetected));
    }

    [TestMethod]
    public void SequenceDoc_ProducesAMappedRight_IfFuncOfAllElementsProducesAMappedRight()
    {
        var doc = new Doc<int>(
                new List<Chunk<int>>()
                {
                    new Chunk<int>(
                        new(){ 1, 0, 3 }
                    ),
                    new Chunk<int>(
                        new(){ 4, 5, 6 }
                    ),
                    new Chunk<int>(
                        new(){ 7, 8, 0 }
                    ),
                }
            );

        var result = doc.Sequence(FuncFailsOn9);

        var expected = new Doc<int>(
                new List<Chunk<int>>()
                {
                    new Chunk<int>(
                        new(){ 2, 1, 4 }
                    ),
                    new Chunk<int>(
                        new(){ 5, 6, 7 }
                    ),
                    new Chunk<int>(
                        new(){ 8, 9, 1 }
                    ),
                }
            );


        result.ShouldBeRight(doc => doc.Should().BeEquivalentTo(expected));
    }
}
