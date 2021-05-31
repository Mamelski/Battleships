using System.Collections.Generic;
using System.Linq;

namespace Battleships.Core.Models.Ships
{
    public record FightingShip : PlacedShip
    {
        public bool IsSunk  => !NotHitCoordinates.Any();
        public List<Coordinate> NotHitCoordinates { get; } 
        
        public FightingShip(
            ShipType shipType,
            List<Coordinate> coordinates)
            : base(shipType, coordinates)
        {
            NotHitCoordinates = coordinates.Select(c => c).ToList();
        }
    }
}