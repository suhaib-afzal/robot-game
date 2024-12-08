using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.DataTypes;

public class GridConstraints
{
    public (int, int) GridDimensions { get; set; }

    public List<(int, int)> ObstaclePositions { get; set; }
}

public static class GridConstraintsFunctions
{
    public static bool withinBounds(GridConstraints gridConstraints, RobotPosition robotPosition)
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

    public static bool hasCrashedIntoObstacle(GridConstraints gridConstraints, RobotPosition robotPosition)
    {
        return gridConstraints.ObstaclePositions.Contains(robotPosition.Coordinates);
    }
}
