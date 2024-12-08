using RobotApp.App.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.DataTypes;

public enum CompassDirection
{
    North,
    South,
    West,
    East,
}

public static class CompassDirectionFunctions
{
    private static string unexpectedCompassDirectionMsg = ;

    public static CompassDirection turnLeft(CompassDirection direction)
    {
        return direction switch
        {
            CompassDirection.North => CompassDirection.West,
            CompassDirection.South => CompassDirection.East,
            CompassDirection.West => CompassDirection.South,
            CompassDirection.East => CompassDirection.North,
            _ => throw new UnexpectedCompassDirectionException(),
        };
    }

    public static CompassDirection turnRight(CompassDirection direction)
    {
        return direction switch
        {
            CompassDirection.North => CompassDirection.East,
            CompassDirection.South => CompassDirection.West,
            CompassDirection.West => CompassDirection.North,
            CompassDirection.East => CompassDirection.South,
            _ => throw new UnexpectedCompassDirectionException(),
        };
    }

    public static char toChar(CompassDirection direction)
    {
        return direction switch
        {
            CompassDirection.North => 'N',
            CompassDirection.South => 'S',
            CompassDirection.West => 'W',
            CompassDirection.East => 'E',
            _ => throw new UnexpectedCompassDirectionException(),
        };
    }
}
