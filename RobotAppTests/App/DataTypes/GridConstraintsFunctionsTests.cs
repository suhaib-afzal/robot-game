using FluentAssertions;
using RobotApp.App.DataTypes;
using static RobotApp.App.DataTypes.GridConstraintsFunctions;

namespace RobotAppTests.App.DataTypes;

[TestClass]
public class GridConstraintsFunctionsTests
{
    [TestMethod]
    [DataRow(10,10)]
    [DataRow(5,6)]
    [DataRow(1,6)]
    [DataRow(12,3)]
    public void ZeroZero_ShouldAlwaysBeWithinBounds(int x, int y)
    {
        //Arrange
        var robotPosition = new RobotPosition((0,0), CompassDirection.North);
        var gridConstraints = new GridConstraints((x, y), new());

        //Assert
        WithinBounds(gridConstraints, robotPosition).Should().BeTrue();
    }


    [TestMethod]
    [DataRow(0)]
    [DataRow(5)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(6)]
    [DataRow(9)]
    public void XEqualToGridX_ShouldAlwaysBeOutsideBounds(int y)
    {
        //Arrange
        var robotPosition = new RobotPosition((10, y), CompassDirection.North);
        var gridConstraints = new GridConstraints((10, 10), new());

        //Assert
        WithinBounds(gridConstraints, robotPosition).Should().BeFalse();
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(5)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(6)]
    [DataRow(9)]
    public void YEqualToGridY_ShouldAlwaysBeOutsideBounds(int x)
    {
        //Arrange
        var robotPosition = new RobotPosition((x, 10), CompassDirection.North);
        var gridConstraints = new GridConstraints((10, 10), new());

        //Assert
        WithinBounds(gridConstraints, robotPosition).Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1,2)]
    [DataRow(3,10)]
    [DataRow(5,5)]
    [DataRow(7,8)]
    [DataRow(15,6)]
    [DataRow(0,0)]
    [DataRow(1,4)]
    public void InsideGridBoundsExamples_ShouldGiveTrue(int x, int y)
    {
        //Arrange
        var robotPosition = new RobotPosition((x, y), CompassDirection.North);
        var gridConstraints = new GridConstraints((16, 11), new());

        //Assert
        WithinBounds(gridConstraints, robotPosition).Should().BeTrue();
    }

    [TestMethod]
    [DataRow(16,11)]
    [DataRow(-1, 12)]
    [DataRow(3, 12)]
    [DataRow(17, 5)]
    [DataRow(-10, -1)]
    public void OutsideGridBoundsExamples_ShouldGiveFalse(int x, int y)
    {
        //Arrange
        var robotPosition = new RobotPosition((x, y), CompassDirection.North);
        var gridConstraints = new GridConstraints((16, 11), new());

        //Assert
        WithinBounds(gridConstraints, robotPosition).Should().BeFalse();
    }
}
