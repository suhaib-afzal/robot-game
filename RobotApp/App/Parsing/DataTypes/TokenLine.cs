using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Parsing.DataTypes;

public class TokenLine
{
    public TokenLine(List<Token> tokens, TextLine textLine)
    {
        Tokens = tokens;
        TextLine = textLine;
    }

    public List<Token> Tokens { get; set; }

    public TextLine TextLine { get; set; }
}
