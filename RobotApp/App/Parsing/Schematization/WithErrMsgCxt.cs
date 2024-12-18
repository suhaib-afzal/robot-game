namespace RobotApp.App.Parsing.Schematization;

internal class WithErrMsgCxt<T>
{
    public WithErrMsgCxt(ErrMsgContext context, T unwrap)
    {
        Context = context;
        Unwrap = unwrap;
    }

    public ErrMsgContext Context { get; }

    public T Unwrap { get; }
}

internal static class WithErrMsgCxtFunctions
{
    public static WithErrMsgCxt<T> WithContext<T>(this T value, ErrMsgContext context)
    {
        return new WithErrMsgCxt<T>(context, value);
    }
} 
