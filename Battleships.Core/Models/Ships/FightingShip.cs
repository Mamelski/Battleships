using System.Collections.Generic;
using System.Linq;

namespace Battleships.Core.Models.Ships
{
    public record FightingShip : PlacedShip
    {
        public bool IsSunk => !NotHitCoordinates.Any();
        public List<Coordinate> NotHitCoordinates { get; } 
        
        public FightingShip(
            ShipClass shipClass,
            List<Coordinate> coordinates)
            : base(shipClass, coordinates)
        {
            NotHitCoordinates = coordinates.Select(c => c).ToList();
        }
    }
}