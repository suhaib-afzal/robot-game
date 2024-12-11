﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.App.DataTypes
{
    public class GameSpecification
    {
        public GridConstraints GridConstraints { get; set; }

        public List<Journey> Journeys { get; set; }
    }
}