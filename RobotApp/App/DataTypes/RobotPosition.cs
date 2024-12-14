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

    public override string ToString()
    {
        return $"{Coordinates.Item1} {Coordinates.Item2} {toChar(FacingDirection)}";
    }

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
    public static RobotPosition turnRobotLeft(RobotPosition robotPosition)
    {
        var newFacingDirection = turnLeft(robotPosition.FacingDirection);
        return new RobotPosition()
        {
            Coordinates = robotPosition.Coordinates,
            FacingDirection = newFacingDirection
        };
    }

    public static RobotPosition turnRobotRight(RobotPosition robotPosition)
    {
        var newFacingDirection = turnRight(robotPosition.FacingDirection);
        return new RobotPosition()
        {
            Coordinates = robotPosition.Coordinates,
            FacingDirection = newFacingDirection
        };
    }

    public static RobotPosition moveRobotForward(RobotPosition robotPosition)
    {
        var coordIncrement = robotPosition.FacingDirection switch
        {
            CompassDirection.North => (0, 1),
            CompassDirection.South => (0, -1),
            CompassDirection.West => (-1, 0),
            CompassDirection.East => (1, 0),
            _ => throw new UnexpectedCompassDirectionException(),
        };

        return new RobotPosition()
        {
            Coordinates = (robotPosition.Coordinates.Item1 + coordIncrement.Item1,
                         robotPosition.Coordinates.Item2 + coordIncrement.Item2),
            FacingDirection = robotPosition.FacingDirection
        };
    }
}
