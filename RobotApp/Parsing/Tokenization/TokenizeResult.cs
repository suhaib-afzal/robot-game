using RobotApp.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Tokenization
{
    public class TokenizeResult
    {
        public StringWithPointer UpdatedStringWPointer { get; set; }

        public Token Token { get; set; }
    }
}
