using System;
using System.Collections.Generic;
using System.Linq;
using static RobotApp.App.DataTypes.GridConstraintsFunctions;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using static RobotApp.App.Parsing.Tokenization.TokenizerFunctions;
using static RobotApp.App.Parsing.Schematization.SchematizerFunctions;
using LanguageExt;
using LanguageExt.Common;
using RobotApp.App.Execution;
using RobotApp.App.DataTypes;
using System.IO;
using System.Text;
using RobotApp.App.Parsing.Tokenization;
using RobotApp.App.Display;

namespace RobotApp;


class Program
{
    static void Main(string[] args)
    {
        var filePath = args.Length == 0 ?
            $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\InputFiles\\EndToEndTests\\EndToEnd8.txt" :
            args[0];

        string readContents;
        using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
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
