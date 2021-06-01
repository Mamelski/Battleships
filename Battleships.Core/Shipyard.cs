using System.Collections.Generic;
using Battleships.Core.Models.Ships;

namespace Battleships.Core
{
    public static class Shipyard
    {
        public static List<UnplacedShip> ProduceShips()
        {
            return new ()
            {
                new UnplacedShip(ShipClass.Battleship),
                new UnplacedShip(ShipClass.Destroyer),
                new UnplacedShip(ShipClass.Destroyer)
            };
        }
    }
}