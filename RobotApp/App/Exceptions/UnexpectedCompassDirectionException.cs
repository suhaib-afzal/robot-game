using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Exceptions;

public class UnexpectedCompassDirectionException : Exception
{
  public override string Message => "Unexpected Compass Direction";
}
