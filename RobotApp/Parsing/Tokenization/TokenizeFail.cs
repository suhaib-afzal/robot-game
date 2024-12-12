using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Tokenization
{
    public class TokenizeFail
    {
        public string Message { get; set; }

        public string Line { get; set; }

        public int LineNumber { get; set; }

        public Option<int> ProblemPosition { get; set; } 

        public TokenizeFail(string message, string line, int lineNumber)
        {
            Message = message;
            Line = line;
            LineNumber = lineNumber;
            ProblemPosition = Option<int>.None;
        }

        public TokenizeFail(string message, string line, int lineNumber, Option<int> problemPosition)
        {
            Message = message;
            Line = line;
            LineNumber = lineNumber;
            ProblemPosition = problemPosition;
        }
    }
}
