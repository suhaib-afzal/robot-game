using LanguageExt;
using RobotApp.App.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.Parsing.ParsingFailType;

//TODO: stop using isChunkLevel to determine if the fail is Chunk or Line level
//      look into Haskell-style constructors so we have same type with diff data
public class ParsingFail
{
    //Both Levels
    public string MessageHeader { get; set; }

    public List<ContextualInfo> ContextualInfoList { get; set; }

    public bool isChunkLevel { get; set; }

    //Chunk Level Error
    public List<TextLine> ProblemLines { get; set; }

    //Line level Error
    public TextLine ProblemLine { get; set; }

    public Option<(int, int)> ProblemSpan { get; set; }


    //Line level constructor
    public ParsingFail(
        string messageHeader,
        List<ContextualInfo> contextualInfoList,
        TextLine problemLine,
        Option<(int, int)> problemSpan)
    {
        ProblemLine = problemLine;
        MessageHeader = messageHeader;
        ContextualInfoList = contextualInfoList;
        isChunkLevel = false;
        ProblemSpan = problemSpan;
    }

    //Chunk level constructor
    public ParsingFail(
        string messageHeader,
        List<ContextualInfo> contextualInfoList,
        List<TextLine> problemLines)
    {
        ProblemLines = problemLines;
        MessageHeader = messageHeader;
        ContextualInfoList = contextualInfoList;
        isChunkLevel = true;
    }
}
