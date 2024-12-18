namespace RobotApp.App.Parsing.DataTypes;

public class Token
{
    public Token(string value, TokenType tokenType, (int, int) positionRange)
    {
        Value = value;
        TokenType = tokenType;
        PositionRange = positionRange;
    }

    public string Value { get; }

    public TokenType TokenType { get; }

    public (int, int) PositionRange { get; }
}

public enum TokenType
{
    Word,
    Number,
    StandaloneLetter,
    NumberxNumber,
    End,
}
