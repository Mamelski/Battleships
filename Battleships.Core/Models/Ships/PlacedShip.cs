using System.Collections.Generic;

namespace Battleships.Core.Models.Ships
{
    public record PlacedShip : UnplacedShip
    {
        public List<Coordinate> Coordinates { get; }

        public PlacedShip(
            ShipClass shipClass,
            List<Coordinate> coordinates)
            : base(shipClass)
        {
            Coordinates = coordinates;
        }
    }
}