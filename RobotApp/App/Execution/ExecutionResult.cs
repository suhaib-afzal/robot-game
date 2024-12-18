using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotApp.App.DataTypes;

namespace RobotApp.App.Execution;

public class ExecutionResult
{
    public ExecutionResult(RobotPosition robotPosition, bool isSuccess)
    {
        RobotPosition = robotPosition;
        IsSuccess = isSuccess;
    }

    public RobotPosition RobotPosition { get; set; }

    public bool IsSuccess { get; set; }

}
