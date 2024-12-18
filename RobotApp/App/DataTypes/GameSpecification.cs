using System.Collections.Generic;

namespace RobotApp.App.DataTypes
{
    public class GameSpecification
    {
        public GameSpecification(GridConstraints constraints, List<Journey> journeys)
        {
            GridConstraints = constraints;
            Journeys = journeys;
        }

        public GridConstraints GridConstraints { get; }

        public List<Journey> Journeys { get; }
    }
}
