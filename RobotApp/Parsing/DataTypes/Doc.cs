using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Parsing.DataTypes;

public class Doc<T>
{
    public Doc(List<Chunk<T>> chunks)
    {
        Chunks = chunks;
    }

    public List<Chunk<T>> Chunks { get; set; }

}

public class Chunk<T>
{
    public List<T> values;
}
