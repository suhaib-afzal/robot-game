namespace RobotApp.App.Parsing.DataTypes;

public class TextLine
{
    public TextLine(string text, int lineNumber)
    {
        Text = text;
        LineNumber = lineNumber;
    }

    public string Text { get; }

    public int LineNumber { get; }
}
