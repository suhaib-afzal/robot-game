using System;
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
      turnRobotLeft,
      turnRobotLeft,
      moveRobotForward
    };
    var gameSpec = new GameSpecification() 
    { 
        GridConstraints = gridConstraints, 
        Journeys = new List<Journey> 
        { 
            new Journey() 
            { 
                GoalPosition = goalPos, 
                Instructions = instructions, 
                StartPosition = startPos 
            } 
        } 
    };

    var result = Execute.runGame(gameSpec);

    var resultString = result
        .ConvertAll(
            e => e.Right(x => x.ToString()).Left(y => y.ToString())
        )
        .Concat();
    Console.WriteLine(resultString);
  }
}
