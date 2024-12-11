using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.DataTypes;

public class Token
{
    public string Value;

    public TokenType TokenType;
}

public enum TokenType
{
    Word,
    Number,
    StandaloneLetter,
    Space,
    Start,
    End,
}
