using System.Collections.Generic;
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
    }
}