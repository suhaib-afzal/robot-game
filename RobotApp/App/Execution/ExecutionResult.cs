using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Execution;

public class ExecutionResult
{
    public RobotPosition RobotPosition { get; set; }

    public bool IsSuccess { get; set; }

    public override string ToString()
    {
        var successStr = IsSuccess ? "SUCCESS" : "FAILURE";
        return $"{successStr} {RobotPosition}";
    }
}
