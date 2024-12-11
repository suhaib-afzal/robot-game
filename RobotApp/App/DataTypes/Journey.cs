using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.DataTypes
{
    public class Journey
    {
        public RobotPosition StartPosition { get; set; }

        public RobotPosition GoalPosition { get; set; }

        public List<Instruction> Instructions { get; set; }
    }
}
