namespace RobotApp.App.Parsing.Tokenization
{
    internal class WithStringPointerState<T>
    {
        public WithStringPointerState(T value, StringWithPointer stringWithPointer)
        {
            Value = value;
            StringWithPointer = stringWithPointer;
        }

        public T Value { get; }

        public StringWithPointer StringWithPointer { get; }
    }
}
