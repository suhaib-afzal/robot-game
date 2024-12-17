using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.DataTypes;

public class Token
{
    public Token(string value, TokenType tokenType, (int, int) positionRange)
    {
        Value = value;
        TokenType = tokenType;
        PositionRange = positionRange;
    }

    public string Value;

    public TokenType TokenType;

    public (int, int) PositionRange;
}

public enum TokenType
{
    Word,
    Number,
    StandaloneLetter,
    NumberxNumber,
    End,
}
