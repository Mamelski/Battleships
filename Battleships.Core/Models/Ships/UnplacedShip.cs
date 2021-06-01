using Battleships.Core.Utils;

namespace Battleships.Core.Models.Ships
{
    public record UnplacedShip
    {
        public ShipClass ShipClass { get; }
        public int Size { get; }

        public UnplacedShip(ShipClass shipClass)
        {
            ShipClass = shipClass;
            Size = ShipSpecification.GetShipClassSize(shipClass);
        }
    }
}