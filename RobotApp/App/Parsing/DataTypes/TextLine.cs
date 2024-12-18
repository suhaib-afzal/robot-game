using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Parsing.DataTypes;

public class TextLine
{
    public TextLine(string text, int lineNumber)
    {
        Text = text;
        LineNumber = lineNumber;
    }

    public string Text { get; set; }

    public int LineNumber { get; set; }
}
