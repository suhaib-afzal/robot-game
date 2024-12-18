using LanguageExt;
using RobotApp.App.DataTypes;
using RobotApp.App.Execution;
using RobotApp.App.Parsing.DataTypes;
using RobotApp.App.Parsing.ParsingFailType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotApp.App.DataTypes.CompassDirectionFunctions;

namespace RobotApp.App.Display;

public static class DisplayFunctions
{
    public static (string, ConsoleColor) Display(this Either<ParsingFail, List<Either<ExecutionFail, ExecutionResult>>> result)
    {
        return result.Match(
                Left: p => (p.Display(), ConsoleColor.Red),
                Right: listEith => (DisplayList(listEith, Display, false), ConsoleColor.White)
              );
    }

    public static string Display(this ParsingFail parsingFail)
    {
        return string.Concat(
                (
                parsingFail.isChunkLevel ?
                    DisplayList(parsingFail.ProblemLines, Display, false)
                    .Append(Environment.NewLine)
                    :
                    DisplayLineWithRangeMarkers(parsingFail.ProblemLine, parsingFail.ProblemSpan)
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine)
                )
                .Append(parsingFail.MessageHeader)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(DisplayList(parsingFail.ContextualInfoList, Display, true))
            );
    }

    public static string DisplayList<T>(List<T> list, Func<T, string> displayT, bool useEmptyLine)
    {
        var fdkv = list.Map(item => string.Concat(displayT(item).Append(Environment.NewLine)))
            .Intersperse(useEmptyLine? Environment.NewLine : "")
            .ToList()
            .Concat();
            return fdkv;
    }

    public static string Display(this ContextualInfo contextualInfo)
    {
        return string.Concat(contextualInfo.Info
            .Append(Environment.NewLine)
            .Append(Environment.NewLine)
            .Append(DisplayList(contextualInfo.Examples, x => x, true)));
    }

    public static string DisplayLineWithRangeMarkers(TextLine problemLine, Option<(int,int)> problemSpan)
    {
        return string.Concat(
                     Display(problemLine)
                    .Append(Environment.NewLine)
                    .Append(
                        problemSpan.Match(              
                            Some: range => Enumerable.Repeat(" ",
                            //Length of line number + 2 spaces + spaces till start of range                             
                             problemLine.LineNumber.ToString().Length + 2 + range.Item1)
                                .Concat(Enumerable.Repeat("^", range.Item2 - range.Item1))
                                .Concat(),
                            None: ""
                    )));
    }

    public static string Display(TextLine textLine)
    {
        return $"{textLine.LineNumber}  {textLine.Text}";
    }

    public static string Display(this Either<ExecutionFail, ExecutionResult> either)
    {
        return either.Match(
                    Left: fail => fail.Display(),
                    Right: res => res.Display()
                );
    }


    public static string Display(this ExecutionResult executionResult)
    {
        var successStr = executionResult.IsSuccess ? "SUCCESS" : "FAILURE";
        var robotPosition = executionResult.RobotPosition;
        return $"{successStr} {robotPosition.Coordinates.Item1} {robotPosition.Coordinates.Item2} {ToChar(robotPosition.FacingDirection)}";
    }


    public static string Display(this ExecutionFail executionFail)
    {
        return executionFail.ExecutionFailReason switch
        {
            ExecutionFailReason.OutOfBounds => "OUT OF BOUNDS",
            ExecutionFailReason.Crashed => $"CRASHED {executionFail.Coords.Item1} {executionFail.Coords.Item2}"
        };
    }
}
