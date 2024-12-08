﻿using System;
using System.Collections.Generic;
using System.Linq;
using static RobotApp.App.DataTypes.GridConstraintsFunctions;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using LanguageExt;
using LanguageExt.Common;
using RobotApp.App.Execution;
using RobotApp.App.DataTypes;

namespace RobotApp;


class Program
{
  static void Main(string[] args)
  {
    var gridConstraints = new GridConstraints() 
    { 
      GridDimensions = (5, 5), 
      ObstaclePositions = new List<(int, int)>() { (2,0) } 
    };
    var goalPos = new RobotPosition() { Coordinates = (4,4), FacingDirection = CompassDirection.North };
    var startPos = new RobotPosition() { Coordinates = (0,0), FacingDirection = CompassDirection.South };
    var instructions = new List<Instruction>() 
    {
      turnRobotLeft, 
      moveRobotForward, 
      turnRobotLeft, 
      moveRobotForward, 
      turnRobotRight, 
      moveRobotForward,
      turnRobotLeft,
      moveRobotForward,
      turnRobotRight,
      moveRobotForward,
      turnRobotLeft,
      moveRobotForward,
      turnRobotRight,
      moveRobotForward,
      turnRobotLeft,
      moveRobotForward
    };
    var result = Execute.runInstructions(gridConstraints, goalPos, startPos, instructions);

    var resultString = result.Right(x => x.ToString()).Left(y => y.ToString());
    Console.WriteLine(resultString);
  }
}
