using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.GridConstraintsFunctions;
using static RobotApp.RobotPositionFunctions;

namespace RobotApp.App.Execution;

public static class Execute
{
    public static Either<ExecutionError, ExecutionResult> runInstructions(
                                                          GridConstraints gridConstraints,
                                                          RobotPosition goalPosition,
                                                          RobotPosition position,
                                                          List<Instruction> instructions)
    {
        if (instructions.Count == 0)
        {
            if (goalPosition.Equals(position))
            {
                return new ExecutionResult() { IsSuccess = true, RobotPosition = position };
            }

            return new ExecutionResult() { IsSuccess = false, RobotPosition = position };
        }

        var instruction = instructions.First();
        var restInstructions = instructions.Skip(1).ToList();

        var nextPosition = instruction.Invoke(position);

        if (!withinBounds(gridConstraints, nextPosition))
        {
            return new ExecutionError("OUT OF BOUNDS");
        }

        if (hasCrashedIntoObstacle(gridConstraints, nextPosition))
        {
            return new
              ExecutionError($"CRASHED {nextPosition.Coordinates.Item1} {nextPosition.Coordinates.Item2}");
        }

        return runInstructions(gridConstraints, goalPosition, nextPosition, restInstructions);
    }
}
