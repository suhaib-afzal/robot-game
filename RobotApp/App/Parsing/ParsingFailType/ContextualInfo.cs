using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Parsing.ParsingFailType;

public class ContextualInfo
{
    public ContextualInfo(string info, List<string> examples)
    {
        Info = info;
        Examples = examples;
    }

    public string Info { get; set; }

    public List<string> Examples { get; set; }
}
