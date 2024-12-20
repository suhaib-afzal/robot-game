﻿using RobotApp.App.Exceptions;

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

    public static CompassDirection TurnLeft(CompassDirection direction)
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

    public static CompassDirection TurnRight(CompassDirection direction)
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

    public static char ToChar(CompassDirection direction)
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
