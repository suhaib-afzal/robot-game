using RobotApp.App.DataTypes;
using static RobotApp.App.DataTypes.RobotPositionFunctions;
using static RobotApp.App.Execution.ExcutionFailFunctions;
using static RobotApp.App.Display.DisplayFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotApp.App.Execution;
using LanguageExt;
using FluentAssertions;

namespace RobotAppTests.App.Execution;

[TestClass]
public class ExecuteTests
{ 

    /*
    +---+---+---+---+
  2 |   | G |   |   |
    +---+---+---+---+
  1 |   | V |   |   |
    +---+---+---+---+
  0 |   |   |   |   |          
    +---+---+---+---+
      0   1   2   3
    */
    [TestMethod]
    public void RunGame_NoObstacles_SimpleScenarios_Works()
    {
        //Arrange
        var goalPos = new RobotPosition((1,2), CompassDirection.North);
        var startPos = new RobotPosition((1,1), CompassDirection.South);
        var gameSpec = 
            new GameSpecification(
                new GridConstraints(
                        (4,3),
                        new()
                    ),
                new List<Journey>()
                {
                    //Wont Reach Goal 1 0 S
                    new Journey(goalPos, startPos,
                        new()
                        {
                            MoveRobotForward,
                        }
                     ),
                    //Reaches Goal
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotLeft,
                            TurnRobotLeft,
                            MoveRobotForward
                        }
                     )
                }
            );

        //Act
        var result = Execute.RunGame(gameSpec);

        //Assert
        var expected = new List<Either<ExecutionFail, ExecutionResult>>()
        {
            new ExecutionResult(new RobotPosition((1,0), CompassDirection.South), false),
            new ExecutionResult(goalPos, true),
        };

        ShouldBeEqiuvalent(result, expected);
    }

    /*
    +---+---+---+---+
  2 | G |   |   |   |
    +---+---+---+---+
  1 |   | V |   |   |
    +---+---+---+---+
  0 |   |   |   |   |          
    +---+---+---+---+
      0   1   2   3
    */
    [TestMethod]
    public void RunGame_NoObstacles_Works()
    {
        var goalPos = new RobotPosition((0, 2), CompassDirection.North);
        var startPos = new RobotPosition((1, 1), CompassDirection.South);
        var gameSpec =
            new GameSpecification(
                new GridConstraints(
                        (4, 3),
                        new()
                    ),
                new List<Journey>()
                {
                    //Wont Reach Goal 2 1 N
                    new Journey(goalPos, startPos,
                        new()
                        {
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward
                        }
                     ),
                    //Reaches Goal
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotRight,
                            TurnRobotRight,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotRight
                        }
                     ),
                    //Wont reach Goal - reaches then turns
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotRight,
                            TurnRobotRight,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotRight,
                            TurnRobotRight
                        }
                     ),
                    //Wont Reach Goal - reaches with wrong orientation
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotRight,
                            TurnRobotRight,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward
                        }
                     ),
                    //Goes out of bounds 3 0
                    new Journey(goalPos,startPos,
                        new()
                        {
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward,
                            MoveRobotForward,
                            MoveRobotForward, //Out of bounds
                            MoveRobotForward
                        }
                     ),
                    //Goes out of bounds 3 0
                    //comes back into bounds, make sure this does
                    //not affect result
                    new Journey(goalPos,startPos,
                        new()
                        {
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward,
                            MoveRobotForward,
                            MoveRobotForward, //Out of bounds
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward
                        }
                     )
                }
            );

        //Act
        var result = Execute.RunGame(gameSpec);

        //Assert
        var expected = new List<Either<ExecutionFail, ExecutionResult>>()
        {
            new ExecutionResult(new RobotPosition((2,1), CompassDirection.North), false),
            new ExecutionResult(goalPos, true),
            new ExecutionResult(new RobotPosition((0, 2), CompassDirection.East), false),
            new ExecutionResult(new RobotPosition((0, 2), CompassDirection.West), false),
            OutOfBounds(),
            OutOfBounds()
        };

        ShouldBeEqiuvalent(result, expected);
    }

    /*
    +---+---+---+---+
  2 |   | O |   |   |
    +---+---+---+---+
  1 | O | V | G |   |
    +---+---+---+---+
  0 |   | O |   |   |          
    +---+---+---+---+
      0   1   2   3
    */
    [TestMethod]
    public void RunGame_WithObstacles_Works()
    {
        var goalPos = new RobotPosition((2, 1), CompassDirection.East);
        var startPos = new RobotPosition((1, 1), CompassDirection.South);
        var gameSpec =
            new GameSpecification(
                new GridConstraints(
                        (4, 3),
                        new()
                        {
                            (1,0),
                            (0,1),
                            (1,2)
                        }
                    ),
                new List<Journey>()
                {
                    //Crashes 1 0
                    new Journey(goalPos, startPos,
                        new()
                        {
                            MoveRobotForward //Crash
                        }
                     ),
                    //Crashes 0 1, Ensure having later instructions
                    //wont prevent crash at second instruction
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotRight,
                            MoveRobotForward, //Crash
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward
                        }
                     ),
                    //Crashes 1 2, Goes through goal
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward //Crashes
                        }
                     ),
                    //Crashes 1 2 then goes out of bounds
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward,
                            TurnRobotLeft,
                            MoveRobotForward, //Crashes
                            MoveRobotForward,
                            MoveRobotForward  //Out of bounds
                        }
                     ),
                    //Reaches goal
                    new Journey(goalPos,startPos,
                        new()
                        {
                            TurnRobotLeft,
                            MoveRobotForward,
                        }
                     )
                }
            );

        //Act
        var result = Execute.RunGame(gameSpec);

        //Assert
        var expected = new List<Either<ExecutionFail, ExecutionResult>>()
        {
            Crashed((1,0)),
            Crashed((0,1)),
            Crashed((1,2)),
            Crashed((1,2)),
            new ExecutionResult(goalPos, true),
        };

        ShouldBeEqiuvalent(result, expected);
    }


    /*
    +---+---+---+---+
  2 | G3| O |   | G2|
    +---+---+---+---+
  1 | G1| V1|   | O |
    +---+---+---+---+
  0 | O |   | Λ3| >2|          
    +---+---+---+---+
      0   1   2   3
    */
    [TestMethod]
    public void RunGame_WithObstacles_DifferentStartsAndGoals_Works()
    {
        var gameSpec =
            new GameSpecification(
                new GridConstraints(
                        (4, 3),
                        new()
                        {
                            (1,0),
                            (0,1),
                            (1,2)
                        }
                    ),
                new List<Journey>()
                {
                    //Crashes 0 0
                    new Journey(
                        new RobotPosition((0,1), CompassDirection.East),
                        new RobotPosition((1,1), CompassDirection.South),
                        new()
                        {
                            MoveRobotForward,
                            TurnRobotRight,
                            MoveRobotForward,
                            MoveRobotForward
                        }
                     ),
                    //Fails to reach goal
                    new Journey(
                        new RobotPosition((3,2), CompassDirection.East),
                        new RobotPosition((3,0), CompassDirection.East),
                        new()
                        {
                            TurnRobotRight,
                            TurnRobotRight,
                            MoveRobotForward,
                            TurnRobotRight,
                            MoveRobotForward,
                            MoveRobotForward,
                            TurnRobotRight,
                        }
                     ),
                    //Out of bounds
                    new Journey(
                        new RobotPosition((0,2), CompassDirection.North),
                        new RobotPosition((2,0), CompassDirection.North),
                        new()
                        {
                            MoveRobotForward,
                            MoveRobotForward,
                            MoveRobotForward,
                            MoveRobotForward,
                            MoveRobotForward
                        }
                     )
                }
            );

        //Act
        var result = Execute.RunGame(gameSpec);

        //Assert
        var expected = new List<Either<ExecutionFail, ExecutionResult>>()
        {
            Crashed((1,0)),
            new ExecutionResult(new RobotPosition((2,2), CompassDirection.East), false),
            OutOfBounds()
        };
        ShouldBeEqiuvalent(result, expected);
    }

    private void ShouldBeEqiuvalent(List<Either<ExecutionFail, ExecutionResult>> actual,
        List<Either<ExecutionFail, ExecutionResult>> expected)
    {
        actual.Count.Should().Be(expected.Count);
        var zipped = actual.Zip(expected).ToList();
        zipped.ForEach(tup => Eq(tup.Item1, tup.Item2));
    }

    private void Eq(Either<ExecutionFail, ExecutionResult> actual, Either<ExecutionFail, ExecutionResult> expected)
    {
        var because = $"actual: {actual.Display()} expected: {expected.Display()}";
        actual.Match(
            Left: f => expected.Match(
                    Left: f2 => f.Should().BeEquivalentTo(f),
                    Right: res => throw new AssertFailedException(because)
                ),
            Right: res => expected.Match(
                    Left: f => throw new AssertFailedException(because),
                    Right: res2 => res.Should().BeEquivalentTo(res2)
                )
        );
    }
}
