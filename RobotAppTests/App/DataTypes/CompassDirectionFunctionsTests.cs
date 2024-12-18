using RobotApp.App.DataTypes;
using static RobotApp.App.DataTypes.CompassDirectionFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace RobotAppTests.App.DataTypes;

[TestClass]
public class CompassDirectionFunctionsTests
{
    [TestMethod]
    [DataRow(CompassDirection.North)]
    [DataRow(CompassDirection.South)]
    [DataRow(CompassDirection.West)]
    [DataRow(CompassDirection.East)]
    public void TurnRightThenTurnLeft_IsIdentity(CompassDirection direction)
    {
        //Act
        var result = TurnLeft(TurnRight(direction));

        //Assert
        result.Should().Be(direction);
    }


    [TestMethod]
    [DataRow(CompassDirection.North)]
    [DataRow(CompassDirection.South)]
    [DataRow(CompassDirection.West)]
    [DataRow(CompassDirection.East)]
    public void TurnLeftThenTurnRight_IsIdentity(CompassDirection direction)
    {
        //Act
        var result = TurnRight(TurnLeft(direction));

        //Assert
        result.Should().Be(direction);
    }

    [TestMethod]
    [DataRow(CompassDirection.North)]
    [DataRow(CompassDirection.South)]
    [DataRow(CompassDirection.West)]
    [DataRow(CompassDirection.East)]
    public void TurnLeftFourTimes_IsIdentity(CompassDirection direction)
    {
        //Act
        var result = TurnLeft(TurnLeft(TurnLeft(TurnLeft(direction))));

        //Assert
        result.Should().Be(direction);
    }

    [TestMethod]
    [DataRow(CompassDirection.North)]
    [DataRow(CompassDirection.South)]
    [DataRow(CompassDirection.West)]
    [DataRow(CompassDirection.East)]
    public void TurnRightFourTimes_IsIdentity(CompassDirection direction)
    {
        //Act
        var result = TurnRight(TurnRight(TurnRight(TurnRight(direction))));

        //Assert
        result.Should().Be(direction);
    }

    [TestMethod]
    public void TurnLeft_North_Works()
    {
        TurnLeft(CompassDirection.North).Should().Be(CompassDirection.West);
    }

    [TestMethod]
    public void TurnLeft_West_Works()
    {
        TurnLeft(CompassDirection.West).Should().Be(CompassDirection.South);
    }

    [TestMethod]
    public void TurnLeft_South_Works()
    {
        TurnLeft(CompassDirection.South).Should().Be(CompassDirection.East);
    }

    [TestMethod]
    public void TurnLeft_East_Works()
    {
        TurnLeft(CompassDirection.East).Should().Be(CompassDirection.North);
    }

    [TestMethod]
    public void TurnRight_North_Works()
    {
        TurnRight(CompassDirection.North).Should().Be(CompassDirection.East);
    }

    [TestMethod]
    public void TurnRight_East_Works()
    {
        TurnRight(CompassDirection.East).Should().Be(CompassDirection.South);
    }

    [TestMethod]
    public void TurnRight_South_Works()
    {
        TurnRight(CompassDirection.South).Should().Be(CompassDirection.West);
    }

    [TestMethod]
    public void TurnRight_West_Works()
    {
        TurnRight(CompassDirection.West).Should().Be(CompassDirection.North);
    }

}
