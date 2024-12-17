using LanguageExt;
using RobotApp.App.DataTypes;
using RobotApp.App.Execution;
using RobotApp.Parsing;
using RobotApp.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        return String.Concat(
                (
                parsingFail.isChunkLevel ?
                    DisplayList(parsingFail.ProblemLines, Display, false)
                    .Append(Environment.NewLine)
                    :
                    Display(parsingFail.ProblemLine)
                    .Append(Environment.NewLine)
                    .Append(GetRangeMarkers(parsingFail))
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine)
                )
                .Append(parsingFail.MessageHeader)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(parsingFail.MessageDetails)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(DisplayList(parsingFail.CorrectExamples, x => x, true))
            );
    }

    public static string DisplayList<T>(this List<T> list, Func<T, string> displayT, bool useEmptyLine)
    {
        var fdkv = list.Map(item => string.Concat(displayT(item).Append(Environment.NewLine)))
            .Intersperse(useEmptyLine? Environment.NewLine : "")
            .ToList()
            .Concat();
            return fdkv;
    }

    public static string GetRangeMarkers(ParsingFail parseFail)
    {
        return parseFail.ProblemSpan.Match(
                Some: range => Enumerable.Repeat(" ", range.Item1 + 2 + parseFail.ProblemLine.LineNumber.ToString().Length)
                           .Concat(Enumerable.Repeat("^", range.Item2 - range.Item1))
                           .Concat(),
                None: ""
            );
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
        return $"{successStr} {executionResult.RobotPosition}";
    }


    public static string Display(this ExecutionFail executionFail)
    {
        return executionFail.Message;
    }
}
