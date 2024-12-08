using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Execution;

public class ExecutionError
{
    public ExecutionError(string message)
    {
        Message = message;
    }

    public string Message { get; }

    public override string ToString()
    {
        return Message;
    }
}
