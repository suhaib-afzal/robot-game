using LanguageExt;
using static RobotApp.App.Parsing.Utility.AltFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt.UnitTesting;
using FluentAssertions;

namespace RobotAppTests.App.Parsing.Utility;

//TODO: Install Moq and assert that WillFaill and WillSucceed are 
//      called the expected number of times per call of combinedFunc
[TestClass]
public class AltFunctionsTests
{
    public static Option<T> WillFail<T>(T t) => Option<T>.None;

    public static Option<T> WillSucceed<T>(T t) => Option<T>.Some(t);

    [TestMethod]
    public void WhenBothSucceed_Alt_Succeeds()
    {
        //Arrange
        var combinedFunc = Alt<int, int>(WillSucceed, WillSucceed);

        //Act
        var one = combinedFunc(1);
        var ten = combinedFunc(10);
        var large = combinedFunc(9898);
        var larger = combinedFunc(938989889);
        var neg = combinedFunc(-8373);
        var negLarge = combinedFunc(-73873838);

        one.ShouldBeSome(x => x.Should().Be(1));
        ten.ShouldBeSome(x => x.Should().Be(10));
        large.ShouldBeSome(x => x.Should().Be(9898));
        larger.ShouldBeSome(x => x.Should().Be(938989889));
        neg.ShouldBeSome(x => x.Should().Be(-8373));
        negLarge.ShouldBeSome(x => x.Should().Be(-73873838));
    }

    [TestMethod]
    public void WhenFirstSucceeds_Alt_Succeeds()
    {
        //Arrange
        var combinedFunc = Alt<int, int>(WillSucceed, WillFail);

        //Act
        var one = combinedFunc(1);
        var ten = combinedFunc(10);
        var large = combinedFunc(9898);
        var larger = combinedFunc(938989889);
        var neg = combinedFunc(-8373);
        var negLarge = combinedFunc(-73873838);

        one.ShouldBeSome(x => x.Should().Be(1));
        ten.ShouldBeSome(x => x.Should().Be(10));
        large.ShouldBeSome(x => x.Should().Be(9898));
        larger.ShouldBeSome(x => x.Should().Be(938989889));
        neg.ShouldBeSome(x => x.Should().Be(-8373));
        negLarge.ShouldBeSome(x => x.Should().Be(-73873838));
    }

    [TestMethod]
    public void WhenSecondSucceeds_Alt_Succeeds()
    {
        //Arrange
        var combinedFunc = Alt<int, int>(WillFail, WillSucceed);

        //Act
        var one = combinedFunc(1);
        var ten = combinedFunc(10);
        var large = combinedFunc(9898);
        var larger = combinedFunc(938989889);
        var neg = combinedFunc(-8373);
        var negLarge = combinedFunc(-73873838);

        one.ShouldBeSome(x => x.Should().Be(1));
        ten.ShouldBeSome(x => x.Should().Be(10));
        large.ShouldBeSome(x => x.Should().Be(9898));
        larger.ShouldBeSome(x => x.Should().Be(938989889));
        neg.ShouldBeSome(x => x.Should().Be(-8373));
        negLarge.ShouldBeSome(x => x.Should().Be(-73873838));
    }

    [TestMethod]
    public void WhenNeitherSucceed_Alt_Fails()
    {
        //Arrange
        var combinedFunc = Alt<int, int>(WillFail, WillFail);

        //Act
        var one = combinedFunc(1);
        var ten = combinedFunc(10);
        var large = combinedFunc(9898);
        var larger = combinedFunc(938989889);
        var neg = combinedFunc(-8373);
        var negLarge = combinedFunc(-73873838);

        one.ShouldBeNone();
        ten.ShouldBeNone();
        large.ShouldBeNone();
        larger.ShouldBeNone();
        neg.ShouldBeNone();
        negLarge.ShouldBeNone();
    }

    [TestMethod]
    public void IfOneInChainSucceeds_AltChainSucceeds()
    {
        //Arrange
        var combinedFunc = AltChain<string, string>(new() {
            WillFail,
            WillFail,
            WillFail,
            WillSucceed,
            WillFail
        });

        //Act
        var one = combinedFunc("one");
        var ten = combinedFunc("ten");
        var strin = combinedFunc("strin");
        var random = combinedFunc("sdjdjddjioe93");

        one.ShouldBeSome(s => s.Should().Be("one"));
        ten.ShouldBeSome(s => s.Should().Be("ten"));
        strin.ShouldBeSome(s => s.Should().Be("strin"));
        random.ShouldBeSome(s => s.Should().Be("sdjdjddjioe93"));
    }

    [TestMethod]
    public void IfNoneInChainSucceeds_AltChainSucceeds()
    {
        //Arrange
        var combinedFunc = AltChain<string, string>(new() {
            WillFail,
            WillFail,
            WillFail,
            WillFail
        });

        //Act
        var one = combinedFunc("one");
        var ten = combinedFunc("ten");
        var strin = combinedFunc("strin");
        var random = combinedFunc("sdjdjddjioe93");

        one.ShouldBeNone();
        ten.ShouldBeNone();
        strin.ShouldBeNone();
        random.ShouldBeNone();
    }
}
