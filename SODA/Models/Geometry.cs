using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    public abstract class Geometry<T>
    {
        protected string coordinatesToWKT(double[] coordinates)
        {
            var format = "{0}";
            if (typeof(T) == typeof(Point) || typeof(T) == typeof(MultiPoint))
                format = "({0})";
            return string.Format(format, string.Join(" ", coordinates.Select(coordinate => coordinate.ToString())));
        }
        protected string coordinatesToWKT(double[][] coordinates)
        {
            return string.Format("({0})", string.Join(", ", coordinates.Select(coordinate => coordinatesToWKT(coordinate))));
        }
        protected string coordinatesToWKT(double[][][] coordinates)
        {
            return string.Format("({0})", string.Join(", ", coordinates.Select(coordinate => coordinatesToWKT(coordinate))));
        }
        protected string coordinatesToWKT(double[][][][] coordinates)
        {
            return string.Format("({0})", string.Join(", ", coordinates.Select(coordinate =>  coordinatesToWKT(coordinate))));
        }
        protected string toWKT(string type, string coordinates)
        {
            return string.Format("{0} {1}", type.ToUpper(), coordinates);
        }
    }
}
