using System.Collections.Generic;
using Battleships.Core.Models.Ships;

namespace Battleships.Core.ShipPlacement
{
    public interface IShipPlacementStrategy
    {
        void AssignCoordinatesToShips(List<Ship> ships);
    }
}