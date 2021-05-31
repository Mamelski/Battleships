using System.Collections.Generic;

namespace Battleships.Core.Models.Ships
{
    public record PlacedShip(
        string Name,
        int Size,
        List<Coordinate> Coordinates);
}