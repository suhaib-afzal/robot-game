using RobotApp.App.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.App.DataTypes.CompassDirectionFunctions;

namespace RobotApp.App.DataTypes;

public class RobotPosition
{
    public RobotPosition((int, int) coordinates, CompassDirection facingDirection)
    {
        Coordinates = coordinates;
        FacingDirection = facingDirection;
    }

    public (int, int) Coordinates { get; set; }

    public CompassDirection FacingDirection { get; set; }

    public override bool Equals(object obj)
    {
        return obj is RobotPosition && Equals(obj as RobotPosition);
    }

    private bool Equals(RobotPosition robotPosition)
    {
        return robotPosition.Coordinates == Coordinates
               && robotPosition.FacingDirection == FacingDirection;
    }
}

public static class RobotPositionFunctions
{
    public static RobotPosition TurnRobotLeft(RobotPosition robotPosition)
    {
        var newFacingDirection = TurnLeft(robotPosition.FacingDirection);
        return new RobotPosition(robotPosition.Coordinates, newFacingDirection);
    }

    public static RobotPosition TurnRobotRight(RobotPosition robotPosition)
    {
        var newFacingDirection = TurnRight(robotPosition.FacingDirection);
        return new RobotPosition(robotPosition.Coordinates, newFacingDirection);
    }

    public static RobotPosition MoveRobotForward(RobotPosition robotPosition)
    {
        var coordIncrement = robotPosition.FacingDirection switch
        {
            CompassDirection.North => (0, 1),
            CompassDirection.South => (0, -1),
            CompassDirection.West => (-1, 0),
            CompassDirection.East => (1, 0),
            _ => throw new UnexpectedCompassDirectionException(),
        };
        var newCoords = (robotPosition.Coordinates.Item1 + coordIncrement.Item1,
                         robotPosition.Coordinates.Item2 + coordIncrement.Item2);

        return new RobotPosition(newCoords, robotPosition.FacingDirection);
    }
}
