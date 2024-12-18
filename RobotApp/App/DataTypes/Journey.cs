using System.Collections.Generic;

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

        public RobotPosition StartPosition { get; }

        public RobotPosition GoalPosition { get; }

        public List<Instruction> Instructions { get; }
    }
}
