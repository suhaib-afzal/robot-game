using RobotApp.App.Parsing.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotAppTests.App.Parsing.Analysis;

public class SchematizerFunctionsTests
{
    [TestMethod]
    public void InvalidTokens_Fails()
    {
        var fail = new Doc<TokenLine>(
            new List<Chunk<TokenLine>>
            {
                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {

                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {

                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {

                    }
                ),

                new Chunk<TokenLine>(
                    new List<TokenLine>()
                    {

                    }
                ),
            }
        );
    }
}
