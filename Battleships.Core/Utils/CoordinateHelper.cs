using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Models;

namespace Battleships.Core.Utils
{
    public static class CoordinateHelper
    { 
        public static List<Coordinate> GetAllCoordinates()
        {
            var freeCoordinates = new List<Coordinate>();
            
            for (var row = 0; row < Consts.Rows; row++)
            {
                for (var column = 0; column < Consts.Columns; column++)
                {
                    freeCoordinates.Add(new Coordinate(row,column));
                }
            }
            return freeCoordinates;
        }

        public static List<Coordinate> GetSurroundingCoordinates(List<Coordinate> coordinates)
        {
            var minRow = coordinates.Min(coordinate => coordinate.Row);
            var maxRow = coordinates.Max(coordinate => coordinate.Row);
            
            var minColumn = coordinates.Min(coordinate => coordinate.Column);
            var maxColumn = coordinates.Max(coordinate => coordinate.Column);

            var surroundingCoordinates = new List<Coordinate>();
            for (var row = minRow - 1; row <= maxRow + 1; ++row)
            {
                for (var column = minColumn - 1; column <= maxColumn + 1; ++column)
                {
                    if (row < 0 || row >= Consts.Rows)
                    {
                        break;
                    }

                    if (column < 0 || column >= Consts.Columns)
                    {
                        continue;
                    }
                    
                    surroundingCoordinates.Add(new Coordinate(row, column));
                }
            }

            return surroundingCoordinates.Except(coordinates).ToList();
        }
    }
}