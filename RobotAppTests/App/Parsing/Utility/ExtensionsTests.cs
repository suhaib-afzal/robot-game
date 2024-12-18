using FluentAssertions;
using RobotApp.App.Parsing.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotAppTests.App.Parsing.Utility;

[TestClass]
public class ExtensionsTests
{
    [TestMethod]
    public void SplitShouldSplitStringAtCommas()
    {
        //Act
        var actual = "jsdjdj, diojdi, dsjodj,ddkode".Split(c => c == ',');

        //Assert
        var expectedAsList = new List<string>() { 
            "jsdjdj", " diojdi", " dsjodj", "ddkode" };
        actual.Map(l => string.Concat(l)).ToList()
            .Should().BeEquivalentTo(expectedAsList);
    }

    [TestMethod]
    public void SplitShouldSplitIntsAtEvens()
    {
        //Act
        var actual = new List<int>() { 1, 2, 3, 5, 9, 4, 10, 101 }
            .Split(i => i % 2 == 0);

        //Assert
        var expectedAsList = new List<List<int>>() {
            new() { 1 },
            new() { 3, 5, 9 },
            new() { },
            new() { 101 } };

        actual.Map(l => l.ToList()).ToList()
            .Should().BeEquivalentTo(expectedAsList);
    }

    [TestMethod]
    public void SplitShouldSplitAtEmptyLines()
    {
        //Act
        var actual = "idjejedeije \n jdiedjioed \n \n idiejiejioe"
            .Split(c => c == '\n');

        //Assert
        var expectedAsList = new List<string>() {
            "idjejedeije ",
            " jdiedjioed ",
            " ",
            " idiejiejioe"
        };

        actual.Map(l => string.Concat(l)).ToList()
            .Should().BeEquivalentTo(expectedAsList);
    }


}
