using System;
using static RobotApp.App.Parsing.Tokenization.TokenizerFunctions;
using static RobotApp.App.Parsing.Schematization.SchematizerFunctions;
using RobotApp.App.Execution;
using System.IO;
using System.Text;
using RobotApp.App.Display;

namespace RobotApp;


class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please pass in file path");
            Environment.Exit(1);
        }

        var filePath = args[0];

        if(!File.Exists(filePath))
        {
            Console.WriteLine("Please ensure path is to a file and that file exists");
            Environment.Exit(1);
        }

        if(!filePath.EndsWith(".txt"))
        {
            Console.WriteLine("Please ensure path is to a text file");
            Environment.Exit(1);
        }
        
        string readContents;
        using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
        {
            readContents = streamReader.ReadToEnd().ReplaceLineEndings();
        }

        if(readContents.Trim() == "")
        {
            Console.WriteLine("Please enusre the file is populated");
            Environment.Exit(1);
        }
        

        var result = (from tokenizedDoc in TokenizeDocument(readContents)
                      from gameSpec in Schematizer(tokenizedDoc)
                      select gameSpec)
                     .Map(gameSpec => Execute.RunGame(gameSpec))
                     .Display();

        Console.ForegroundColor = result.Item2;    
        Console.WriteLine(result.Item1);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
