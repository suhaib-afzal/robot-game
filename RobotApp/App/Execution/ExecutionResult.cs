using RobotApp.App.DataTypes;

namespace RobotApp.App.Execution;

public class ExecutionResult
{
    public ExecutionResult(RobotPosition robotPosition, bool isSuccess)
    {
        RobotPosition = robotPosition;
        IsSuccess = isSuccess;
    }

    public RobotPosition RobotPosition { get; }

    public bool IsSuccess { get; }

}
