using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.DataTypes;

public class Token
{
    public Token(string value, TokenType tokenType)
    {
        Value = value;
        TokenType = tokenType;
    }

    public string Value;

    public TokenType TokenType;
}

public enum TokenType
{
    Word,
    Number,
    StandaloneLetter,
}
