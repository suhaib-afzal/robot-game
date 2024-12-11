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

        public Option<int> ProblemPosition { get; set; } 

        public TokenizeFail(string message, string line)
        {
            Message = message;
            Line = line;
            ProblemPosition = Option<int>.None;
        }

        public TokenizeFail(string message, string line, Option<int> problemPosition)
        {
            Message = message;
            Line = line;
            ProblemPosition = problemPosition;
        }
    }
}
