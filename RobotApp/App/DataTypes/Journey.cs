using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.DataTypes
{
    public class Journey
    {
        public Journey(RobotPosition goalPosition, RobotPosition startPosition, List<Instruction> instructions)
        {
            StartPosition = startPosition;
            GoalPosition = goalPosition;
            Instructions = instructions;
        }

        public RobotPosition StartPosition { get; set; }

        public RobotPosition GoalPosition { get; set; }

        public List<Instruction> Instructions { get; set; }
    }
}
