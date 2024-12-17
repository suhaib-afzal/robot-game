using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Execution;

public class ExecutionFail
{
    public ExecutionFail(string message)
    {
        Message = message;
    }

    public string Message { get; }

}
