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

        public TokenizeFail(string message)
        {
            Message = message;
        }
    }
}
