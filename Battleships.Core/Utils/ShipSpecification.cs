using System;
using Battleships.Core.Models.Ships;

namespace Battleships.Core.Utils
{
    public static class ShipSpecification
    {
        public static int GetShipClassSize(ShipClass shipClass)
        {
            return shipClass switch
            {
                ShipClass.Battleship => 5,
                ShipClass.Destroyer => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(shipClass), shipClass, "Not supported ship class")
            };
        }
    }
}