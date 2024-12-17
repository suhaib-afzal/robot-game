using System;
using System.Collections.Generic;
using System.Linq;
using static RobotApp.App.DataTypes.GridConstraintsFunctions;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using static RobotApp.Parsing.Tokenization.TokenizerFunctions;
using static RobotApp.Parsing.Analysis.SchematizerFunctions;
using LanguageExt;
using LanguageExt.Common;
using RobotApp.App.Execution;
using RobotApp.App.DataTypes;
using System.IO;
using System.Text;
using RobotApp.Parsing.Tokenization;
using RobotApp.App.Display;

namespace RobotApp;


class Program
{
    static void Main(string[] args)
    {
        string readContents;
        using (var streamReader = new StreamReader("C:\\git\\suhaib-robot-dev-test\\RobotApp\\InputFiles\\EndToEnd5.txt", Encoding.UTF8))
        {
            readContents = streamReader.ReadToEnd().ReplaceLineEndings();
        }

        var result = (from tokenizedDoc in TokenizeDocument(readContents)
                      from gameSpec in Schematizer(tokenizedDoc)
                      select gameSpec)
                     .Map(gameSpec => Execute.RunGame(gameSpec))
                     .Display();

        Console.ForegroundColor = result.Item2;    
        Console.WriteLine(result.Item1);
    }
}


/*
        var gridConstraints = new GridConstraints((5, 5), new List<(int, int)>() { (2, 0) });
        var goalPos = new RobotPosition((4, 4), CompassDirection.North);
        var startPos = new RobotPosition((0, 0), CompassDirection.South);
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
        var gameSpec = new GameSpecification
        (
            gridConstraints, 
            new List<Journey> 
            { 
                new Journey(goalPos,startPos, instructions)
            } 
        );
        */