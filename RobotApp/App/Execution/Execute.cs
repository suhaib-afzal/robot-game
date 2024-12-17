using LanguageExt;
using RobotApp.App.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.App.DataTypes.GridConstraintsFunctions;

namespace RobotApp.App.Execution;

public static class Execute
{

    public static List<Either<ExecutionFail, ExecutionResult>> RunGame(GameSpecification gameSpecification)
    {
        return gameSpecification.Journeys.Map(journey => RunInstructions(gameSpecification.GridConstraints,
                                                                         journey.GoalPosition,
                                                                         journey.StartPosition,
                                                                         journey.Instructions))
                                                                        .ToList();
    }

    private static Either<ExecutionFail, ExecutionResult> RunInstructions(
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
            return new ExecutionFail("OUT OF BOUNDS");
        }

        if (hasCrashedIntoObstacle(gridConstraints, nextPosition))
        {
            return new
              ExecutionFail($"CRASHED {nextPosition.Coordinates.Item1} {nextPosition.Coordinates.Item2}");
        }

        return RunInstructions(gridConstraints, goalPosition, nextPosition, restInstructions);
    }
}
