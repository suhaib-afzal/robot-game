using RobotApp.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.Tokenization
{
    public class WithStringPointerState<T>
    {
        public WithStringPointerState(T value, StringWithPointer stringWithPointer)
        {
            Value = value;
            StringWithPointer = stringWithPointer;
        }

        public T Value { get; set; }

        public StringWithPointer StringWithPointer { get; set; }
    }
}
