using System.Collections.Generic;

namespace Battleships.Core.Models.Ships
{
    public record PlacedShip : UnplacedShip
    {
        public List<Coordinate> Coordinates { get; }

        public PlacedShip(
            ShipType shipType,
            List<Coordinate> coordinates)
            : base(shipType)
        {
            Coordinates = coordinates;
        }
    }
}