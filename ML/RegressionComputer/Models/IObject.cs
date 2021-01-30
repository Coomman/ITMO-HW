using System.Collections.Generic;

namespace RegressionComputer.Models
{
    public interface IObject
    {
        Vector Features { get; }
        Vector Labels { get; }
        int Class { get; }
        Dictionary<DistanceFunc, double[]> Distances { get; set; }
    }
}
