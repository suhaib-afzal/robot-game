using System.Collections.Generic;

namespace RobotApp.App.Parsing.ParsingFailType;

public class ContextualInfo
{
    public ContextualInfo(string info, List<string> examples)
    {
        Info = info;
        Examples = examples;
    }

    public string Info { get; }

    public List<string> Examples { get; }
}
