using System;

namespace Battleships.Core.Models.Ships
{
    public record UnplacedShip
    {
        public ShipType ShipType { get; }
        public int Size { get; }

        public UnplacedShip(ShipType shipType)
        {
            ShipType = shipType;
            Size = shipType switch
            {
                ShipType.Battleship => 5,
                ShipType.Destroyer => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(shipType), shipType, "Not supported ship type")
            };
        }
    }
}