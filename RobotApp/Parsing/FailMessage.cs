using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing;

public class FailMessage
{
    public string MessageHeader { get; set; }

    public string MessageDetails { get; set; }

    public List<string> CorrectExamples { get; set; }
}
