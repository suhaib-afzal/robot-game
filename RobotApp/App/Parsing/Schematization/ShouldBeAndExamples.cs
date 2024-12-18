using System.Collections.Generic;

namespace RobotApp.App.Parsing.Schematization;

internal static class ShouldBeAndExamples
{
    public static string GridDefinitionShouldBe = @"Grid Definition should be GRID followed by NumxNum, followed by an empty line";

    public static List<string> GridDefinitionExamples = new List<string>()
    {
        "GRID 4x5",
        "GRID 10x8",
        "GRID 209x80"
    };

    public static string ObstaclesDefinitionShouldBe = @"Obstacles Definition should be multiple lines of OBSTACLE followed by Num Num, the section to end with an empty line";

    public static List<string> ObstaclesDefinitionExamples = new List<string>()
    {
        @"OBSTACLE 1 100
OBSTACLE 7 99
OBSTACLE 8 88",

        @"OBSTACLE 6 7",

        @"OBSTACLE 9 0
OBSTACLE 0 2
OBSTACLE 1000 10000
OBSTACLE 880 73",
    };

    public static string RobotPositionDefinitionShouldBe = @"Robot Position should be defined as 1 line with Num Num then a Compass Direction as a single letter";

    public static List<string> RobotPositionDefinitionExamples = new List<string>()
    {
        "1 1 N",
        "90 28 E",
        "89 50 W",
    };

    public static string InstructionDefinitionShouldBe = @"The Instructions should be defined on 1 line as a sequence of Fs Rs and Ls";

    public static List<string> InstructionDefinitionExamples = new List<string>()
    {
        "RRRRRRRRRR",
        "RLLLRFF",
        "FFFLLR",
    };

    public static string JourneyDefinitionShouldBe
        = @$"Journey Defintion should be 3 lines, with the start robot position, then the instructions, then the goal robot position, the section should end with an empty line
{RobotPositionDefinitionShouldBe}
{InstructionDefinitionShouldBe}";

    public static List<string> JourneyDefinitionExamples = new List<string>()
    {
        @"89 4 W
LLFF
7 0 E",

        @"7 0 N
RRRL
0 0 E",

        @"40 40 N
FFFFFFFFFF
39 39 N",
    };
}

