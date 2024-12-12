using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.DataTypes;

public class TokenLine
{
    public TokenLine(List<Token> tokens, int lineNumber)
    {
        Tokens = tokens;
        LineNumber = lineNumber;
    }

    public List<Token> Tokens { get; set; }

    public int LineNumber { get; set; }
}
