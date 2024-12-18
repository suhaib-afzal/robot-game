using LanguageExt;
using RobotApp.App.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.App.DataTypes.GridConstraintsFunctions;
using static RobotApp.App.Execution.ExcutionFailFunctions;

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
                return new ExecutionResult(position, true);
            }

            return new ExecutionResult(position, false);
        }

        var instruction = instructions.First();
        var restInstructions = instructions.Skip(1).ToList();

        var nextPosition = instruction.Invoke(position);

        if (!WithinBounds(gridConstraints, nextPosition))
        {
            return OutOfBounds();
        }

        if (HasCrashedIntoObstacle(gridConstraints, nextPosition))
        {
            return Crashed(nextPosition.Coordinates);
        }

        return RunInstructions(gridConstraints, goalPosition, nextPosition, restInstructions);
    }
}
