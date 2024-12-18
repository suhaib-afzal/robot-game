using FluentAssertions;
using RobotApp.App.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using static RobotApp.App.DataTypes.CompassDirectionFunctions;

namespace RobotAppTests.App.DataTypes;

[TestClass]
public class RobotPositionFunctionsTests
{
    [TestMethod]
    [DataRow(CompassDirection.North)]
    [DataRow(CompassDirection.South)]
    [DataRow(CompassDirection.West)]
    [DataRow(CompassDirection.East)]
    public void MoveForwardDoesNotChangeFacingDirection(CompassDirection direction)
    {
        //Arrange
        var startRobotPos = new RobotPosition((0, 0), direction);

        //Assert
        MoveRobotForward(startRobotPos).FacingDirection.Should().Be(direction);
    }

    [TestMethod]
    [DataRow(CompassDirection.North)]
    [DataRow(CompassDirection.South)]
    [DataRow(CompassDirection.West)]
    [DataRow(CompassDirection.East)]
    public void TurnRobotLeft_ShouldBe_SameAsTurnCompassDirectionLeft(CompassDirection direction)
    {
        //Arrange
        var startRobotPos = new RobotPosition((0, 0), direction);

        //Assert
        TurnRobotLeft(startRobotPos).FacingDirection.Should().Be(TurnLeft(direction));
    }

    [TestMethod]
    [DataRow(CompassDirection.North)]
    [DataRow(CompassDirection.South)]
    [DataRow(CompassDirection.West)]
    [DataRow(CompassDirection.East)]
    public void TurnRobotRight_ShouldBe_SameAsTurnCompassDirectionRight(CompassDirection direction)
    {
        //Arrange
        var startRobotPos = new RobotPosition((0, 0), direction);

        //Assert
        TurnRobotRight(startRobotPos).FacingDirection.Should().Be(TurnRight(direction));
    }

    [TestMethod]
    public void MoveWest_DecreasesXValue_YStaysSame()
    {
        //Arrange
        var startRobotPos = new RobotPosition((0, 0), CompassDirection.West);

        //Assert
        MoveRobotForward(startRobotPos).Coordinates.Should().Be((-1,0));
    }

    [TestMethod]
    public void MoveEast_IncreasesXValue_YStaysSame()
    {
        //Arrange
        var startRobotPos = new RobotPosition((0, 0), CompassDirection.East);

        //Assert
        MoveRobotForward(startRobotPos).Coordinates.Should().Be((1,0));
    }

    [TestMethod]
    public void MoveNorth_IncreasesYValue_XStaysSame()
    {
        //Arrange
        var startRobotPos = new RobotPosition((0, 0), CompassDirection.North);

        //Assert
        MoveRobotForward(startRobotPos).Coordinates.Should().Be((0,1));
    }

    [TestMethod]
    public void MoveSouth_IncreasesYValue_XStaysSame()
    {
        //Arrange
        var startRobotPos = new RobotPosition((0, 0), CompassDirection.South);

        //Assert
        MoveRobotForward(startRobotPos).Coordinates.Should().Be((0,-1));
    }

}
