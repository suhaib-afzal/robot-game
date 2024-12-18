using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.DataTypes;

//TODO:
//Add Mechanism to validate GridConstraints (not in this file necessarily)
//Validate that Obstacles are located inside Grid
//Ensure Grid dimensions are >= 1
public class GridConstraints
{
    public GridConstraints((int, int) gridDimensions, List<(int, int)> obstaclePositions)
    {
        GridDimensions = gridDimensions;
        ObstaclePositions = obstaclePositions;
    }

    public (int, int) GridDimensions { get; set; }

    public List<(int, int)> ObstaclePositions { get; set; }
}

public static class GridConstraintsFunctions
{
    public static bool WithinBounds(GridConstraints gridConstraints, RobotPosition robotPosition)
    {
        if (robotPosition.Coordinates.Item1 < 0 || robotPosition.Coordinates.Item2 < 0)
        {
            return false;
        }

        if (robotPosition.Coordinates.Item1 > gridConstraints.GridDimensions.Item1 - 1)
        {
            return false;
        }

        if (robotPosition.Coordinates.Item2 > gridConstraints.GridDimensions.Item2 - 1)
        {
            return false;
        }

        return true;
    }

    public static bool HasCrashedIntoObstacle(GridConstraints gridConstraints, RobotPosition robotPosition)
    {
        return gridConstraints.ObstaclePositions.Contains(robotPosition.Coordinates);
    }
}
