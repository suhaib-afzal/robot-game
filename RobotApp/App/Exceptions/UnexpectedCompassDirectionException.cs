using System;

namespace RobotApp.App.Exceptions;

public class UnexpectedCompassDirectionException : Exception
{
  public override string Message => "Unexpected Compass Direction";
}
