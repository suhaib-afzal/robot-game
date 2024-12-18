namespace RobotApp.App.Execution;

//TODO: Dont like risking Coords to be accessed from OutOfBounds
//      instance, dont want additonal red tape of wrapping in an Option
//      when we access from Crashed instance, look into this
public class ExecutionFail
{
    public ExecutionFail()
    {
        ExecutionFailReason = ExecutionFailReason.OutOfBounds;
    }

    public ExecutionFail((int,int) coords)
    {
        Coords = coords;
        ExecutionFailReason = ExecutionFailReason.Crashed;
    }

    public ExecutionFailReason ExecutionFailReason { get; }

    public (int,int) Coords { get; }

}

public enum ExecutionFailReason
{
    Crashed,
    OutOfBounds
}

public static class ExcutionFailFunctions
{
    public static ExecutionFail Crashed((int, int) coords)
    {
        return new ExecutionFail(coords);
    }

    public static ExecutionFail OutOfBounds()
    {
        return new ExecutionFail();
    }
}

