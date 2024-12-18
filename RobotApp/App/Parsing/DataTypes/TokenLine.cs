using System.Collections.Generic;

namespace RobotApp.App.Parsing.DataTypes;

public class TokenLine
{
    public TokenLine(List<Token> tokens, TextLine textLine)
    {
        Tokens = tokens;
        TextLine = textLine;
    }

    public List<Token> Tokens { get; }

    public TextLine TextLine { get; }
}
