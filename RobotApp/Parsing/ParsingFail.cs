using LanguageExt;
using RobotApp.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing;

//TODO: stop using isChunkLevel to determine if the fail is Chunk or Line level
//      look into Haskell-style constructors so we have same type with diff data
public class ParsingFail
{
    //Both Levels
    public string MessageHeader { get; set; }

    public string MessageDetails { get; set; }
    
    public List<string> CorrectExamples { get; set; }

    public bool isChunkLevel { get; set; }

    //Chunk Level Error
    public List<TextLine> ProblemLines { get; set; }

    //Line level Error
    public TextLine ProblemLine { get; set; }

    public Option<(int,int)> ProblemSpan { get; set; }


    //public override string ToString()
    //{
    //    return ProblemLines
    //        .Map(line => $"{line.LineNumber} {line.Text}{Environment.NewLine}")
    //        .Append(Environment.NewLine)
    //        .Append(MessageHeader)
    //        .Append(Environment.NewLine)
    //        .Append(MessageDetails)
    //        .Append(Environment.NewLine)
    //        .Append(CorrectExamples.Map(line => $"{line}{Environment.NewLine}"))
    //        .Concat();
    //}

    //Line level constructor
    public ParsingFail(
        string messageHeader,
        string messageDetails,
        List<string> correctExamples,
        TextLine problemLine)
    {
        ProblemLine = problemLine;
        MessageHeader = messageHeader;
        MessageDetails = messageDetails;
        CorrectExamples = correctExamples;
        ProblemSpan = Option<(int,int)>.None;
        isChunkLevel = false;
    }

    //Line level constructor
    public ParsingFail(
        string messageHeader,
        string messageDetails,
        List<string> correctExamples,
        TextLine problemLine,
        Option<(int,int)> problemSpan)
    {
        ProblemLine = problemLine;
        MessageHeader = messageHeader;
        MessageDetails = messageDetails;
        CorrectExamples = correctExamples;
        isChunkLevel = false;
        ProblemSpan= problemSpan;
    }

    //Chunk level constructor
    public ParsingFail(
        string messageHeader,
        string messageDetails,
        List<string> correctExamples,
        List<TextLine> problemLines)
    {
        ProblemLines = problemLines;
        MessageHeader = messageHeader;
        MessageDetails = messageDetails;
        CorrectExamples = correctExamples;
        isChunkLevel = true;
    }
}
